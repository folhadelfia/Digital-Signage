using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using QuartzTypeLib;

namespace VideoPlayer
{
    public class FileVideoPlayer : VideoControl
    {
        /// <summary>
        /// Supported extensions. Includes the dot (.mp4)
        /// </summary>
        /// <returns></returns>
        public static string[] SupportedVideoExtensions() { return new string[] {".avi", ".mpg", ".mpeg", ".mp4", ".mkv", ".wmv" }; }

        public int ID { get; set; }

        public FileVideoPlayer()
        {
            Initialize();
        }
        public FileVideoPlayer(string path)
        {
            this.Source = path;

            Initialize();
        }

        public event EventHandler StateChanged;
        private void OnStateChanged()
        {
            if (StateChanged != null) StateChanged(this, new EventArgs());
        }
        public event SourceChangedEventHandler SourceChanged;
        private void OnSourceChanged(string oldSource, string newSource)
        {
            if (!string.IsNullOrWhiteSpace(newSource) && this.Playlist.Autoplay) this.Restart();
            else if (string.IsNullOrWhiteSpace(newSource)) this.Stop();

            if (SourceChanged != null) SourceChanged(this, new SourceChangedEventArgs(oldSource, newSource));
        }

        #region Guts

        private const int WM_APP = 0x8000;
        private const int WM_GRAPHNOTIFY = WM_APP + 1;
        private const int EC_COMPLETE = 0x01;
        private const int WS_CHILD = 0x40000000;
        private const int WS_CLIPCHILDREN = 0x2000000;

        private FilgraphManager filterGraph = null;
        private IBasicAudio basicAudio = null;
        private IVideoWindow videoWindow = null;
        private IMediaEvent mediaEvent = null;
        private IMediaEventEx mediaEventEx = null;
        private IMediaPosition mediaPosition = null;
        private IMediaControl mediaControl = null;

        #endregion

        public enum MediaStatus { Stopped, Paused, Running };
        private MediaStatus state;
        public MediaStatus State{ 
            get 
            {
                return state == null ? (state = MediaStatus.Stopped) : state;
            }
            private set
            {
                if (value != this.State) OnStateChanged();
                state = value;
            }
        }

        #region Playlist

        public class VideoPlaylist
        {
            internal VideoPlaylist(FileVideoPlayer owner)
            {
                this.owner = owner;
            }
            private FileVideoPlayer owner;
            private Dictionary<int, string> playList = new Dictionary<int, string>();

            public List<string> Files { get { return playList.Values.ToList(); } }

            public bool Replay { get; set; }
            public bool Autoplay { get; set; }

            public event VideoAddedEventHandler VideoAdded;
            public event VideoRemovedEventHandler VideoRemoved;
            public event VideoMovedEventHandler VideoMoved;

            private void OnVideoAdded(int videoNumber, string file)
            {
                if (VideoAdded != null)
                    VideoAdded(this, new VideoAddedEventArgs(videoNumber, file));

                if (Autoplay) this.TrackNumber = videoNumber;
            }
            private void OnVideoRemoved(int videoNumber, string file)
            {
                if (VideoRemoved != null)
                    VideoRemoved(this, new VideoRemovedEventArgs(videoNumber, file));
            }
            private void OnVideoMoved(int oldVideoNumber, int newVideoNumber, string file)
            {
                if (VideoMoved != null)
                    VideoMoved(this, new VideoMovedEventArgs(oldVideoNumber, newVideoNumber, file));
            }

            public int Count { get { return playList.Count; } }
            private int trackNumber = 0;
            public int TrackNumber 
            {
                get { return trackNumber; }
                set  { trackNumber = value; }
            }

