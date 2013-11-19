using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assemblies.Configurations;

namespace Assemblies.Testes
{
    public interface IPlayerComponent
    {
        /// <summary>
        /// Dá a configuração do item para ser passada pela rede. NÂO ESQUECER DE DEFINIR O TAMANHO FINAL
        /// </summary>
        /// <returns></returns>
        ItemConfiguration ExtractConfiguration();
    }
}
