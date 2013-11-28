using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TV2Lib;

using Assemblies.ClientModel;

namespace Assemblies.Options
{
    public partial class TVOptions : Form, IOptionsWindow
    {
        #region Atributos
        private int frequency, onid, tsid, sid;
        private string device;
        private Connection connection;

        private List<Channel> chList;

        public event ChannelEventHandler ChannelListUpdated;

        public List<Channel> ChannelList
        {
            get { return chList ?? new List<Channel>(); }
            set 
            { 
                chList = value;

                if (ChannelListUpdated!=null) ChannelListUpdated(this, new ChannelEventArgs(value)); 
            }
        }

        #endregion

        public TVOptions()
        {
            InitializeComponent();

            this.textBoxFreq.KeyPress += textBoxFreq_KeyPress;

            this.ChannelListUpdated += TVOptions_ChannelListUpdated;
        }

        void textBoxFreq_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        void TVOptions_ChannelListUpdated(object sender, ChannelEventArgs e)
        {
            listBoxAllChannels.Items.Clear();

            foreach (var channel in e.ChannelList)
            {
                listBoxAllChannels.Items.Add(channel);
            }
        }

        public void AssignConnection(Connection con)
        {
            connection = con;
        }

        private void buttonScanFrequencies_Click(object sender, EventArgs e)
        {
            if (connection.State != ClientModel.ConnectionState.Open)
                connection.Open();
            ChannelList = connection.GetTVChannels().ToList();

            connection.Close();
        }

        private void listBoxAllChannels_MouseDown(object sender, MouseEventArgs e)
        {
            var listBox = sender as ListBox;

            if (e.Button != System.Windows.Forms.MouseButtons.Left ||
                listBox.Items.Count == 0) return;

            for (int i = 0; i < listBox.Items.Count; i++)
            {
                if (listBox.GetItemRectangle(i).Contains(e.Location)) return;
            }

            listBox.ClearSelected();
        }

        private void TVOptions_Load(object sender, EventArgs e)
        {
            if (connection != null && connection.State != ConnectionState.Open) connection.Open();
            else if(connection==null) return;

            comboBoxTunerDevices.DisplayMember = "Name";

            comboBoxTunerDevices.Items.Clear();

            IEnumerable<DataContracts.TunerDevice> allDevices = connection.GetTunerDevices();
            IEnumerable<DataContracts.TunerDevice> usedDevices = connection.GetTunerDevicesInUse();

            foreach (var item in connection.GetTunerDevices().Where(x=>!usedDevices.Contains(x)))
                comboBoxTunerDevices.Items.Add(item);

            connection.Close();
        }

        private void comboBoxTunerDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            device = (comboBoxTunerDevices.SelectedItem as DataContracts.TunerDevice).DevicePath;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                frequency = Convert.ToInt32(textBoxFreq.Text);
            }
            catch
            { frequency = 754000; }
        }

        public void ApplyChangesToComponent(Components.ComposerComponent component)
        {
            if (!(component is Components.TVComposer)) return;

            Components.TVComposer temp = component as Components.TVComposer;

            (temp.Configuration as Configurations.TVConfiguration).Frequency = this.frequency;
            (temp.Configuration as Configurations.TVConfiguration).TunerDevicePath = this.device;

            
        }
    }
}