            public bool Add(string file)
            {
                return this.Add(file, this.Count);
            }
            public bool Add(string file, int position)
            {
                Dictionary<int, string> backupList = new Dictionary<int, string>();
                foreach (var item in playList) backupList.Add(item.Key, item.Value);

                try
                {
                    Dictionary<int, string> temp = new Dictionary<int, string>();
                    Dictionary<int, string> paraApagar = new Dictionary<int, string>();

                    foreach (var item in playList.Where(x => x.Key >= position))
                    {
                        temp.Add(item.Key + 1, item.Value);
                        paraApagar.Add(item.Key, item.Value);
                    }

                    foreach (var item in paraApagar) playList.Remove(item.Key);

                    playList.Add(position, file);

                    foreach (var item in temp) playList.Add(item.Key, item.Value);

                    if (playList.Count == 1) owner.Source = file;
                }
                catch
                {
                    playList.Clear();
                    foreach (var item in backupList) playList.Add(item.Key, item.Value);
                    return false;
                }

                this.OnVideoAdded(position, file);
                return true;
            }
            public bool Add(string[] files)
            {
                for (int i = 0; i < files.Length; i++) if (!Add(files[i])) return false;

                return true;
            }
            public bool Add(IEnumerable<string> files)
            {
                return Add(files.ToArray());
            }
            public bool Add(string[] files, int position)
            {
                for (int i = 0; i < files.Length; i++) if (!Add(files[i], position++)) return false;

                return true;
            }
            public bool Add(IEnumerable<string> files, int position)
            {
                return Add(files.ToArray(), position);
            }

            public bool Remove(int fileNumber)
            {
                Dictionary<int, string> backupList = new Dictionary<int, string>();
                foreach (var item in playList) backupList.Add(item.Key, item.Value);

                string oldFile = playList[fileNumber];

                try
                {
                    if (!playList.Keys.Contains(fileNumber)) return false; //Não tem um vídeo com aquele num, return false
                    else
                    {
                        if (playList[fileNumber] == owner.Source) //O video a apagar é o seleccionado?
                        {
                            //Sim
                            if (owner.State != MediaStatus.Stopped) //Se o vídeo estiver a correr...
                            {
                                if (fileNumber == playList.Count - 1) //... e o ficheiro for o último na lista...
                                    owner.Source = playList[fileNumber - 1]; //... seleccionar o penúltimo ficheiro na lista...
                                else //... senão...
                                    owner.Source = playList[fileNumber + 1]; //... seleccionar o ficheiro seguinte.

                                //Ao usar o owner.Source, deixo a reprodução ao critério do owner.Autoplay
                            }
                            else //Se o vídeo estiver parado
                            {
                                if (fileNumber == playList.Count - 1) //... e o ficheiro for o último na lista...
                                    owner.fileSource = playList[fileNumber - 1]; //... seleccionar o penúltimo ficheiro na lista...
                                else //... senão...
                                    owner.fileSource = playList[fileNumber + 1]; //... seleccionar o ficheiro seguinte.

                                //Assim evito começar a reprodução se apagar o video seleccionado
                            }
                        }

                        Dictionary<int, string> temp = new Dictionary<int, string>(); //Este dicionário vai ser utilizado para guardar todos os vídeos que estão depois do seleccionado para apagar, para lhes diminuir o videoNumber
                        List<int> paraApagar = new List<int>();

                        for (int i = fileNumber; i < playList.Count - 1; i++) playList[i] = playList[i + 1];

                        playList.Remove(playList.Count - 1);
                    }
                }
                catch
                {
                    playList.Clear();
                    foreach (var item in backupList) playList.Add(item.Key, item.Value);
                    return false;
                }

                OnVideoRemoved(fileNumber, oldFile);
                return true;
            }
            public bool Remove(int[] fileNumbers)
            {
                for (int i = 0; i < fileNumbers.Length; i++) if (!Remove(fileNumbers[i])) return false;

                return true;
            }
            public bool Remove(IEnumerable<int> fileNumbers)
            {
                return Remove(fileNumbers.ToArray());
            }

            public bool Swap(int file1, int file2)
            {
                Dictionary<int, string> backupList = new Dictionary<int, string>();
                foreach (var item in playList) backupList.Add(item.Key, item.Value);

                try
                {
                    if (file1 + file2 >= playList.Count) file2 = playList.Count - file1; //Se quiser mover o ficheiro para uma posição fora da lista por excesso, dar-lhe um offset que o ponha na última pos.
                    else if (file1 + file2 < 0) file2 = -file1; //Se for por defeito, dar-lhe a primeira pos


                    KeyValuePair<int, string> backupItem1 = new KeyValuePair<int, string>(file1, playList[file1]); //1 - Backup
                    KeyValuePair<int, string> backupItem2 = new KeyValuePair<int, string>(file1 + file2, playList[file1 + file2]); //1 - Backup

                    playList.Remove(backupItem1.Key);
                    playList.Remove(backupItem2.Key);

                    playList.Add(backupItem1.Key, backupItem2.Value);
                    playList.Add(backupItem2.Key, backupItem1.Value);
                }
                catch
                {
                    playList.Clear();
                    foreach (var item in backupList) playList.Add(item.Key, item.Value);
                    return false;
                }

                return true;
            }

