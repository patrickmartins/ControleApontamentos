using CA.Core.Configuracoes;
using CA.Core.Entidades.Tfs;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace CA.Repositorios.Tfs.Configuracoes
{
    internal class ServicoImpersonateBehavior : IEndpointBehavior
    {
        private readonly UsuarioTfs _usuario;

        public ServicoImpersonateBehavior(UsuarioTfs usuario)
        {
            _usuario = usuario;
        }
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters) { }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            var inspector = new ServicoTfsImpersonateInspector(_usuario);

            clientRuntime.ClientMessageInspectors.Add(inspector);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher) { }
        public void Validate(ServiceEndpoint endpoint) { }
    }
}
