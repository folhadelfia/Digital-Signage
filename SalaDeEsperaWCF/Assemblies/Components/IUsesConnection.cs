using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assemblies.ClientModel;

namespace Assemblies.Components
{
    public interface IUsesConnection
    {
        void SetConnection(Connection con);
    }
}
