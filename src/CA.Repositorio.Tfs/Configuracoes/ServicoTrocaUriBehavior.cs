using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace CA.Repositorios.Tfs.Configuracoes
{
    internal class ServicoTrocaUriBehavior : IEndpointBehavior
    {
        private readonly Uri uri;

        public ServicoTrocaUriBehavior(Uri uri)
        {
            this.uri = uri;
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters) { }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime) { }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            endpoint.Address = new System.ServiceModel.EndpointAddress(uri);
        }

        public void Validate(ServiceEndpoint endpoint) { }
    }
}
