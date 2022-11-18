using CA.Core.Entidades.Tfs;
using CA.Repositorios.Tfs.Helpers;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace CA.Repositorios.Tfs.Configuracoes
{
    internal class ServicoTfsImpersonateInspector : IClientMessageInspector
    {
        private readonly UsuarioTfs _usuario;

        public ServicoTfsImpersonateInspector(UsuarioTfs config)
        {
            _usuario = config;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState) { }

        public object? BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            var binder = channel.GetType().GetProperty("Binder", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).GetValue(channel);
            var endereco = binder.GetType().GetProperty("RemoteAddress", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).GetValue(binder);
            var canal = binder.GetType().GetProperty("Channel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).GetValue(binder);

            if (request.Properties.TryGetValue(HttpRequestMessageProperty.Name, out object httpRequestMessageObject))
            {
                var requestMessage = httpRequestMessageObject as HttpRequestMessageProperty;

                if (requestMessage is not null)
                {
                    requestMessage.Headers["X-TFS-Impersonate"] = $"{_usuario.Identidade.Tipo};{_usuario.Identidade.Id}";
                }
            }
            else
            {
                var requestMessage = new HttpRequestMessageProperty();

                if (requestMessage is not null)
                {
                    requestMessage.Headers.Add("X-TFS-Impersonate", $"{_usuario.Identidade.Tipo};{_usuario.Identidade.Id}");
                }

                request.Properties.Add(HttpRequestMessageProperty.Name, requestMessage);
            }

            return null;
        }
    }
}
