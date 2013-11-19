using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assemblies.Components;

namespace Assemblies.Options
{
    public interface IOptionsWindow
    {
        void ApplyChangesToComponent(ComposerComponent component);
    }
}
