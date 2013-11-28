using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Assemblies.DataContracts;

namespace Client
{
    public partial class ChooseTVTuner : Form
    {

        public TunerDevice Tuner
        {
            get
            {
                return comboBoxTuners.SelectedItem as TunerDevice;
            }
        }
        private ChooseTVTuner()
        {
            InitializeComponent();
        }

        public ChooseTVTuner(IEnumerable<TunerDevice> devices)
            : this()
        {
            comboBoxTuners.DisplayMember = "Name";

            foreach (var device in devices)
            {
                comboBoxTuners.Items.Add(device);
            }

            comboBoxTuners.SelectedIndex = 0;
        }
    }
}
