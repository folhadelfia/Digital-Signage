using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TV2Lib;

using Assemblies.ClientModel;
using System.Threading;

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

            this.textBoxFreq.KeyPress += textBoxFreqs_KeyPress;
            this.textBoxMaxFreq.KeyPress += textBoxFreqs_KeyPress;
            this.textBoxMinFreq.KeyPress += textBoxFreqs_KeyPress;
            this.textBoxSens.KeyPress += textBoxFreqs_KeyPress;

            this.ChannelListUpdated += TVOptions_ChannelListUpdated;

            ToolTip t = new ToolTip();
            t.InitialDelay = 0;
            t.IsBalloon = true;

            t.SetToolTip(this.labelHelpForceScan, "Os canais podem estar guardados num ficheiro, sendo devolvidos para o scan ser mais rápido." + Environment.NewLine + "Se seleccionar esta opção, os canais vão ser obrigatoriamente lidos do sinal de TV");

            textBoxMaxFreq.Text = DigitalTVScreen.ChannelStuff.MAX_FREQUENCY.ToString();
            textBoxMinFreq.Text = DigitalTVScreen.ChannelStuff.MIN_FREQUENCY.ToString();
            textBoxSens.Text = DigitalTVScreen.ChannelStuff.DEFAULT_STEP.ToString();

        }

        void textBoxFreqs_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        //channels[Nome do Canal][Nome do campo][Valor]
        private Dictionary<string, Dictionary<string, string>> channelList = new Dictionary<string, Dictionary<string, string>>();

        void TVOptions_ChannelListUpdated(object sender, ChannelEventArgs e)
        {
            listViewChannels.Items.Clear();

            foreach (var channel in e.ChannelList)
            {
                AddChannelToList(channel as ChannelDVBT);
            }

            UpdateChannelListHelper();
        }

        private void UpdateChannelListHelper()
        {
            if (this.InvokeRequired) this.Invoke((MethodInvoker)(() => { this.UpdateChannelListHelper(); }));
            else
            {
                listViewChannels.BeginUpdate();

                List<ListViewItem> old = new List<ListViewItem>();

                foreach (ListViewItem channel in listViewChannels.Items)
                {
                    old.Add(channel);
                }

                try
                {
                    listViewChannels.Items.Clear();

                    foreach (var group in channelList.Keys)
                    {
                        if (listViewChannels.Groups.OfType<ListViewGroup>().Where(x => x.Header == group).Count() == 0)
                            listViewChannels.Groups.Add(new ListViewGroup() { Header = group });

                        foreach (var title in channelList[group].Keys)
                        {
                            string value = channelList[group][title];

                            AddChannelToListView(group, title, value);
                        }
                    }
                }
                catch
                {
                    listViewChannels.Items.Clear();

                    foreach (var item in old)
                    {
                        listViewChannels.Items.Add(item);
                    }
                }

                foreach (ColumnHeader column in listViewChannels.Columns)
                {
                    column.Width = -1;
                }

                listViewChannels.EndUpdate();
                listViewChannels.Refresh();
            }
        }

        private void AddChannelToList(ChannelDVBT channel)
        {
            AddChannelDetailsToList(channel.Name, "Numero", channel.ChannelNumber.ToString());
            AddChannelDetailsToList(channel.Name, "Formato de video", channel.VideoDecoderType.ToString());
            AddChannelDetailsToList(channel.Name, "Formato de audio", channel.AudioDecoderType.ToString());
        }
        private void AddChannelDetailsToList(string name, string title, string value)
        {
            if (!channelList.Keys.Contains(name)) channelList.Add(name, new Dictionary<string, string>());

            if (!channelList[name].Keys.Contains(title)) channelList[name].Add(title, value);
            else channelList[name][title] = value;
        }

        private void AddChannelToListView(string name, string title, string value)
        {
            //if (this.InvokeRequired) this.Invoke((MethodInvoker)(() => { this.AddChannelToListView(name, title, value); }));

            ListViewItem newItem;

            try
            {
                newItem = new ListViewItem()
                {
                    Group = listViewChannels.Groups.OfType<ListViewGroup>().Single(x=>x.Header == name),
                    Text = title,
                    ToolTipText = string.Format("{0}{1}", title, !string.IsNullOrWhiteSpace(value) ? " - " + value : "")
                };

                newItem.SubItems.Add(new ListViewItem.ListViewSubItem()
                {
                    Text = value
                });

                listViewChannels.Items.Add(newItem);
            }
            catch (Exception)
            {
                return;
            }
        }
        public void AssignConnection(Connection con)
        {
            connection = con;
        }

        private void buttonScanFrequencies_Click(object sender, EventArgs e)
        {
            //if (connection.State != ClientModel.ConnectionState.Open)
            //    connection.Open();
            //ChannelList = connection.GetTVChannels().ToList();

            //connection.Close();

            try
            {
                buttonScanFrequencies.Enabled = false;

                ScanFrequenciesAsync(ChannelListCallbackMethod);
            }
            catch (Exception)
            {
                buttonScanFrequencies.Enabled = true;
                throw;
            }

            //AQUI ESTÁ O TRABALHO!

            /*
             * De acordo com o botao seleccionado, escolher o scan com max e min ou freq fixa (auto ou manual)
             * Adicionar um botão para forçar o rescan
             * Adicionar métodos de pesquisa assíncrona
             * Deixar o utilizador escolher o tuner
             * 
             * Na página dos codecs, carregar os codecs e deixar o utilizador definir codecs automáticos ou manualmente
             * Criar métodos no serviço para enviar e receber os codecs
             * 
             * Alterar o ficheiro de configuração para levar estes dados novos todos (codecs, device, etc)
             * Não é preciso enviar canais porque já lá estão guardados em xml. Caso o xml desapareça, o programa faz sempre o rescan
             */
        }

        private void ChannelListCallbackMethod(IEnumerable<ChannelDVBT> channelList)
        {
            if (this.InvokeRequired) this.Invoke((MethodInvoker)(() => { this.ChannelListCallbackMethod(channelList); }));
            else
            {
                foreach (var ch in channelList)
                    AddChannelToList(ch);

                UpdateChannelListHelper();

                buttonScanFrequencies.Enabled = true;
            }
        }

        private void ScanFrequenciesAsync(ChannelListCallback callback)
        {
            if (callback == null) throw new ArgumentNullException("No callback specified");

            Thread t = new Thread(new ThreadStart((MethodInvoker)(() =>
                {
                    try
                    {
                        if (connection.State != ConnectionState.Open)
                            connection.Open();
                    }
                    catch
                    {
                        return;
                    }

                    IEnumerable<Channel> channels;

                    if (radioButtonAutomatica.Checked)
                    {
                        int maxFreq,
                            minFreq,
                            step;

                        if
                            (int.TryParse(textBoxMinFreq.Text, out minFreq) &&
                            int.TryParse(textBoxMaxFreq.Text, out maxFreq) &&
                            int.TryParse(textBoxSens.Text, out step))
                        {
                            if (maxFreq < minFreq)
                            {
                                MessageBox.Show(
                                    "A frequência máxima não pode ser inferior à mínima",
                                    "Atenção",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);

                                return;
                            }
                            else if
                                (maxFreq > DigitalTVScreen.ChannelStuff.MAX_FREQUENCY ||
                                minFreq < DigitalTVScreen.ChannelStuff.MIN_FREQUENCY)
                            {
                                MessageBox.Show(
                                    string.Format(
                                        "A frequência máxima não pode ser superior a {0} khz e a mínima não pode ser inferior a {1} khz.",
                                        DigitalTVScreen.ChannelStuff.MAX_FREQUENCY,
                                        DigitalTVScreen.ChannelStuff.MIN_FREQUENCY),
                                    "Atenção",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);

                                return;
                            }
                            else
                            {
                                channels = connection.GetTVChannels(device, minFreq, maxFreq, step, checkBoxForceScan.Checked);
                            }
                        }
                        else
                        {
                            MessageBox.Show("As frequências estão num formato não-reconhecido", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        int freq;

                        if (int.TryParse(textBoxFreq.Text, out freq))
                        {
                            if (freq > DigitalTVScreen.ChannelStuff.MAX_FREQUENCY || freq < DigitalTVScreen.ChannelStuff.MIN_FREQUENCY)
                            {
                                MessageBox.Show(
                                string.Format(
                                        "A frequência não pode ser superior a {0} khz nem inferior a {1} khz.",
                                        DigitalTVScreen.ChannelStuff.MAX_FREQUENCY,
                                        DigitalTVScreen.ChannelStuff.MIN_FREQUENCY),
                                    "Atenção",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);

                                return;
                            }
                            else
                            {
                                channels = connection.GetTVChannels(device, freq, checkBoxForceScan.Checked);
                            }
                        }
                        else
                        {
                            MessageBox.Show("A frequência está num formato não-reconhecido", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    List<ChannelDVBT> c = new List<ChannelDVBT>();

                    foreach (var item in channels.OfType<ChannelDVBT>())
                    {
                        c.Add(item);
                    }

                    connection.Close();

                    callback(c);
                })));

            t.Start();
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
            else if (connection == null)
            {
                buttonScanFrequencies.Enabled = false;
                return;
            }

            #region Rede TV
            comboBoxTunerDevices.DisplayMember = "Name";

            comboBoxTunerDevices.Items.Clear();

            IEnumerable<DataContracts.GeneralDevice> allDevices = connection.GetTunerDevices();
            IEnumerable<DataContracts.GeneralDevice> usedDevices = connection.GetTunerDevicesInUse();

            foreach (var item in connection.GetTunerDevices().Where(x=>!usedDevices.Contains(x)))
                comboBoxTunerDevices.Items.Add(item);

            if (comboBoxTunerDevices.Items.Count > 0) comboBoxTunerDevices.SelectedIndex = 0;

            comboBoxTunerDevices.DropDown += comboBox_DropDown_AdjustWidth;

            buttonScanFrequencies.Enabled = comboBoxTunerDevices.SelectedIndex != -1 && comboBoxTunerDevices.Items.Count > 0;
            #endregion

            #region Codecs

            //comboBoxAudioDecoder.DisplayMember = "Name";
            comboBoxAudioRenderer.DisplayMember = "Name";
            comboBoxH264Decoder.DisplayMember = "Name";
            comboBoxMPEG2Codec.DisplayMember = "Name";

            //comboBoxAudioDecoder.Items.Clear();
            //foreach (var item in connection.GetAudioDecoders())
            //    comboBoxAudioDecoder.Items.Add(item);

            comboBoxAudioRenderer.Items.Clear();
            foreach (var item in connection.GetAudioRenderers())
                comboBoxAudioRenderer.Items.Add(item);

            comboBoxH264Decoder.Items.Clear();
            foreach (var item in connection.GetH264Decoders())
                comboBoxH264Decoder.Items.Add(item);

            comboBoxMPEG2Codec.Items.Clear();
            foreach (var item in connection.GetMPEG2Decoders())
                comboBoxMPEG2Codec.Items.Add(item);

            //comboBoxAudioDecoder.DropDown += comboBox_DropDown_AdjustWidth;
            comboBoxAudioRenderer.DropDown += comboBox_DropDown_AdjustWidth;
            comboBoxH264Decoder.DropDown += comboBox_DropDown_AdjustWidth;
            comboBoxMPEG2Codec.DropDown += comboBox_DropDown_AdjustWidth;

            //if (comboBoxAudioDecoder.Items.Count > 0) comboBoxAudioDecoder.SelectedIndex = 0;
            if (comboBoxAudioRenderer.Items.Count > 0) comboBoxAudioRenderer.SelectedIndex = 0;
            if (comboBoxH264Decoder.Items.Count > 0) comboBoxH264Decoder.SelectedIndex = 0;
            if (comboBoxMPEG2Codec.Items.Count > 0) comboBoxMPEG2Codec.SelectedIndex = 0;


            #endregion


            connection.Close();
        }

        void comboBox_DropDown_AdjustWidth(object sender, EventArgs e)
        {
            ComboBox temp = sender as ComboBox;

            foreach (var item in temp.Items)
            {
                int w = TextRenderer.MeasureText(temp.GetItemText(item), temp.Font).Width;

                if (w > temp.DropDownWidth) temp.DropDownWidth = w;
            }
        }

        private void comboBoxTunerDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            device = (comboBoxTunerDevices.SelectedItem as DataContracts.GeneralDevice).DevicePath;

            buttonScanFrequencies.Enabled = (sender as ComboBox).SelectedIndex != -1 && (sender as ComboBox).Items.Count > 0;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                frequency = Convert.ToInt32(textBoxFreq.Text);
            }
            catch
            { frequency = DigitalTVScreen.ChannelStuff.DEFAULT_FREQUENCY; }
        }

        public void ApplyChangesToComponent(Components.ComposerComponent component)
        {
            if (!(component is Components.TVComposer)) return;

            Components.TVComposer temp = component as Components.TVComposer;

            Configurations.TVConfiguration tempConfig = temp.Configuration as Configurations.TVConfiguration;

            tempConfig.Frequency = this.frequency;
            tempConfig.TunerDevicePath = this.device;

            //tempConfig.AudioDecoder = (comboBoxAudioDecoder.SelectedItem as DataContracts.GeneralDevice).DevicePath;
            tempConfig.AudioRenderer = (comboBoxAudioRenderer.SelectedItem as DataContracts.GeneralDevice).DevicePath;
            tempConfig.H264Decoder = (comboBoxH264Decoder.SelectedItem as DataContracts.GeneralDevice).DevicePath;
            tempConfig.MPEG2Decoder = (comboBoxMPEG2Codec.SelectedItem as DataContracts.GeneralDevice).DevicePath;
        }

        private void groupBoxTune_Enter(object sender, EventArgs e)
        {

        }


        private string valueTextBoxMinFreq = "-",
                       valueTextBoxMaxFreq = "-",
                       valueTextBoxSens = "-",
                       valueTextBoxFreq = "-";

        private void radioButtonAutomatica_CheckedChanged(object sender, EventArgs e)
        {
            bool check = (sender as RadioButton).Checked;

            labelMinFreqDesc.Enabled = check;
            labelMaxFreqDesc.Enabled = check;
            labelSensDesc.Enabled = check;
            textBoxMinFreq.Enabled = check;
            textBoxMaxFreq.Enabled = check;
            textBoxSens.Enabled = check;
            labelMinFreqKhz.Enabled = check;
            labelMaxFreqKhz.Enabled = check;
            labelSensKhz.Enabled = check;

            if (check)
            {
                textBoxMinFreq.Text = valueTextBoxMinFreq == "-" ? DigitalTVScreen.ChannelStuff.MIN_FREQUENCY.ToString() : valueTextBoxMinFreq;
                textBoxMaxFreq.Text = valueTextBoxMaxFreq == "-" ? DigitalTVScreen.ChannelStuff.MAX_FREQUENCY.ToString() : valueTextBoxMaxFreq;
                textBoxSens.Text = valueTextBoxSens == "-" ? DigitalTVScreen.ChannelStuff.DEFAULT_STEP.ToString() : valueTextBoxSens;
            }
            else
            {
                valueTextBoxMinFreq = textBoxMinFreq.Text;
                valueTextBoxMaxFreq = textBoxMaxFreq.Text;
                valueTextBoxSens = textBoxSens.Text;

                textBoxMinFreq.Text = textBoxMaxFreq.Text = textBoxSens.Text = "";
            }

            labelFreqDesc.Enabled = !check;
            textBoxFreq.Enabled = !check;
            labelFreqKhz.Enabled = !check;

            if (!check)
            {
                textBoxFreq.Text = valueTextBoxFreq == "-" ? DigitalTVScreen.ChannelStuff.DEFAULT_FREQUENCY.ToString() : valueTextBoxFreq;
            }
            else
            {
                valueTextBoxFreq = textBoxFreq.Text;

                textBoxFreq.Text = "";
            }
        }

        private void buttonDefault_Click(object sender, EventArgs e)
        {
            if (radioButtonAutomatica.Checked)
            {
                textBoxMaxFreq.Text = DigitalTVScreen.ChannelStuff.MAX_FREQUENCY.ToString();
                textBoxMinFreq.Text = DigitalTVScreen.ChannelStuff.MIN_FREQUENCY.ToString();
                textBoxSens.Text = DigitalTVScreen.ChannelStuff.DEFAULT_STEP.ToString();
            }
            else
            {
                textBoxFreq.Text = DigitalTVScreen.ChannelStuff.DEFAULT_FREQUENCY.ToString();
            }
        }
    }

    public delegate void ChannelListCallback(IEnumerable<ChannelDVBT> channelList);
}