            public bool Move(int file, int offset)
            {
                if (offset == 0) return true;
                if (!playList.Keys.Contains(file)) return false;

                string movedFile = playList[file];

                Dictionary<int, string> backupList = new Dictionary<int, string>();
                foreach (var item in playList) backupList.Add(item.Key, item.Value);

                try
                {
                    if (file + offset >= playList.Count) offset = playList.Count - file; //Se quiser mover o ficheiro para uma posição fora da lista por excesso, dar-lhe um offset que o ponha na última pos.
                    else if (file + offset < 0) offset = -file; //Se for por defeito, dar-lhe a primeira pos


                    KeyValuePair<int, string> backupItem = new KeyValuePair<int, string>(file, playList[file]); //1 - Backup

                    if (offset > 0)
                    {
                        for (int i = file; i < file + offset; i++) playList[i] = playList[i + 1];
                    }
                    else
                    {
                        for (int i = file; i < file + offset; i--) playList[i] = playList[i - 1];
                    }

                    playList[file + offset] = backupItem.Value;
                }
                catch
                {
                    playList.Clear();
                    foreach (var item in backupList) playList.Add(item.Key, item.Value);
                    return false;
                }

                OnVideoMoved(file, file + offset, movedFile);
                return true;
            }
            public bool Move(int[] files, int offset)
            {
                for (int i = 0; i < files.Length; i++) if (!Move(files[i], offset)) return false;

                return true;
            }
            public bool Move(IEnumerable<int> files, int offset)
            {
                return Move(files.ToArray(), offset);
            }

            public bool Clear()
            {
                Dictionary<int, string> backupList = new Dictionary<int, string>();
                foreach (var item in playList) backupList.Add(item.Key, item.Value);

                try
                {
                    playList.Clear();
                    owner.Source = "";
                }
                catch
                {
                    playList.Clear();
                    foreach (var item in backupList) playList.Add(item.Key, item.Value);
                    return false;
                }

                return true;
            }
        }
        private VideoPlaylist playlist;
        public VideoPlaylist Playlist
        {
            get { return playlist == null ? playlist = new VideoPlaylist(this) : playlist; }
            set { playlist = value; }
        }
        
        #endregion

        protected string fileSource = "";

        public string Source 
        { 
            get { return fileSource; }
            internal set  { fileSource = value; }
        }

        private void Initialize()
        {
            this.State = MediaStatus.Stopped;
            base.BackColor = Color.Black;

            base.SizeChanged += VideoPlayer_SizeChanged;
        }

        private int globalFileErrorCount = 0;
        public void Run()
        {
            if(!string.IsNullOrWhiteSpace(this.Source))
            {
                if (this.State == MediaStatus.Stopped)
                {
                    try
                    {


                        filterGraph = new FilgraphManager();

                        this.filterGraph.RenderFile(this.Source);
                        basicAudio = filterGraph as IBasicAudio;

                        try
                        {
                            videoWindow = filterGraph as IVideoWindow;
                            videoWindow.Owner = (int)this.Handle;
                            videoWindow.WindowStyle = WS_CHILD | WS_CLIPCHILDREN;
                            videoWindow.SetWindowPosition(this.ClientRectangle.Left, this.ClientRectangle.Top, this.ClientRectangle.Width, this.ClientRectangle.Height);
                        }
                        catch (Exception)
                        {
                            videoWindow = null;
                        }

                        mediaEvent = filterGraph as IMediaEvent;

                        mediaEventEx = filterGraph as IMediaEventEx;
                        mediaEventEx.SetNotifyWindow((int)base.Handle, WM_GRAPHNOTIFY, 0);

                        mediaPosition = filterGraph as IMediaPosition;

                        mediaControl = filterGraph as IMediaControl;


                    }
                    catch (COMException ex)
                    {
                        this.Next();
                        this.Run();
                    }
                    catch
                    {
                        try
                        {
                            mediaControl.StopWhenReady();
                        }
                        catch
                        {
                            mediaControl.Stop();
                        }

                        this.CleanUp();
                        this.State = MediaStatus.Stopped;
                        throw;
                    }
                }

                if (this.State != MediaStatus.Running)
                {
                    this.mediaControl.Run();
                    this.State = MediaStatus.Running;
                }
            }
        }
        public void Pause()
        {
            if (this.State == MediaStatus.Running)
            {
                try
                {
                    mediaControl.Pause();
                }
                catch
                {
                    try
                    {
                        mediaControl.StopWhenReady();
                    }
                    catch
                    {
                        mediaControl.Stop();
                    }

                    this.State = MediaStatus.Stopped;
                    throw;
                }

                this.State = MediaStatus.Paused;
            }
        }
        public void Stop()
        {
            if (this.State != MediaStatus.Stopped)
            {
                try
                {
                    mediaControl.StopWhenReady();
                }
                catch
                {
                    mediaControl.Stop();
                    this.State = MediaStatus.Stopped;
                    throw;
                }

                mediaPosition.CurrentPosition = 0;

                this.CleanUp();
                this.State = MediaStatus.Stopped;
            }
        }
        public void Restart()
        {
            this.Stop();
            this.Run();
        }

