﻿using CA.Core.Configuracoes;
using CA.Core.Entidades.Tfs;
using CA.Servicos.Tfs.Interfaces;
using CA.Servicos.Tfs.Models.Requests;
using CA.Servicos.Tfs.Models.Responses;
using Microsoft.Extensions.Caching.Memory;

namespace CA.Servicos.Tfs.Cache
{
    public class ServicoWorkitemTfsSoapCache : IServicoWorkitemTfsSoap
    {
        private readonly ConfiguracaoClienteTfs _configuracoes;
        private readonly IServicoWorkitemTfsSoap _servico;
        private readonly IMemoryCache _cache;

        public ServicoWorkitemTfsSoapCache(ConfiguracaoClienteTfs configuracoes, IServicoWorkitemTfsSoap servico, IMemoryCache cache)
        {
            _cache = cache;
            _servico = servico;
            _configuracoes = configuracoes;
        }

        public Task AtualizarWorkitemAsync(UpdateRequest request, UsuarioTfs usuario, string colecao)
        {
            return _servico.AtualizarWorkitemAsync(request, usuario, colecao);
        }

        public Task<QueryWorkitemsResponse> ExecutarQueryAsync(QueryWorkitemsRequest request, string colecao)
        {
            return _servico.ExecutarQueryAsync(request, colecao);
        }

        public Task<GetMetadataEx2Response> ObterMetadadosPorColecaoAsync(GetMetadataEx2Request request, string colecao)
        {
            return _cache.GetOrCreateAsync($"metadados_colecao_{colecao}", (e) =>
            {
                e.AbsoluteExpiration = DateTime.Now.AddMinutes(_configuracoes.TempoCacheMetadados);

                return _servico.ObterMetadadosPorColecaoAsync(request, colecao);
            });
        }

        public Task<PageWorkitemsByIdsResponse> ObterWorkitemsPorIdsAsync(PageWorkitemsByIdsRequest request, string colecao)
        {
            return _servico.ObterWorkitemsPorIdsAsync(request, colecao);
        }
    }
}
