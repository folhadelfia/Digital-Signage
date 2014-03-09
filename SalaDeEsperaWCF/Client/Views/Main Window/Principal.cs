/*
 * Builder
 * No serviço, ter um método que devolve a resolução do monitor principal onde este está a correr, ou do monitor escolhido nas definições para correr.
 * 
 * 
 * Utilizar threads para fazer as ligações, ou qualquer acção que bloqueie a UI
 * Eliminar a janela ServiceList e utilizar a treeview da página inicial
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;

using Assemblies.Components;
using Assemblies.Configurations;
using Assemblies.Factories;
using Assemblies.ClientModel;
using Assemblies.ClientProxies;
using Assemblies.DataContracts;
using Assemblies.PlayerComponents;
using Assemblies.Options.OptionsGeneral;
using Assemblies.Toolkit;
using Assemblies.XMLSerialization.Components;
using Assemblies.XMLSerialization;
using System.Xml.Serialization;
using Assemblies.PlayerServiceImplementation;

using Assemblies.Linq;
using System.ServiceModel;

namespace Client
{
    public partial class Principal : Form
    {
        #region Connection stuff



        #endregion

        //Dictionary<PlayerPC, Dictionary<ScreenInformation, List<ComposerComponent>>> allPlayersConfiguration = new Dictionary<PlayerPC, Dictionary<ScreenInformation, List<ComposerComponent>>>(); //Ainda não está a ser usado

        private object oLock = 0;

        private List<string> logs = new List<string>();

        private Size finalResolution = Screen.PrimaryScreen.Bounds.Size;

        private bool cancelTreeViewContextMenu = false;

        private System.Windows.Forms.Timer searchPlayersTimer = new System.Windows.Forms.Timer();

        public Principal()
        {
            InitializeComponent();

            this.ConnectionChanged += Principal_ConnectionChanged;
            treeViewRede.NodeMouseClick += (sender, args) => treeViewRede.SelectedNode = args.Node; //Selecciona o nó com o botao direito
            treeViewRede.MouseDown += (sender, args) =>
                {
                    cancelTreeViewContextMenu = args.Button == System.Windows.Forms.MouseButtons.Right && treeViewRede.GetNodeAt(args.Location) == null;
                }; //Só deixa abrir o menu d contexto em cima de um nó

            //searchPlayersTimer.Interval = 10000;
            //searchPlayersTimer.Tick += searchPlayersTimer_Tick;
        }

        //void searchPlayersTimer_Tick(object sender, EventArgs e)
        //{
        //    ScanAndUpdateUI();
        //}

        private void Principal_Load(object sender, EventArgs e)
        {
            try
            {
                #region Popular lista de componentes
                listViewComponents.Items.Add(new ListViewItem() { Text = "Data e hora", Tag = new DateTimeCreator(), ImageIndex = listViewComponents.SmallImageList.Images.IndexOfKey("DateTime") });
                listViewComponents.Items.Add(new ListViewItem() { Text = "Imagem", Tag = new ImageCreator(), ImageIndex = listViewComponents.SmallImageList.Images.IndexOfKey("Image") });
                listViewComponents.Items.Add(new ListViewItem() { Text = "Lista de Espera", Tag = new WaitListCreator(), ImageIndex = listViewComponents.SmallImageList.Images.IndexOfKey("WaitList") });
                listViewComponents.Items.Add(new ListViewItem() { Text = "Meteorologia", Tag = new WeatherCreator(), ImageIndex = listViewComponents.SmallImageList.Images.IndexOfKey("Weather") });
                listViewComponents.Items.Add(new ListViewItem() { Text = "Rodapé", Tag = new MarkeeCreator(), ImageIndex = listViewComponents.SmallImageList.Images.IndexOfKey("Footer") });
                listViewComponents.Items.Add(new ListViewItem() { Text = "Slide Show", Tag = new SlideShowCreator(), ImageIndex = listViewComponents.SmallImageList.Images.IndexOfKey("SlideShow") });
                listViewComponents.Items.Add(new ListViewItem() { Text = "Tabela de Preços", Tag = new PriceListCreator(), ImageIndex = listViewComponents.SmallImageList.Images.IndexOfKey("PriceList") });
                listViewComponents.Items.Add(new ListViewItem() { Text = "TV", Tag = new TVCreator(), ImageIndex = listViewComponents.SmallImageList.Images.IndexOfKey("TV") });
                listViewComponents.Items.Add(new ListViewItem() { Text = "Video", Tag = new VideoCreator(), ImageIndex = listViewComponents.SmallImageList.Images.IndexOfKey("Video") });
                #endregion

                #region Pesquisar periodicamente por players

                this.ScanAndUpdateUI();

                #endregion
            }
            catch(Exception ex)
            {
                Log(ex.Message);
#if DEBUG
                MessageBox.Show(ex.Message);
#endif
            }
        }

        ////////////////////

        #region Drag Drop
        private void listViewComponents_ItemDrag(object sender, ItemDragEventArgs e)
        {
            try
            {
                ListView temp = (ListView)sender;
                List<IComponentCreator> items = new List<IComponentCreator>();

                foreach (ListViewItem aux in temp.SelectedItems)
                    items.Add(aux.Tag as IComponentCreator);

                temp.DoDragDrop(items, DragDropEffects.Copy);
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
#endif
            }
        }

        private void listViewComponents_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                e.Effect = DragDropEffects.None;
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
#endif
            }
        }

        private void panelConfigurador_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                Panel panel = (Panel)sender;

                if (!e.Data.GetDataPresent(typeof(List<IComponentCreator>)))
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }

                List<IComponentCreator> items = (List<IComponentCreator>)e.Data.GetData(typeof(List<IComponentCreator>));

                int counter = 0;
                foreach (IComponentCreator creator in items)
                {
                    var comp = AddComponentToPanel(creator);
                    if(comp != null) comp.Location = panel.PointToClient(new Point(e.X + (counter * 10), e.Y + (counter++ * 10)));
                }

                panel.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
#endif
            }
        }

        private void panelConfigurador_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                e.Effect = DragDropEffects.Copy;
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
#endif
            }
        }
        #endregion

        ////////////////////

        #region ListView

        private void listViewComponents_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                foreach (ListViewItem item in listViewComponents.SelectedItems)
                {
                    AddComponentToPanel(item.Tag as IComponentCreator);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
#endif
            }
        }

        #endregion

        ////////////////////

        #region Panel

        /// <summary>
        /// DoubleClick dos componentes no panel do configurador
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void component_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                (sender as ComposerComponent).OpenOptionsWindow();
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("component_DoubleClick" + Environment.NewLine + ex.Message);
#endif
            }
        }

        private ComposerComponent AddComponentToPanel(IComponentCreator creator, ItemConfiguration config)
        {
            try
            {
                ComposerComponent component = creator.FromConfiguration(config);
                
                Size newSize = new Size();
                Point newLocation = new Point();

                newSize.Width = (config.Size.Width * panelBuilder.Width) / config.Resolution.Width;
                newSize.Height = (config.Size.Height * panelBuilder.Height) / config.Resolution.Height;

                newLocation.X = (config.Location.X * panelBuilder.Width) / config.Resolution.Width;
                newLocation.Y = (config.Location.Y * panelBuilder.Height) / config.Resolution.Height;

                component.Size = newSize;
                component.Location = newLocation;

                component.Configuration.Resolution = panelBuilder.Size;
                component.Configuration.FinalResolution = finalResolution;
                component.Configuration.Size = component.Size;

                component.ContextMenuStrip = contextMenuStripComponents;

                if (component is IUsesConnection)
                    (component as IUsesConnection).SetConnection(Connection);

                component.DoubleClick += component_DoubleClick;

                panelBuilder.Controls.Add(component);

                return component;
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
#endif
                return null;
            }
        }
        private ComposerComponent AddComponentToPanel(IComponentCreator creator)
        {
            try
            {
                if (creator is TVCreator && panelBuilder.Controls.OfType<TVComposer>().Count() > 0) throw new ApplicationException("Já existe um componente de televisão neste monitor");
                if (creator is VideoCreator && panelBuilder.Controls.OfType<VideoComposer>().Count() > 0) throw new ApplicationException("Já existe um componente de vídeo neste monitor"); //mais tarde, dar a hipotese de ter mais do que um video

                ComposerComponent component = creator.Instance;

                component.Configuration.Resolution = panelBuilder.Size;
                component.Configuration.FinalResolution = finalResolution;
                component.Configuration.Size = component.Size;

                component.ContextMenuStrip = contextMenuStripComponents;

                if (component is IUsesConnection)
                    (component as IUsesConnection).SetConnection(Connection); //vai ser preciso fazer o mesmo no video
                else if (component is VideoComposer)
                { 
                    int videoCount = panelBuilder.Controls.OfType<VideoComposer>().Count();

                    ((component as VideoComposer).Configuration as VideoConfiguration).ID = videoCount + 1;

                    ((component as VideoComposer).Configuration as VideoConfiguration).Playlist.Add("C:\\Users\\Cláudio\\Videos\\Wildlife.wmv");
                }

                component.DoubleClick += component_DoubleClick;

                panelBuilder.Controls.Add(component);

                return component;
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
#endif
                return null;
            }
        }

        private void panelBuilder_MouseEnter(object sender, EventArgs e)
        {
            foreach (Control comp in (sender as Panel).Controls)
                comp.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        }
        private void panelBuilder_MouseLeave(object sender, EventArgs e)
        {
            foreach (Control comp in (sender as Panel).Controls)
                comp.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        }

        #endregion

        ////////////////////

        #region Conexões

        private event EventHandler ConnectionChanged;

        private WCFConnection connection;
        private WCFConnection Connection
        {
            get { return connection; }
            set
            {
                connection = value;
                if (ConnectionChanged != null)
                    ConnectionChanged(this, new EventArgs());
            }
        }

        void Principal_ConnectionChanged(object sender, EventArgs e)
        {
            foreach (var component in panelBuilder.Controls.Cast<ComposerComponent>().Where(x => x is IUsesConnection).ToList())
                (component as IUsesConnection).SetConnection(Connection);
        }

        #endregion

        ////////////////////

        #region Context menu componentes

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                panelBuilder.Controls.Remove((((sender as ToolStripMenuItem).Owner as ContextMenuStrip).SourceControl as ComposerComponent));
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
#endif
            }
        }


        private void propriedadesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                (((sender as ToolStripMenuItem).Owner as ContextMenuStrip).SourceControl as ComposerComponent).OpenOptionsWindow();

                #region Nome do videoplayer, fazer depois
                //if ((((sender as ToolStripMenuItem).Owner as ContextMenuStrip).SourceControl as ComposerComponent).Configuration is VideoConfiguration)
                //{
                //    VideoConfiguration cfg = (((sender as ToolStripMenuItem).Owner as ContextMenuStrip).SourceControl as ComposerComponent).Configuration as VideoConfiguration;



                //    if (panelBuilder.Controls.OfType<VideoComposer>().Where(x => (x.Configuration as VideoConfiguration).Name == cfg.Name && (x.Configuration as VideoConfiguration).ID != cfg.ID).Count() > 0) //se o nome já está repetido
                //    {
                //        string tempName = string.Format("{0} (1)", cfg.Name);

                //        if (panelBuilder.Controls.OfType<VideoComposer>().Select(x => (x.Configuration as VideoConfiguration).Name).Contains(tempName))
                //        {
                //            int repeatNumber = 0;

                //            string lastName = panelBuilder.Controls.OfType<VideoComposer>().Select(x => (x.Configuration as VideoConfiguration).Name).Last();

                //            int.TryParse(lastName.Substring(lastName.LastIndexOf('(') + 1, lastName.Length - lastName.LastIndexOf('(') + 1), out repeatNumber);

                //            tempName = string.Format("{0} ({1})", cfg.Name, repeatNumber);
                //        }

                //        cfg.Name = tempName;
                //    }

                //    (((sender as ToolStripMenuItem).Owner as ContextMenuStrip).SourceControl as ComposerComponent).Designation = cfg.Name;
                //}

                #endregion
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("propriedadesToolStripMenuItem_Click" + Environment.NewLine + ex.Message);
#endif
            }
        }

        #endregion

        ////////////////////

        #region Logs

        private void Log(string text)
        {
            logs.Add("[" + DateTime.Now.ToString("HH:mm:ss") + "] " + text);
        }

        #endregion

        ////////////////////

        #region Status

        //statusLines[Group][Title][Values(...)]
        private Dictionary<string, Dictionary<string, List<string>>> statusLines = new Dictionary<string, Dictionary<string, List<string>>>();

        private void SetStatusLine(string group, string title, List<string> values)
        {
            if (!statusLines.Keys.Contains(group)) statusLines.Add(group, new Dictionary<string, List<string>>());

            if (!statusLines[group].Keys.Contains(title)) statusLines[group].Add(title, values);
            else statusLines[group][title] = values;
        }
        private void SetStatusLine(string group, string title, string value)
        {
            SetStatusLine(group, title, new List<string>() { value });
        }
        private void UpdatePlayerStatus()
        {
            if (connection != null)
            {
                if (connection.State != Assemblies.ClientModel.ConnectionState.Open) connection.Open();

                statusLines.Clear();

                SetStatusLine("Geral", "Nome do dispositivo", MyToolkit.Networking.ResolveIP(connection.PlayerIP));
                SetStatusLine("Geral", "Endereço IP", connection.PlayerIP);
                SetStatusLine("Geral", "Monitores", connection.GetDisplayInformation().Length + "");

                foreach (var item in connection.GetDisplayInformation().OrderBy(x => !x.Primary).ThenBy(x => x.Bounds.Left))
                {
                    string tempDisplayName = string.Format("{0} (X: {1}, Y: {2})", item.Name, item.Bounds.X, item.Bounds.Y);

                    SetStatusLine(tempDisplayName, "Resolução", string.Format("{0}x{1}", item.Bounds.Width.ToString(), item.Bounds.Height.ToString()));
                    SetStatusLine(tempDisplayName, "Primário", item.Primary ? "Sim" : "Não");
                    SetStatusLine(tempDisplayName, "Estado", connection.PlayerWindowIsOpen(item.DeviceID) ? "Ligado" : "Desligado");
                }

                foreach (var item in connection.GetTunerDevices().OrderBy(x => x.Name))
                {
                    SetStatusLine("Sintonizadores TV", item.Name, "");
                }
                //Se existirem sintonizadores, mostrar também uma lista com codecs e reprodutores de audio e video
                UpdatePlayerStatusHelper();
            }
        }

        //ATENÇÃO: 

        /// <summary>
        /// Faz o update ao status sem fazer nenhum tipo de verificação no player. Apenas usa o dicionário. Para ser usado nas funções de status
        /// </summary>
        private void UpdatePlayerStatusHelper()
        {
            ListView.ListViewItemCollection old = listViewPlayerStatus.Items;

            try
            {
                listViewPlayerStatus.Items.Clear();

                foreach (var group in statusLines.Keys)
                {
                    if (listViewPlayerStatus.Groups.OfType<ListViewGroup>().Where(x => x.Header == group).Count() == 0)
                        listViewPlayerStatus.Groups.Add(new ListViewGroup() { Header = group });

                    foreach (var title in statusLines[group].Keys)
                    {
                        List<string> values = statusLines[group][title];

                        if (group == "Geral" && title == "Estado")
                            AddStatusLineHelper(group, title, values[0], values[0] == "Ligado" ? Color.ForestGreen : Color.Red);
                        else
                            AddStatusLineHelper(group, title, values);
                    }
                }
            }
            catch
            {
                listViewPlayerStatus.Items.Clear();

                foreach (var item in old)
                    listViewPlayerStatus.Items.Add(item as ListViewItem);
            }

            foreach (ColumnHeader column in listViewPlayerStatus.Columns)
            {
                column.Width = -1;
            }

            //SetGroupCollapse(listViewPlayerStatus, GroupState.COLLAPSIBLE);
        }

        #region Fechar os grupos na listview
        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr window, int message, int wParam, IntPtr lParam);

        private void SetGroupCollapse(ListView target, GroupState state)
        {
            if (target == null) return;
            for (int i = 0; i <= target.Groups.Count; i++)
            {

                LVGROUP group = new LVGROUP();
                group.cbSize = Marshal.SizeOf(group);
                group.state = (int)state; // LVGS_COLLAPSIBLE 
                group.mask = 4; // LVGF_STATE 
                group.iGroupId = i;

                IntPtr ip = IntPtr.Zero;
                try
                {
                    ip = Marshal.AllocHGlobal(group.cbSize);
                    Marshal.StructureToPtr(group, ip, true);
                    SendMessage(target.Handle, 0x1000 + 147, i, ip); // #define LVM_SETGROUPINFO (LVM_FIRST + 147) 
                }

                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                }
                finally
                {
                    if (null != ip) Marshal.FreeHGlobal(ip);
                }
            }

        }
        #endregion

        private ListViewItem AddStatusLineHelper(string group, string title, List<string> values)
        {
            ListViewItem newItem;
            List<ListViewItem.ListViewSubItem> subItems = new List<ListViewItem.ListViewSubItem>();
            try
            {
                newItem = new ListViewItem()
                {
                    Group = listViewPlayerStatus.Groups.OfType<ListViewGroup>().Single(x => x.Header == group),
                    Text = title,
                    ToolTipText = string.Format("{0}{1}", title, !string.IsNullOrWhiteSpace(values[0]) ? " - " + values[0] : "")

                };

                newItem.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Text = values[0]
                });

                for (int i = 1; i < values.Count; i++)
                {
                    subItems.Add(new ListViewItem.ListViewSubItem() { Text = values[i] });
                }
            }
            catch
            {
                return null;
            }

            try
            {
                listViewPlayerStatus.Items.Add(newItem);

                foreach (var subItem in subItems)
                {
                    ListViewItem nextLine = new ListViewItem() { Text = "" };
                    nextLine.SubItems.Add(subItem);

                    listViewPlayerStatus.Items.Add(nextLine);
                }
            }
            catch { return null; }

            return newItem;
        }
        private ListViewItem AddStatusLineHelper(string group, string title, string value)
        {
            return AddStatusLineHelper(group, title, new List<string>() { value });
        }
        private ListViewItem AddStatusLineHelper(string group, string title, string value, Color valueColor)
        {
            ListViewItem item = AddStatusLineHelper(group, title, value);
            item.SubItems[1].ForeColor = valueColor;

            return item;
        }
        #endregion

        ////////////////////

        #region Network Tree

        ScreenInformation selectedScreen;

        private void treeViewRede_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Nodes.Count > 0)
                    foreach (TreeNode node in e.Node.Nodes)
                        node.Checked = e.Node.Checked;

                if (e.Node.Parent != null)
                {
                    e.Node.Parent.Checked = e.Node.Parent.Nodes.Cast<TreeNode>().Where(x => !x.Checked).Count() == 0;
                }
            }
        }

        private void ScanAndUpdateUI()
        {
            buttonScan.Enabled = false;

            progressBarScan.Value = 0;
            progressBarScan.Visible = true;

            this.GetPlayersFromDatabase();

            buttonScan.Enabled = true;
            progressBarScan.Visible = false;
        }
        private void buttonScan_Click(object sender, EventArgs e)
        {
            //Thread t = new Thread(new ThreadStart(ScanPlayers));

            //t.Start();

            this.ScanAndUpdateUI();

        }
        #region A usar discovery server
        ///// <summary>
        ///// Procura no discovery server por players
        ///// </summary>
        //[Obsolete("ScanPlayers é sucatada, usar o ScanPlayersAsync")]
        //private void ScanPlayers()
        //{
        //    if (this.InvokeRequired) this.Invoke((MethodInvoker)(() => { ScanPlayers(); }));
        //    try
        //    {
        //        using (Assemblies.ClientModel.Connection discoveryServerConnection = new WCFConnection()
        //        {
        //            PlayerIP = "10.0.0.165",
        //            ServerPort = "8001"
        //        })
        //        {
        //            treeViewRede.Nodes.Clear();
        //            discoveryServerConnection.Open();

        //            foreach (var pc in discoveryServerConnection.GetPlayers())
        //            {
        //                TreeNode nodePC = new TreeNode
        //                {
        //                    Text = pc.Name,
        //                    ToolTipText = string.Format("IP: {0}", pc.IP),
        //                    Tag = pc,
        //                    ImageKey = "Computer",
        //                    SelectedImageKey = "Computer"
        //                };

        //                foreach (var display in pc.Displays)
        //                {
        //                    TreeNode t = new TreeNode()
        //                    {
        //                        Text = display.Name,
        //                        ToolTipText = string.Format("Resolução: {0}{1}Primário: {2}", display.Bounds.Size.ToString(), Environment.NewLine, (display.Primary ? "Sim" : "Não")),
        //                        Tag = display,
        //                        ImageKey = "Monitor",
        //                        SelectedImageKey = "Monitor"
        //                    };

        //                    nodePC.Nodes.Add(t);
        //                }

        //                treeViewRede.Nodes.Add(nodePC);
        //            }
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}
        //private void ScanPlayersAsync()
        //{
        //    try
        //    {
        //        using (Assemblies.ClientModel.Connection discoveryServerConnection = new WCFConnection()
        //        {
        //            PlayerIP = "10.0.0.165",
        //            ServerPort = "8001"
        //        })
        //        {
        //            treeViewRede.Nodes.Clear();
        //            discoveryServerConnection.Open();

        //            discoveryServerConnection.GetPlayersAsync(ScanPlayersCallback);
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}
        //private void ScanPlayersCallback(PlayerPC pc)
        //{
        //    if (this.InvokeRequired) this.Invoke((MethodInvoker)(() => { this.ScanPlayersCallback(pc); }));
        //    else
        //    {
        //        TreeNode nodePC = new TreeNode
        //        {
        //            Text = pc.Name,
        //            ToolTipText = string.Format("IP: {0}", pc.IP),
        //            Tag = pc,
        //            ImageKey = "Computer",
        //            SelectedImageKey = "Computer"
        //        };

        //        foreach (var display in pc.Displays)
        //        {
        //            string tempDispName = string.Format("{0} (X: {1}, Y: {2})", display.Name, display.Bounds.X, display.Bounds.Y);

        //            TreeNode t = new TreeNode()
        //            {
        //                Text = tempDispName,
        //                ToolTipText = string.Format("Resolução: {0}{1}Primário: {2}", display.Bounds.Size.ToString(), Environment.NewLine, (display.Primary ? "Sim" : "Não")),
        //                Tag = display,
        //                ImageKey = "Monitor",
        //                SelectedImageKey = "Monitor"
        //            };

        //            nodePC.Nodes.Add(t);
        //        }

        //        treeViewRede.Nodes.Add(nodePC);
        //    }
        //}
        #endregion

        int playersFound = 0,
            playersScanned = 0;

        #region Players from database
        private void GetPlayersFromDatabase()
        {
            //Alterar a classe WCFPlayerPC para aceitar mais de um tipo de endpoints, e copiar os dados para lá

            List<Player> players = new List<Player>();
            List<Endpoint> endpoints = new List<Endpoint>();

            using (var db = new PlayersLigadosDataContext(LinqConnectionStrings.LigacaoPlayersLigados))
            {
                foreach (var pl in db.Players.Where(x => x.isActive).ToList())
                {
                    foreach (var ep in pl.Endpoints)
                    {
                        endpoints.Add(ep);
                    }

                    players.Add(pl);
                }
            }

            //using (var db = new PlayersLigadosDataContext(LinqConnectionStrings.LigacaoPlayersLigados)) players = db.Players.Where(x => x.isActive).ToList();

            playersFound = players.Count;
            playersScanned = 0;

            List<Clinica> clinicas = new List<Clinica>();

            using (var db = new ClinicasDataContext(LinqConnectionStrings.LigacaoClinicas)) clinicas = db.Clinicas.Where(x => (x.isActive ?? false) && players.Select(y => y.idClinica).Contains(x.idClinica)).ToList();

            foreach (var player in players)
            {
                string clinicLocation = clinicas.Single(x => x.idClinica == player.idClinica).Localidade;
                string clinicName = clinicas.Single(x => x.idClinica == player.idClinica).Nome;

                string clinicScreenName = string.Format("{0} ({1})", clinicName, clinicLocation);

                BackgroundWorker worker = new BackgroundWorker();

                worker.DoWork += worker_DoWorkAddPlayerToTreeView;
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;

                if (this.InvokeRequired) treeViewRede.Invoke((MethodInvoker)(() =>
                {
                    treeViewRede.Nodes.Clear();
                }));
                else
                    treeViewRede.Nodes.Clear();

                worker.RunWorkerAsync(new object[] { player, endpoints.Where(x=>x.IDPlayer == player.ID).ToList(), clinicScreenName });
            }
            //using (var db = new ClinicaDataContext(LinqConnectionStrings.LigacaoClinica)) thisClinic = db.ClinicaDados.FirstOrDefault();
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            playersScanned++;

            //if (playersFound != 0) //para não dividir por zero, just in case. se o programa chegou aqui é porque à partida é != 0, mas mesmo assim...
            //{
            //    progressBarScan.Value = Convert.ToInt32(Math.Round((playersScanned / playersFound) * 100f));
            //}

            buttonScan.Enabled = playersScanned >= playersFound;
            progressBarScan.Visible = !buttonScan.Enabled;
        }

        void worker_DoWorkAddPlayerToTreeView(object sender, DoWorkEventArgs e)
        {
            try
            {
                Player player = (e.Argument as object[])[0] as Player;
                List<Endpoint> endpoints = (e.Argument as object[])[1] as List<Endpoint>;
                List<WCFScreenInformation> displays = new List<WCFScreenInformation>();

                string clinicScreenName = (e.Argument as object[])[2] as string;

                string playerEndpointString = endpoints.Single(x => x.Type == (int)EndpointTypeEnum.Player).Address,
                       fileTransferEndpointString = endpoints.Single(x => x.Type == (int)EndpointTypeEnum.FileTransfer).Address;

                bool goodPlayer = true;

                if (!(player.publicIPAddress == MyToolkit.Networking.PublicIPAddress.ToString() && MyToolkit.Networking.IsLocal(player.privateIPAddress)))
                {
                    playerEndpointString = this.PrivateToPublicEndpoint(player, EndpointTypeEnum.Player);
                    fileTransferEndpointString = this.PrivateToPublicEndpoint(player, EndpointTypeEnum.FileTransfer);
                }

                EndpointAddress playerEndpoint = new EndpointAddress(playerEndpointString),
                                fileTransferEndpoint = new EndpointAddress(fileTransferEndpointString);
                

                NetTcpBinding bindingPC = new NetTcpBinding();
                bindingPC.Security.Mode = SecurityMode.None;
                bindingPC.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.None;
                bindingPC.CloseTimeout = new TimeSpan(0, 0, 3);

                PlayerProxy client = new PlayerProxy(bindingPC, playerEndpoint);

                client.Open();

                try
                {
                    displays = client.GetDisplayInformation().ToList<WCFScreenInformation>();
                }
                catch (EndpointNotFoundException)
                {
                    goodPlayer = false;
                }

                client.Close();

                lock (oLock)
                {
                    this.Invoke((MethodInvoker)(() =>
                    {
                        //WCFPlayerPC pc = new WCFPlayerPC() { Displays = displays, PlayerEndpoint = new EndpointAddress(playerEndpointString) };
                        WCFPlayerPC pc = new WCFPlayerPC() { Displays = displays, PlayerEndpoint = playerEndpoint, FileTransferEndpoint = fileTransferEndpoint };

                        TreeNode nodeClinic;
                        bool newClinicNode = true;

                        if (treeViewRede.Nodes.OfType<TreeNode>().Where(x => x.Text == clinicScreenName).Count() > 0)
                        {
                            newClinicNode = false;
                            nodeClinic = treeViewRede.Nodes.OfType<TreeNode>().Single(x => x.Text == clinicScreenName);
                        }
                        else
                        {
                            nodeClinic = new TreeNode
                            {
                                Text = clinicScreenName,
                                ImageKey = "Clinic",
                                SelectedImageKey = "Clinic"
                            };
                        }

                        TreeNode nodePC = new TreeNode
                        {
                            Text = player.privateHostname,
                            //ToolTipText = string.Format("IP: {0}", pc.IP),
                            Tag = pc,
                            ImageKey = "Computer",
                            SelectedImageKey = goodPlayer ? "Computer" : "ComputerError",
                            ForeColor = goodPlayer ? SystemColors.ControlText : SystemColors.ControlLight
                        };

                        //Aqui os computadores que estão inacessiveis sao mostrados, deve-se dar a opçao de os desactivar com o builder. Por também a opção de desactivar players inacessiveis sempre que forem encontrados (nao recomendado)

                        nodeClinic.Nodes.Add(nodePC);

                        foreach (var display in pc.Displays)
                        {
                            string tempDispName = string.Format("{0} (X: {1}, Y: {2})", display.Name, display.Bounds.X, display.Bounds.Y);

                            TreeNode t = new TreeNode()
                            {
                                Text = tempDispName,
                                ToolTipText = string.Format("Resolução: {0}{1}Primário: {2}", display.Bounds.Size.ToString(), Environment.NewLine, (display.Primary ? "Sim" : "Não")),
                                Tag = display,
                                ImageKey = "Monitor",
                                SelectedImageKey = "Monitor"
                            };

                            nodePC.Nodes.Add(t);
                        }
                        if(newClinicNode)
                            treeViewRede.Nodes.Add(nodeClinic);
                    }));
                }
            }
            catch(EndpointNotFoundException ex)
            {
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().ToString());

                // normalmente cai aqui quando a ligação não e bem sucedida. atenção às firewalls
            }
            //Tratar EndpointNotFoundException
        }
        #endregion

        private string PrivateToPublicEndpoint(Player player, EndpointTypeEnum type)
        {
            string result = player.Endpoints.Single(x=>x.Type == (int)type).Address;

            result = result.Replace(player.privateIPAddress, player.publicIPAddress);
            result = result.Replace(player.Endpoints.Single(x => x.Type == (int)type).PrivatePort, player.Endpoints.Single(x => x.Type == (int)type).PublicPort);

            return result;


        }

        private void ligarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChoosePlayerFromTreeView();
        }

        private void treeViewRede_DoubleClick(object sender, EventArgs e)
        {
            ChoosePlayerFromTreeView();
        }

        private void ChoosePlayerFromTreeView()
        {
            if (treeViewRede.SelectedNode != null && treeViewRede.SelectedNode.Tag is WCFScreenInformation)
            {
                if (connection != null && connection.State != Assemblies.ClientModel.ConnectionState.Closed)
                {
                    connection.Dispose();
                }

                Connection = new WCFConnection(treeViewRede.SelectedNode.Parent.Tag as WCFPlayerPC);
                Connection.Open();

                selectedScreen = NetWCFConverter.ToNET(treeViewRede.SelectedNode.Tag as WCFScreenInformation);

                UpdatePlayerStatus();
                foreach (var clinicNode in treeViewRede.Nodes.OfType<TreeNode>())
                {
                    foreach (var parentNode in clinicNode.Nodes.OfType<TreeNode>())
                    {
                        foreach (var node in parentNode.Nodes.OfType<TreeNode>())
                        {
                            node.BackColor = SystemColors.Window;
                        }
                    }
                }

                treeViewRede.SelectedNode.BackColor = Color.FromArgb(100, 137, 255, 161);
                treeViewRede.SelectedNode = null;

            }
        }






        #endregion

        ////////////////////

        #region Saves

        /// <summary>
        /// Gets or sets the full path of the current working file
        /// </summary>
        private string CurrentProjectSavePath { get; set; }
        /// <summary>
        /// Gets the directory of the current working file
        /// </summary>
        private string CurrentProjectSaveDirectory
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.CurrentProjectSavePath)) return "";

                return CurrentProjectSavePath.Substring(0, CurrentProjectSavePath.LastIndexOf(@"\") + 1);
            }
        }
        /// <summary>
        /// Gets the filename of the current working file
        /// </summary>
        private string CurrentProjectSaveFileName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.CurrentProjectSavePath)) return "";

                return CurrentProjectSavePath.Substring(CurrentProjectSavePath.LastIndexOf(@"\") + 1, CurrentProjectSavePath.Length - CurrentProjectSavePath.LastIndexOf(@"\") - 1);
            }
        }



        private void SaveProject(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path)) return;

                using (System.IO.FileStream writer = new System.IO.FileStream(path, System.IO.FileMode.Create))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<XMLItemConfiguration>));

                    List<XMLItemConfiguration> temp = new List<XMLItemConfiguration>();

                    foreach (var item in panelBuilder.Controls.OfType<ComposerComponent>().Select(x => x.Configuration).OrderBy(x => x.GetType().ToString()).ToList())
                        temp.Add(NetXMLConverter.ToXML(item));

                    serializer.Serialize(writer, temp);
                    writer.Close();

                    this.CurrentProjectSavePath = path;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("Falha ao gravar o ficheiro {0}", path), ex);
            }
        }
        private IEnumerable<ItemConfiguration> LoadProject(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path)) return null;

                using (System.IO.FileStream reader = new System.IO.FileStream(path, FileMode.Open))
                {
                    XmlSerializer deserializer = new XmlSerializer(typeof(List<XMLItemConfiguration>));

                    List<XMLItemConfiguration> temp = deserializer.Deserialize(reader) as List<XMLItemConfiguration>;

                    reader.Close();

                    List<ItemConfiguration> res = new List<ItemConfiguration>();

                    foreach (var item in temp)
                    {
                        res.Add(NetXMLConverter.ToNET(item));
                    }

                    this.CurrentProjectSavePath = path;

                    return res;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("Falha ao ler o ficheiro {0}", path), ex);
            }
        }


        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.CurrentProjectSavePath))
            {
                SaveFileDialog sfd = new SaveFileDialog();

                DirectoryInfo dir = new System.IO.DirectoryInfo(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Digital Signage"));

                if (!dir.Exists) dir.Create();

                sfd.InitialDirectory = dir.ToString();
                sfd.OverwritePrompt = true;
                sfd.Filter = "Ficheiro XML do Digital Signage|*.dsml";

                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.SaveProject(sfd.FileName);
                }
            }
            else
            {
                this.SaveProject(this.CurrentProjectSavePath);
            }
        }
        private void guardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.FileName = string.IsNullOrWhiteSpace(this.CurrentProjectSaveFileName) ? "Projecto_novo.dsml" : this.CurrentProjectSaveFileName;
            sfd.InitialDirectory = string.IsNullOrWhiteSpace(this.CurrentProjectSaveDirectory) ? System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Digital Signage") : this.CurrentProjectSaveDirectory;
            sfd.OverwritePrompt = true;
            sfd.Filter = "Ficheiro XML do Digital Signage|*.dsml";

            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.SaveProject(sfd.FileName);
            }
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.InitialDirectory = string.IsNullOrWhiteSpace(this.CurrentProjectSaveDirectory) ? System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Digital Signage") : this.CurrentProjectSaveDirectory;

            ofd.Filter = "Ficheiro XML do Digital Signage|*.dsml|Ficheiro XML|*.xml";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                panelBuilder.Controls.Clear();
                IComponentCreator creator;

                foreach (var item in LoadProject(ofd.FileName))
                {
                    if (item is MarkeeConfiguration) creator = new MarkeeCreator();
                    else if (item is TVConfiguration) creator = new TVCreator();
                    else if (item is VideoConfiguration) creator = new VideoCreator();
                    //else if (item is DateTimeConfiguration...
                    else return;

                    object o = this.AddComponentToPanel(creator, item);
                    if (o is Markee) (o as Markee).Run();
                }
            }
        }

        #endregion

        ////////////////////

        List<ScreenInformation> selectedDisplays = new List<ScreenInformation>();

        private void listViewDisplays_SelectedIndexChanged(object sender, EventArgs e)
        {
            var temp = sender as ListView;

            foreach (var item in temp.SelectedItems)
            {
                try
                {
                    temp.SelectedItems.Clear();
                    selectedDisplays.Add(temp.Tag as ScreenInformation);
                }
                catch
                {
                    continue;
                }
            }
        }

        private void computadoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ServiceList lista = new ServiceList();

                if (lista.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Connection = new WCFConnection(lista.PC);

                    Connection.Open();
                }
            }
            catch
            {
            }
        }

        private void buttonFechar_Click(object sender, EventArgs e)
        {
            if (connection == null) return;
            if (Connection.State != Assemblies.ClientModel.ConnectionState.Open) Connection.Open();

            string display = selectedScreen == null ? Connection.GetDisplayInformation().Single(x => x.Primary).DeviceID : selectedScreen.DeviceID;


            if (Connection != null && Connection.State == Assemblies.ClientModel.ConnectionState.Open) Connection.ClosePlayerWindow(display);
            UpdatePlayerStatus();
        }

        private void propriedadesBackgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FormOptionsBuilder options = new FormOptionsBuilder(panelBuilder.Controls.Cast<ComposerComponent>().ToList());

            //options.ShowDialog();

            var backgroundOptWnd = new Assemblies.Options.BackgroundOptions();

            if (backgroundOptWnd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                panelBuilder.BackgroundImage = Image.FromFile(backgroundOptWnd.SelectedPath);
                panelBuilder.BackgroundImageLayout = backgroundOptWnd.BackgroundImageLayout;
            }
        }

        private void listViewPlayerStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            //foreach (ListViewItem item in (sender as ListView).SelectedItems)
            //    item.Selected = false;
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Connection != null && this.Connection.State == Assemblies.ClientModel.ConnectionState.Open)
                this.Connection.Close();

            this.Close();
        }



        private void Principal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (connection != null && connection.State != Assemblies.ClientModel.ConnectionState.Closed)
            {
                connection.Dispose();
                connection = null;
            }
        }

        private void abrirPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirPlayer();
        }

        private void buttonPlayer_Click(object sender, EventArgs e)
        {
            AbrirPlayer();
        }

        private void AbrirPlayer()
        {
            if (connection != null && connection.State != Assemblies.ClientModel.ConnectionState.Open)
                connection.Open();

            if (connection == null || connection.State != Assemblies.ClientModel.ConnectionState.Open) return;

            ScreenInformation display = selectedScreen == null ? Connection.GetDisplayInformation().Single(x => x.Primary) : selectedScreen;

            if (!connection.PlayerWindowIsOpen(display.DeviceID))
            {
                #region PlayerWindowInformation
                PlayerWindowInformation info = new PlayerWindowInformation();

                List<ItemConfiguration> configs = new List<ItemConfiguration>();
                try
                {
                    foreach (var composerComponent in panelBuilder.Controls.OfType<ComposerComponent>())
                    {
                        if(composerComponent is TVComposer)
                        {
                            TVConfiguration tvConfig = (composerComponent as TVComposer).Configuration as TVConfiguration;

                            var devices = connection.GetTunerDevices();
                            //var aDecoders = connection.GetAudioDecoders();
                            var aRenderers = connection.GetAudioRenderers();
                            var hDecoders = connection.GetH264Decoders();
                            var mDecoders = connection.GetMPEG2Decoders();

                            if ((string.IsNullOrWhiteSpace(tvConfig.TunerDevicePath) && devices.Count() > 0) ||
                                //(string.IsNullOrWhiteSpace(tvConfig.AudioDecoder) && aDecoders.Count() > 0) ||
                                (string.IsNullOrWhiteSpace(tvConfig.AudioRenderer) && aRenderers.Count() > 0) ||
                                (string.IsNullOrWhiteSpace(tvConfig.H264Decoder) && hDecoders.Count() > 0) ||
                                (string.IsNullOrWhiteSpace(tvConfig.MPEG2Decoder) && mDecoders.Count() > 0))
                            {
                                DialogResult res = MessageBox.Show("Não configurou o componente de TV." + Environment.NewLine +
                                                   "Deseja fazê-lo agora?" + Environment.NewLine + 
                                                   Environment.NewLine + 
                                                   "Sim - Abre a janela de opções" + Environment.NewLine + 
                                                   "Não - Continua com a abertura do Player (não recomendado" + Environment.NewLine + 
                                                   "Cancelar - Voltar para o Builder",
                                                   "Atenção",
                                                   MessageBoxButtons.YesNoCancel,
                                                   MessageBoxIcon.Warning);
                                if (res == System.Windows.Forms.DialogResult.Yes)
                                {
                                    composerComponent.OpenOptionsWindow();
                                    return;
                                }
                                else if (res == System.Windows.Forms.DialogResult.No)
                                {
                                    continue;
                                }
                                else if (res == System.Windows.Forms.DialogResult.Cancel)
                                {
                                    return;
                                }
                            }
                            else if (devices.Count() == 0 && MessageBox.Show(string.Format("Não existe nenhum sintonizador disponível. Não será possível apresentar televisão.{0}{0}Deseja continuar?", Environment.NewLine), "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No)
                            {
                                throw new ApplicationException("No tuner device selected", new ArgumentNullException());
                            }
                        }

                        configs.Add(composerComponent.Configuration);
                    }


                    info.Components = configs;
                    info.Display = display;
                    info.Background = panelBuilder.BackgroundImage;
                    info.BackgroundImageLayout = panelBuilder.BackgroundImageLayout;
                    /*
                     * 1 - Transformar o info.Display num IEnumerable de displays
                     * 2 - Fazer com que sejam abertas várias janelas nos displays inseridos
                     */

                    Connection.Open();
                    Connection.OpenPlayerWindow(info);
                }
                catch
                {

                    if (connection.State != Assemblies.ClientModel.ConnectionState.Closed) connection.Close();
                }
                #endregion

                #region PlayerWindowInformation2
                //PlayerWindowInformation2 info = new PlayerWindowInformation2();

                //List<ItemConfiguration> configs = new List<ItemConfiguration>();

                //Connection.Open();

                //foreach (var composerComponent in panelBuilder.Controls.Cast<ComposerComponent>())
                //{
                //    configs.Add(composerComponent.Configuration);
                //}

                //info.Configuration
                #endregion

                UpdatePlayerStatus();
            }

            //if (connection.State != Assemblies.ClientModel.ConnectionState.Closed) connection.Close();
        }

        private void contextMenuStripTreeViewRede_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = this.cancelTreeViewContextMenu || !(treeViewRede.SelectedNode.Tag is WCFScreenInformation);
        }
    }

    #region Fechar os grupos na listview

    [StructLayout(LayoutKind.Sequential)]
    public struct LVGROUP
    {
        public int cbSize;
        public int mask;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pszHeader;
        public int cchHeader;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pszFooter;
        public int cchFooter;
        public int iGroupId;
        public int stateMask;
        public int state;
        public int uAlign;
    }

    public enum GroupState
    {
        COLLAPSIBLE = 8,
        COLLAPSED = 1,
        EXPANDED = 0
    } 

    #endregion
}


//Falta a lógica toda desde que se adiciona o componente de video do builder, assim como a janela de opçoes