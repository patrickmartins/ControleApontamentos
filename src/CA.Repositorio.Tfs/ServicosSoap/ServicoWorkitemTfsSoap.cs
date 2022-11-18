﻿using CA.Core.Configuracoes;
using CA.Core.Entidades.Tfs;
using CA.Core.Interfaces.Tfs;
using CA.FlurlSoap;
using CA.Repositorios.Tfs.Constantes;
using CA.Repositorios.Tfs.Interfaces;
using CA.Repositorios.Tfs.ServicosSoap.Models.Requests;
using CA.Repositorios.Tfs.ServicosSoap.Models.Responses;
using Flurl.Http;
using Polly;
using Polly.Registry;

namespace CA.Repositorios.Tfs.ServicosSoap
{
    public class ServicoWorkitemTfsSoap : IServicoWorkitemTfsSoap
    {
        private readonly ConfiguracaoClienteTfs _configuracoes;
        private readonly IAsyncPolicy _politicaPolly;

        public ServicoWorkitemTfsSoap(ConfiguracaoClienteTfs configuracoes, IPolicyRegistry<string> registry)
        {
            configuracoes.Validar();

            _configuracoes = configuracoes;

            _politicaPolly = registry.Get<IAsyncPolicy>(nameof(IRepositorioTfs));
        }

        public Task AtualizarWorkitemAsync(UpdateRequest request, UsuarioTfs usuario, string colecao)
        {
            var url = $"{_configuracoes.UrlTfs}{colecao}{_configuracoes.UrlServicoItemTrabalho}";

            return url.WithHeader("X-TFS-Impersonate", $"{usuario.Identidade.Tipo};{usuario.Identidade.Id}")
                        .WithWindowsAuth(_configuracoes.Usuario, _configuracoes.Senha)
                        .SendXmlSoapAsync(ActionsServicoTfsSoap.UpdateRequest, request, request.RequestHeader);
        }

        public Task<QueryWorkitemsResponse> ExecutarQueryAsync(QueryWorkitemsRequest request, string colecao)
        {
            var url = $"{_configuracoes.UrlTfs}{colecao}{_configuracoes.UrlServicoItemTrabalho}";

            return _politicaPolly.ExecuteAsync(() =>
            {
                return url.WithWindowsAuth(_configuracoes.Usuario, _configuracoes.Senha)
                            .SendXmlSoapAsync(ActionsServicoTfsSoap.QueryWorkitems, request, request.RequestHeader)
                            .ReceiveXmlSoapAsync<QueryWorkitemsResponse>();
            });
        }

        public Task<GetMetadataEx2Response> ObterMetadadosPorColecaoAsync(GetMetadataEx2Request request, string colecao)
        {
            var url = $"{_configuracoes.UrlTfs}{colecao}{_configuracoes.UrlServicoItemTrabalho}";

            return _politicaPolly.ExecuteAsync(() =>
            {
                return url.WithWindowsAuth(_configuracoes.Usuario, _configuracoes.Senha)
                        .SendXmlSoapAsync(ActionsServicoTfsSoap.GetMetadataEx2Response, request, request.RequestHeader)
                        .ReceiveXmlSoapAsync<GetMetadataEx2Response>();
            });
        }

        public Task<PageWorkitemsByIdsResponse> ObterWorkitemsPorIdsAsync(PageWorkitemsByIdsRequest request, string colecao)
        {
            var url = $"{_configuracoes.UrlTfs}{colecao}{_configuracoes.UrlServicoItemTrabalho}";

            return _politicaPolly.ExecuteAsync(() =>
            {
                return url.WithWindowsAuth(_configuracoes.Usuario, _configuracoes.Senha)
                            .SendXmlSoapAsync(ActionsServicoTfsSoap.PageWorkitemsByIds, request, request.RequestHeader)
                            .ReceiveXmlSoapAsync<PageWorkitemsByIdsResponse>();
            });
        }
    }
}
