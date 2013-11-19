using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Assemblies.Components;
using Assemblies.Configurations;

namespace Assemblies.Options.OptionsGeneral
{
    public class OptionsView : UserControl
    {
        protected OptionsView() { }
        protected OptionsView(ItemConfiguration config) { Configuration = config; }
        protected OptionsView(ComposerComponent component) { Configuration = component.Configuration; }

        public ItemConfiguration Configuration
        {
            get
            {
                return GetItemConfiguration();
            }
            set
            {
                SetValues(value);
            }
        }

        public virtual ItemConfiguration GetItemConfiguration() { return null; }
        public virtual void SetValues(ItemConfiguration config) { }
        public virtual void SetValues(ComposerComponent component) { }

        public virtual void ApplyChangesToComponent(ComposerComponent component) {  }
    }
}
