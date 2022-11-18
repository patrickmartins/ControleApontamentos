using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace CA.Repositorios.Tfs.Configuracoes
{
    internal class ChannelFactoryCA<TType> : ChannelFactory<TType>
    {
        public ChannelFactoryCA(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress) { }

        public override TType CreateChannel(EndpointAddress address, Uri via)
        {
            return base.CreateChannel(address, via);
        }
    }
}
