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

using Assemblies.Toolkit;

using Assemblies.ClientModel;

using Assemblies.ClientProxies;
using Assemblies.DataContracts;
using Assemblies.PlayerServiceContracts;

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
            get { return (listViewPCs.SelectedItems.Count == 0 ? null : listViewPCs.SelectedItems[0].Tag as WCFPlayerPC); }
        }

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
        private void buttonSearchServices_Click(object sender, EventArgs e)
        {
            if(!(NetworkingToolkit.ValidateIPAddress(ip) && NetworkingToolkit.ValidatePort(port)))
            {
                configurarToolStripMenuItem.PerformClick();
            }

            listViewPCs.Items.Clear();

            connection.ServerIP = ip;
            connection.ServerPort = port;
            try
            {
                foreach (var pc in connection.GetPlayers())
                {
                    ListViewItem temp = new ListViewItem(pc.IP.ToString());
                    temp.SubItems.Add(pc.Name);
                    temp.SubItems.Add((pc.Displays as List<WCFScreenInformation>).Count.ToString());

                    temp.Tag = pc;

                    listViewPCs.Items.Add(temp);
                }
            }
            catch
            {
            }
        }

        private void listViewPCs_ItemEntered(IEnumerable<WCFScreenInformation> info)
        {

        }

        private void listViewPCs_DoubleClick(object sender, EventArgs e)
        {
            ListView listView = sender as ListView;

            if (listView.SelectedItems[0].Tag is IEnumerable<WCFScreenInformation>)
                listViewPCs_ItemEntered(listView.SelectedItems[0].Tag as IEnumerable<WCFScreenInformation>);
        }

        private void listViewPCs_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            ListView listView = sender as ListView;

            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
                if (listView.SelectedItems[0].Tag is IEnumerable<WCFScreenInformation>)
                    listViewPCs_ItemEntered(listView.SelectedItems[0].Tag as IEnumerable<WCFScreenInformation>);

        }

        private void listViewPCs_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {

        }

        private void buttonOk_Click(object sender, EventArgs e)
        {

        }
    }
}
