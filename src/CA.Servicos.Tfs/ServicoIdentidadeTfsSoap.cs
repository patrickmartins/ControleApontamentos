using CA.Core.Configuracoes;
using CA.FlurlSoap;
using CA.Servicos.Tfs.Constantes;
using CA.Servicos.Tfs.Interfaces;
using CA.Servicos.Tfs.Models.Requests;
using CA.Servicos.Tfs.Models.Responses;

namespace CA.Servicos.Tfs
{
    public class ServicoIdentidadeTfsSoap : IServicoIdentidadeTfsSoap
    {
        private readonly ConfiguracaoClienteTfs _configuracoes;

        public ServicoIdentidadeTfsSoap(ConfiguracaoClienteTfs configuracoes)
        {
            _configuracoes = configuracoes;
        }

        public Task<ReadIdentitiesResponse> BuscarUsuariosTfs(ReadIdentitiesRequest request, string colecao)
        {
            var url = $"{_configuracoes.UrlTfs}{colecao}{_configuracoes.UrlServicoIdentidade}";

            return url.WithWindowsAuth(_configuracoes.Usuario, _configuracoes.Senha)
                        .SendXmlSoapAsync(ActionsServicoTfsSoap.ReadIdentities, request)
                        .ReceiveXmlSoapAsync<ReadIdentitiesResponse>();
        }
    }
}
