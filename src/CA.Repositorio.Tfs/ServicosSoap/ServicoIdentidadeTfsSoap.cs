using CA.Core.Configuracoes;
using CA.FlurlSoap;
using CA.Repositorios.Tfs.Constantes;
using CA.Repositorios.Tfs.Interfaces;
using CA.Repositorios.Tfs.ServicosSoap.Models.Requests;
using CA.Repositorios.Tfs.ServicosSoap.Models.Responses;

namespace CA.Repositorios.Tfs.ServicosSoap
{
    public class ServicoIdentidadeTfsSoap : IServicoIdentidadeTfsSoap
    {
        private readonly ConfiguracaoClienteTfs _configuracoes;

        public ServicoIdentidadeTfsSoap(ConfiguracaoClienteTfs configuracoes)
        {
            configuracoes.Validar();

            _configuracoes = configuracoes;
        }

        public Task<ReadIdentitiesResponse> ObterIdentidadeDeUsuarioTfs(ReadIdentitiesRequest request, string colecao)
        {
            var url = $"{_configuracoes.UrlTfs}{colecao}{_configuracoes.UrlServicoIdentidade}";

            return url.WithWindowsAuth(_configuracoes.Usuario, _configuracoes.Senha)
                        .SendXmlSoapAsync(ActionsServicoTfsSoap.ReadIdentities, request)
                        .ReceiveXmlSoapAsync<ReadIdentitiesResponse>();
        }
    }
}
