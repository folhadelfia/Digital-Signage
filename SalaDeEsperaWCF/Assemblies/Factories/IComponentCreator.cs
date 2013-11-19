using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Assemblies.Components;

namespace Assemblies.Factories
{
    public interface IComponentCreator
    {
        ComposerComponent Instance
        {
            get;
        }

        ComposerComponent FromConfiguration(Assemblies.Configurations.ItemConfiguration config);
    }
}
