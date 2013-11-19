using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VideoPlayer
{
    public delegate void SourceChangedEventHandler(object sender, SourceChangedEventArgs e);
    public delegate void VideoAddedEventHandler(object sender, VideoAddedEventArgs e);
    public delegate void VideoRemovedEventHandler(object sender, VideoRemovedEventArgs e);
    public delegate void VideoMovedEventHandler(object sender, VideoMovedEventArgs e);
    public delegate void PlaylistClearedEventHandler(object sender, EventArgs e);


    public class SourceChangedEventArgs : EventArgs
    {
        public string OldSource { get; private set; }
        public string NewSource { get; private set; }

        public SourceChangedEventArgs(string oldSource, string newSource)
        {
            this.OldSource = oldSource;
            this.NewSource = newSource;
        }
    }
    public class VideoAddedEventArgs : EventArgs
    {
        public string Name { get; private set; }
        public int Index { get; private set; }

        public VideoAddedEventArgs(int index, string name) 
        {
            this.Index = index;
            this.Name = name;
        }
    }
    public class VideoRemovedEventArgs : EventArgs
    {
        public string Name { get; private set; }
        public int Index { get; private set; }

        public VideoRemovedEventArgs(int index, string name) 
        {
            this.Index = index;
            this.Name = name;
        } 
    }
    public class VideoMovedEventArgs : EventArgs
    {
        public string Name { get; private set; }
        public int OldIndex { get; private set; }
        public int NewIndex { get; private set; }

        public VideoMovedEventArgs(int oldIndex, int newIndex, string name) 
        {
            this.NewIndex = newIndex;
            this.OldIndex = oldIndex;
            this.Name = name;
        } 
    }
}
