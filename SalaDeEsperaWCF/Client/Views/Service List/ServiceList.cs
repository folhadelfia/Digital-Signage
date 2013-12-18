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
using System.Threading;

namespace Client
{
    public partial class ServiceList : Form
    {
        private const string SERVERIP_CONST = "10.0.0.165",
                             SERVERPORT_CONST = "8001";

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
            this.ScanPlayersAsync();
        }

        private void ScanPlayersAsync()
        {
            Thread t = new Thread(new ThreadStart(() =>
            {
                try
                {
                    using (Assemblies.ClientModel.Connection discoveryServerConnection = new WCFConnection()
                    {
                        ServerIP = SERVERIP_CONST,
                        ServerPort = SERVERPORT_CONST
                    })
                    {
                        treeViewRede.Nodes.Clear();
                        discoveryServerConnection.Open();

                        discoveryServerConnection.GetPlayersAsync(ScanPlayersCallback);
                    }
                }
                catch
                {
                }
            }));

            t.Start();
        }

        private void ScanPlayersCallback(PlayerPC pc)
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


            if (this.InvokeRequired)
                this.Invoke((MethodInvoker)(() => { treeViewRede.Nodes.Add(nodePC); }));
            else
                treeViewRede.Nodes.Add(nodePC);
            
        }


        private void ChoosePlayerFromTreeView()
        {            
            if (treeViewRede.SelectedNode != null && treeViewRede.SelectedNode.Tag is WCFScreenInformation)
            {
                if (connection != null && connection.State != Assemblies.ClientModel.ConnectionState.Closed)
                {
                    connection.Dispose();
                }

                PC = treeViewRede.SelectedNode.Parent.Tag as WCFPlayerPC;

                Screen = (treeViewRede.SelectedNode.Tag as WCFScreenInformation).DeviceID;
            }

            foreach (var parentNode in treeViewRede.Nodes.OfType<TreeNode>())
            {
                foreach (var node in parentNode.Nodes.OfType<TreeNode>())
                {
                    node.BackColor = SystemColors.Window;
                }
            }

            treeViewRede.SelectedNode.BackColor = Color.FromArgb(100, 137, 255, 161);
            treeViewRede.SelectedNode = null;
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
    }
}
