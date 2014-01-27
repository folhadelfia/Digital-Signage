using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.ServiceModel;
using System.ServiceModel.Discovery;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;

using Assemblies.Toolkit;

using Assemblies.ClientModel;

using Assemblies.ClientProxies;
using Assemblies.DataContracts;
using Assemblies.PlayerServiceContracts;

using Assemblies.Linq;

using System.Threading;

namespace Client
{
    public partial class ServiceList : Form
    {
        private const string SERVERIP_CONST = "10.0.0.165",
                             SERVERPORT_CONST = "8001";

        private Color SELECTED_COLOR = Color.ForestGreen;

        private object oLock = 0;

        private Connection connection = new WCFConnection();

        private string ip, port;

        public WCFPlayerPC PC
        {
            get; //{ return (listViewPCs.SelectedItems.Count == 0 ? null : listViewPCs.SelectedItems[0].Tag as WCFPlayerPC); }
            set;
        }
        public string Screen { get; set; }

        public ServiceList()
        {
            InitializeComponent();

            ip = SERVERIP_CONST;
            port = SERVERPORT_CONST;
        }

        private void configurarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigureForm config = new ConfigureForm(ip, port);
                config.ShowDialog();

                if (config.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    ip = config.IP;
                    port = config.Port;
                }
            }
            catch(Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.TargetSite + Environment.NewLine + ex.Message);
#endif
            }
        }
        private void buttonSearchServices_Click(object sender, EventArgs e) //Com thread
        {
            this.GetPlayersFromDatabase();
        }

        #region A usar discovery server
        //private void ScanPlayersAsync()
        //{
        //    Thread t = new Thread(new ThreadStart(() =>
        //    {
        //        try
        //        {
        //            using (Assemblies.ClientModel.Connection discoveryServerConnection = new WCFConnection()
        //            {
        //                ServerIP = SERVERIP_CONST,
        //                ServerPort = SERVERPORT_CONST
        //            })
        //            {
        //                treeViewRede.Nodes.Clear();
        //                discoveryServerConnection.Open();

        //                discoveryServerConnection.GetPlayersAsync(ScanPlayersCallback);
        //            }
        //        }
        //        catch
        //        {
        //        }
        //    }));

        //    t.Start();
        //}
        //private void ScanPlayersCallback(PlayerPC pc)
        //{
        //    TreeNode nodePC = new TreeNode
        //    {
        //        Text = pc.Name,
        //        ToolTipText = string.Format("IP: {0}", pc.IP),
        //        Tag = pc,
        //        ImageKey = "Computer",
        //        SelectedImageKey = "Computer"
        //    };

        //    foreach (var display in pc.Displays)
        //    {
        //        string tempDispName = string.Format("{0} (X: {1}, Y: {2})", display.Name, display.Bounds.X, display.Bounds.Y);

        //        TreeNode t = new TreeNode()
        //        {
        //            Text = tempDispName,
        //            ToolTipText = string.Format("Resolução: {0}{1}Primário: {2}", display.Bounds.Size.ToString(), Environment.NewLine, (display.Primary ? "Sim" : "Não")),
        //            Tag = display,
        //            ImageKey = "Monitor",
        //            SelectedImageKey = "Monitor"
        //        };

        //        nodePC.Nodes.Add(t);
        //    }


        //    if (this.InvokeRequired)
        //        this.Invoke((MethodInvoker)(() => { treeViewRede.Nodes.Add(nodePC); }));
        //    else
        //        treeViewRede.Nodes.Add(nodePC);
            
        //}
        #endregion

        private void ChoosePlayerFromTreeView()
        {
            if (treeViewRede.SelectedNode != null && treeViewRede.SelectedNode.Tag is WCFScreenInformation && treeViewRede.SelectedNode.ForeColor == SELECTED_COLOR)
            {
                if (connection != null && connection.State != Assemblies.ClientModel.ConnectionState.Closed)
                {
                    connection.Dispose();
                }

                PC = treeViewRede.SelectedNode.Parent.Tag as WCFPlayerPC;

                Screen = (treeViewRede.SelectedNode.Tag as WCFScreenInformation).DeviceID;

                this.buttonOk.PerformClick();
            }


            //foreach (var parentNode in treeViewRede.Nodes.OfType<TreeNode>())
            //{
            //    foreach (var node in parentNode.Nodes.OfType<TreeNode>())
            //    {
            //        node.BackColor = SystemColors.Window;
            //    }
            //}

            //treeViewRede.SelectedNode.BackColor = Color.FromArgb(100, 137, 255, 161);
            //treeViewRede.SelectedNode = null;
        }

        //private void listViewPCs_DoubleClick(object sender, EventArgs e)
        //{
        //    ListView listView = sender as ListView;

        //    if (listView.SelectedItems[0].Tag is IEnumerable<WCFScreenInformation>)
        //        listViewPCs_ItemEntered(listView.SelectedItems[0].Tag as IEnumerable<WCFScreenInformation>);
        //}

        //private void listViewPCs_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        //{
        //    ListView listView = sender as ListView;

        //    if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
        //        if (listView.SelectedItems[0].Tag is IEnumerable<WCFScreenInformation>)
        //            listViewPCs_ItemEntered(listView.SelectedItems[0].Tag as IEnumerable<WCFScreenInformation>);

        //}

        //private void listViewPCs_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        //{

        //}

        private void buttonOk_Click(object sender, EventArgs e)
        {

        }

        private void treeViewRede_DoubleClick(object sender, EventArgs e)
        {
            this.ChoosePlayerFromTreeView();
        }




        int playersFound = 0;
        int playersScanned = 0;

        private void GetPlayersFromDatabase()
        {
            List<Player> players = new List<Player>();

            using (var db = new PlayersLigadosDataContext(LinqConnectionStrings.LigacaoPlayersLigados)) players = db.Players.Where(x => x.isActive).ToList();

            playersFound = players.Count;
            playersScanned = 0;

            List<Clinica> clinicas = new List<Clinica>();

            using (var db = new ClinicasDataContext(LinqConnectionStrings.LigacaoClinicas)) clinicas = db.Clinicas.Where(x => x.isActive ?? false && players.Select(y => y.idClinica).Contains(x.idClinica)).ToList();

            foreach (var player in players)
            {
                string clinicLocation = clinicas.Single(x => x.idClinica == player.idClinica).Localidade;
                string clinicName = clinicas.Single(x => x.idClinica == player.idClinica).Nome;

                string clinicScreenName = string.Format("{0} ({1})", clinicName, clinicLocation);

                BackgroundWorker worker = new BackgroundWorker();

                worker.DoWork += worker_DoWorkAddPlayerToTreeView;
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;

                treeViewRede.Nodes.Clear();
                worker.RunWorkerAsync(new object[] { player, clinicScreenName });
            }
            //using (var db = new ClinicaDataContext(LinqConnectionStrings.LigacaoClinica)) thisClinic = db.ClinicaDados.FirstOrDefault();
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            playersScanned++;

            if (playersFound != 0) //para não dividir por zero, just in case. se o programa chegou aqui é porque à partida é != 0, mas mesmo assim...
            {
                progressBarScan.Value = Convert.ToInt32(Math.Round((playersScanned / playersFound) * 100f));
            }

            buttonScan.Enabled = playersScanned >= playersFound;
            progressBarScan.Visible = !buttonScan.Enabled;
        }

        void worker_DoWorkAddPlayerToTreeView(object sender, DoWorkEventArgs e)
        {
            try
            {
                Player player = (e.Argument as object[])[0] as Player;
                string clinicScreenName = (e.Argument as object[])[1] as string;

                string endpointString = player.Endpoints.Single(x=>x.Type == (int)EndpointTypeEnum.Player).Address;

                if (!(player.publicIPAddress == MyToolkit.Networking.PublicIPAddress.ToString() && MyToolkit.Networking.IsLocal(player.privateIPAddress)))
                    endpointString = this.PrivateToPublicEndpoint(player, EndpointTypeEnum.Player);

                EndpointAddress endpoint = new EndpointAddress(endpointString);

                NetTcpBinding bindingPC = new NetTcpBinding();
                bindingPC.Security.Mode = SecurityMode.None;
                bindingPC.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.None;
                bindingPC.CloseTimeout = new TimeSpan(0, 0, 2);

                PlayerProxy client = new PlayerProxy(bindingPC, endpoint);

                client.Open();

                Dictionary<WCFScreenInformation, bool> displays = new Dictionary<WCFScreenInformation, bool>();

                var tempDisplays = client.GetDisplayInformation().ToList<WCFScreenInformation>();

                foreach (var display in tempDisplays)
                {

                    displays.Add(display, client.PlayerWindowIsOpen(display.DeviceID));
                }

                client.Close();

                if (displays.Count < 1) return;
                lock (oLock)
                {
                    this.Invoke((MethodInvoker)(() =>
                    {
                        WCFPlayerPC pc = new WCFPlayerPC() { Displays = displays.Keys, PlayerEndpoint = new EndpointAddress(endpointString) };

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
                            SelectedImageKey = "Computer"
                        };

                        nodeClinic.Nodes.Add(nodePC);

                        foreach (var display in pc.Displays)
                        {
                            string tempDispName = string.Format("{0} (X: {1}, Y: {2})", display.Name, display.Bounds.X, display.Bounds.Y);
                            bool isOpen = displays[display];
                            

                            TreeNode t = new TreeNode()
                            {
                                Text = tempDispName,
                                ToolTipText = string.Format("Resolução: {0}{1}Primário: {2}", display.Bounds.Size.ToString(), Environment.NewLine, (display.Primary ? "Sim" : "Não")),
                                Tag = display,
                                ImageKey = "Monitor",
                                SelectedImageKey = "Monitor",
                                ForeColor = isOpen ? SELECTED_COLOR : SystemColors.ControlText
                            };

                            nodePC.Nodes.Add(t);
                        }
                        if (newClinicNode)
                            treeViewRede.Nodes.Add(nodeClinic);
                    }));
                }
            }
            catch
            {
            }
        }

        private string PrivateToPublicEndpoint(Player player, EndpointTypeEnum type)
        {
            string result = player.Endpoints.Single(x => x.Type == (int)type).Address;

            result = result.Replace(player.privateIPAddress, player.publicIPAddress);
            result = result.Replace(player.Endpoints.Single(x => x.Type == (int)type).PrivatePort, player.Endpoints.Single(x => x.Type == (int)type).PublicPort);

            return result;


        }
    }
}
