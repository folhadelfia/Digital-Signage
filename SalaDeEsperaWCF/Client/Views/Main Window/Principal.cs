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
using Assemblies.Components;
using Assemblies.Configurations;
using Assemblies.Factories;
using Assemblies.ClientModel;
using Assemblies.ClientProxies;
using Assemblies.DataContracts;
using Assemblies.PlayerComponents;
using Assemblies.Options.OptionsGeneral;
using Assemblies.Toolkit;
using System.Xml.Serialization;
using Assemblies.XMLSerialization.Components;
using Assemblies.XMLSerialization;
using System.IO;
using System.Threading;

namespace Client
{
    public partial class Principal : Form
    {
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

        Dictionary<PlayerPC, Dictionary<ScreenInformation, List<ComposerComponent>>> allPlayersConfiguration = new Dictionary<PlayerPC, Dictionary<ScreenInformation, List<ComposerComponent>>>();

        private List<string> logs = new List<string>();

        private Size finalResolution = Screen.PrimaryScreen.Bounds.Size;

        public Principal()
        {
            InitializeComponent();

            this.ConnectionChanged += Principal_ConnectionChanged;
        }

        void Principal_ConnectionChanged(object sender, EventArgs e)
        {
            foreach (var component in panelBuilder.Controls.Cast<ComposerComponent>().Where(x => x is TVComposer).ToList())
                (component as TVComposer).SetOptionsWindowConnection(Connection);
        }

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
                    AddComponentToPanel(creator).Location = panel.PointToClient(new Point(e.X + (counter * 10), e.Y + (counter++ * 10)));
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

                if (component is TVComposer)
                    (component as TVComposer).SetOptionsWindowConnection(Connection);

                component.DoubleClick += component_DoubleClick;

                panelBuilder.Controls.Add(component);