        public void Next()
        {
            this.Playlist.TrackNumber++;

            if (this.Playlist.TrackNumber >= this.playlist.Count)
            {
                if (this.Playlist.Replay) this.Playlist.TrackNumber = 0;
                else return;
            }

            try
            {
                this.Source = this.Playlist.Files[this.Playlist.TrackNumber];

                if (this.State == MediaStatus.Running) this.Restart();
            }
            catch
            {
                this.Playlist.TrackNumber--;
            }
        }
        public void Previous()
        {
            this.Playlist.TrackNumber--;

            try
            {
                this.Source = this.Playlist.Files[this.Playlist.TrackNumber];

                if (this.State == MediaStatus.Running) this.Restart();
            }
            catch
            {
                this.Playlist.TrackNumber++;
            }
        }

        private void OnMediaEvent(int eventCode)
        {
            switch (eventCode)
            {
                case EC_COMPLETE:
                    this.Next();
                    break;
                default:
                    break;
            }
        }

        void VideoPlayer_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                videoWindow.SetWindowPosition(base.ClientRectangle.Left, base.ClientRectangle.Top, base.Width, base.Height);
            }
            catch
            {
            }
        }

        private void CleanUp()
        {
            base.Parent.Focus();

            if (mediaControl != null)
                mediaControl.Stop();

            this.State = MediaStatus.Stopped;

            if (mediaEventEx != null)
                mediaEventEx.SetNotifyWindow(0, 0, 0);

            if (videoWindow != null)
            {
                videoWindow.Visible = 0;
                //int oldOwner = videoWindow.Owner;
                //videoWindow.Owner = 0;
                //Control.FromHandle((IntPtr)oldOwner).Focus();
            }

            if (mediaControl != null) Marshal.ReleaseComObject(mediaControl); mediaControl = null;
            if (mediaPosition != null) Marshal.ReleaseComObject(mediaPosition); mediaPosition = null;
            if (mediaEventEx != null) Marshal.ReleaseComObject(mediaEventEx); mediaEventEx = null;
            if (mediaEvent != null) Marshal.ReleaseComObject(mediaEvent); mediaEvent = null;
            if (videoWindow != null) Marshal.ReleaseComObject(videoWindow); videoWindow = null;
            if (basicAudio != null) Marshal.ReleaseComObject(basicAudio); basicAudio = null;
            if (filterGraph != null) Marshal.ReleaseComObject(filterGraph); filterGraph = null;

            //if (mediaControl != null) mediaControl = null;
            //if (mediaPosition != null) mediaPosition = null;
            //if (mediaEventEx != null) mediaEventEx = null;
            //if (mediaEvent != null) mediaEvent = null;
            //if (videoWindow != null) videoWindow = null;
            //if (basicAudio != null) basicAudio = null;
            //if (filterGraph != null) filterGraph = null; 
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_GRAPHNOTIFY)
            {
                int lEventCode;
                int lParam1, lParam2;

                while (true)
                {
                    try
                    {
                        mediaEventEx.GetEvent(out lEventCode,
                            out lParam1,
                            out lParam2,
                            0);

                        mediaEventEx.FreeEventParams(lEventCode, lParam1, lParam2);

                        this.OnMediaEvent(lEventCode);
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }
            }

            base.WndProc(ref m);
        }
    }
}