                return component;
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
                return null;
#endif
            }
        }
        private ComposerComponent AddComponentToPanel(IComponentCreator creator)
        {
            try
            {
                ComposerComponent component = creator.Instance;

                component.Configuration.Resolution = panelBuilder.Size;
                component.Configuration.FinalResolution = finalResolution;
                component.Configuration.Size = component.Size;

                component.ContextMenuStrip = contextMenuStripComponents;

                if (component is TVComposer)
                    (component as TVComposer).SetOptionsWindowConnection(Connection);

                component.DoubleClick += component_DoubleClick;

                panelBuilder.Controls.Add(component);

                return component;
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
                return null;
#endif
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

        #region Serviço

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
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("propriedadesToolStripMenuItem_Click" + Environment.NewLine + ex.Message);
#endif
            }
        }

        #endregion

        #region Logs

        private void Log(string text)
        {
            logs.Add("[" + DateTime.Now.ToString("HH:mm:ss") + "] " + text);
        }

        #endregion

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

        private void buttonPlayer_Click(object sender, EventArgs e)
        {
            ScreenInformation display = selectedScreen == null ? Connection.GetDisplayInformation().Single(x => x.Primary) : selectedScreen;

            if (connection != null && !connection.PlayerWindowIsOpen(display.DeviceID))
            {
                #region PlayerWindowInformation
                PlayerWindowInformation info = new PlayerWindowInformation();

                List<ItemConfiguration> configs = new List<ItemConfiguration>();
                try
                {

                    foreach (var composerComponent in panelBuilder.Controls.OfType<ComposerComponent>())
                    {
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
            string display = selectedScreen == null ? Connection.GetDisplayInformation().Single(x => x.Primary).DeviceID : selectedScreen.DeviceID;


            if (Connection != null && Connection.State == Assemblies.ClientModel.ConnectionState.Open) Connection.ClosePlayerWindow(display);
            UpdatePlayerStatus();
        }

        private void buttonPause_click(object sender, EventArgs e)
        {
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

        #region Status
        //statusLines[Group][Title][Values(...)]
        private Dictionary<string, Dictionary<string, List<string>>> statusLines = new Dictionary<string, Dictionary<string, List<string>>>();

        private void SetStatusLine(string group, string title, List<string> values)
        {
            if(!statusLines.Keys.Contains(group)) statusLines.Add(group, new Dictionary<string, List<string>>());

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
                SetStatusLine("Geral", "Nome do dispositivo", NetworkingToolkit.resolveIP(connection.ServerIP));
                SetStatusLine("Geral", "Endereço IP", connection.ServerIP);
                SetStatusLine("Geral", "Monitores", connection.GetDisplayInformation().Length + "");

                foreach (var item in connection.GetDisplayInformation().OrderBy(x=>!x.Primary).ThenBy(x=>x.Bounds.Left))
                {
                    SetStatusLine(item.DeviceID, "Resolução", string.Format("{0}x{1}", item.Bounds.Width.ToString(), item.Bounds.Height.ToString()));
                    SetStatusLine(item.DeviceID, "Primário", item.Primary ? "Sim" : "Não");
                    SetStatusLine(item.DeviceID, "Estado", connection.PlayerWindowIsOpen(item.DeviceID) ? "Ligado" : "Desligado");
                }
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
        }

        private ListViewItem AddStatusLineHelper(string group, string title, List<string> values)
        {
            ListViewItem newItem;
            List<ListViewItem.ListViewSubItem> subItems = new List<ListViewItem.ListViewSubItem>();
            try
            {
                newItem = new ListViewItem()
                {
                    Group = listViewPlayerStatus.Groups.OfType<ListViewGroup>().Single(x => x.Header == group),
                    Text = title
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

        private void buttonScan_Click(object sender, EventArgs e)
        {
            //Thread t = new Thread(new ThreadStart(ScanPlayers));

            //t.Start();

            this.ScanPlayersAsync();
            
        }
        /// <summary>
        /// Procura no discovery server por players
        /// </summary>
        [Obsolete("ScanPlayers é sucatada, usar o ScanPlayersAsync")]
        private void ScanPlayers()
        {
            if (this.InvokeRequired) this.Invoke((MethodInvoker)(()=> { ScanPlayers(); }));
            try
            {
                using (Assemblies.ClientModel.Connection discoveryServerConnection = new WCFConnection()
                {
                    ServerIP = "10.0.0.165",
                    ServerPort = "8001"
                })
                {
                    treeViewRede.Nodes.Clear();
                    discoveryServerConnection.Open();

                    progressBarScanPlayers.Value = 0;

                    foreach (var pc in discoveryServerConnection.GetPlayers())
                    {
                        TreeNode nodePC = new TreeNode
                        {
                            Text = pc.Name,
                            ToolTipText = string.Format("IP: {0}", pc.IP),
                            Tag = pc,
                            ImageKey = "Computer",
                            SelectedImageKey = "Computer"
                        };

                        foreach (var display in pc.Displays)
                        {
                            TreeNode t = new TreeNode()
                            {
                                Text = display.DeviceID,
                                ToolTipText = string.Format("Resolução: {0}{1}Primário: {2}", display.Bounds.Size.ToString(), Environment.NewLine, (display.Primary ? "Sim" : "Não")),
                                Tag = display,
                                ImageKey = "Monitor",
                                SelectedImageKey = "Monitor"
                            };

                            nodePC.Nodes.Add(t);
                        }

                        treeViewRede.Nodes.Add(nodePC);
                    }
                }
            }
            catch
            {
            }
        }
        private void ScanPlayersAsync()
        {
            try
            {
                using (Assemblies.ClientModel.Connection discoveryServerConnection = new WCFConnection()
                {
                    ServerIP = "10.0.0.165",
                    ServerPort = "8001"
                })
                {
                    treeViewRede.Nodes.Clear();
                    discoveryServerConnection.Open();

                    progressBarScanPlayers.Value = 0;

                    discoveryServerConnection.GetPlayersAsync(ScanPlayersCallback);
                }
            }
            catch
            {
            }
        }
        private void ScanPlayersCallback(PlayerPC pc)
        {
            if (this.InvokeRequired) this.Invoke((MethodInvoker)(() => { this.ScanPlayersCallback(pc); }));
            else
            {
                TreeNode nodePC = new TreeNode
                {
                    Text = pc.Name,
                    ToolTipText = string.Format("IP: {0}", pc.IP),
                    Tag = pc,
                    ImageKey = "Computer",
                    SelectedImageKey = "Computer"
                };

                foreach (var display in pc.Displays)
                {
                    TreeNode t = new TreeNode()
                    {
                        Text = display.DeviceID,
                        ToolTipText = string.Format("Resolução: {0}{1}Primário: {2}", display.Bounds.Size.ToString(), Environment.NewLine, (display.Primary ? "Sim" : "Não")),
                        Tag = display,
                        ImageKey = "Monitor",
                        SelectedImageKey = "Monitor"
                    };

                    nodePC.Nodes.Add(t);
                }

                treeViewRede.Nodes.Add(nodePC);
            }
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
            if (treeViewRede.SelectedNode.Tag is WCFScreenInformation)
            {
                if (connection != null && connection.State != Assemblies.ClientModel.ConnectionState.Closed)
                {
                    connection.Dispose();
                }

                Connection = new WCFConnection(treeViewRede.SelectedNode.Parent.Tag as WCFPlayerPC);
                Connection.Open();

                selectedScreen = NetWCFConverter.ToNET(treeViewRede.SelectedNode.Tag as WCFScreenInformation);

                UpdatePlayerStatus();

                groupBoxPC.Text = string.Format("Ligado a: {0}", (treeViewRede.SelectedNode.Parent.Tag as WCFPlayerPC).Name);
            }
        }






        #endregion

        private void listViewPlayerStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in (sender as ListView).SelectedItems)
                item.Selected = false;
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Connection != null && this.Connection.State == Assemblies.ClientModel.ConnectionState.Open)
                this.Connection.Close();

            this.Close();
        }



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

                    foreach (var item in panelBuilder.Controls.OfType<ComposerComponent>().Select(x => x.Configuration).OrderBy(x=>x.GetType().ToString()).ToList())
                        temp.Add(NetToXMLConverter.ToXML(item));

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

                using(System.IO.FileStream reader = new System.IO.FileStream(path, FileMode.Open))
                {
                    XmlSerializer deserializer = new XmlSerializer(typeof(List<XMLItemConfiguration>));

                    List<XMLItemConfiguration> temp = deserializer.Deserialize(reader) as List<XMLItemConfiguration>;

                    reader.Close();

                    List<ItemConfiguration> res = new List<ItemConfiguration>();

                    foreach (var item in temp)
                    {
                        res.Add(NetToXMLConverter.ToNET(item));
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

                foreach(var item in LoadProject(ofd.FileName))
                {
                    if (item is MarkeeConfiguration) creator = new MarkeeCreator();
                    else if (item is TVConfiguration) creator = new TVCreator();
                    //else if (item is DateTimeConfiguration...
                    else return;

                    object o = this.AddComponentToPanel(creator, item);
                    if (o is Markee) (o as Markee).Run();
                }
            }
        }

        #endregion

        private void Principal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (connection != null && connection.State != Assemblies.ClientModel.ConnectionState.Closed)
            {
                connection.Dispose();
                connection = null;
            }
        }
    }
}