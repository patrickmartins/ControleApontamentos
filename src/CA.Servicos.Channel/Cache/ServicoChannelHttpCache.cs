using CA.Core.Configuracoes;
using CA.Servicos.Channel.Interfaces;
using CA.Servicos.Channel.Models.Responses;
using Microsoft.Extensions.Caching.Memory;

namespace CA.Servicos.Channel.Cache
{
    public class ServicoChannelHttpCache : IServicoChannelHttp
    {
        private readonly ConfiguracaoClienteChannel _configuracoes;

        private readonly IServicoChannelHttp _servico;
        private readonly IMemoryCache _cache;

        public ServicoChannelHttpCache(ConfiguracaoClienteChannel configuracoes, IServicoChannelHttp servico, IMemoryCache cache)
        {
            _servico = servico;
            _cache = cache;
            _configuracoes = configuracoes;
        }

        public Task<IEnumerable<AtividadeResponse>> ObterAtividadesPorProjetoAsync(int idProjeto)
        {
            return _cache.GetOrCreateAsync($"atividades_channel_{idProjeto}", (e) =>
            {
                e.AbsoluteExpiration = DateTime.Now.AddMinutes(_configuracoes.TempoCacheAtividades);

                return _servico.ObterAtividadesPorProjetoAsync(idProjeto);
            });
        }

        public Task<IEnumerable<ProjetoResponse>> ObterProjetosAsync()
        {
            return _cache.GetOrCreateAsync("projetos_channel", (e) =>
            {
                e.AbsoluteExpiration = DateTime.Now.AddMinutes(_configuracoes.TempoCacheProjetos);

                return _servico.ObterProjetosAsync();
            });
        }

        public Task<IEnumerable<ApontamentoResponse>> ObterApontamentosPorPeriodoAsync(DateOnly inicio, DateOnly fim)
        {
            return _cache.GetOrCreateAsync($"apontamentos_channel_{inicio}_{fim}", (e) =>
            {
                e.AbsoluteExpiration = DateTime.Now.AddMinutes(_configuracoes.TempoCacheApontamentos);

                return _servico.ObterApontamentosPorPeriodoAsync(inicio, fim);
            });
        }

        public Task<IEnumerable<UsuarioResponse>> ObterUsuariosAtivosAsync()
        {
            return _cache.GetOrCreateAsync("usuarios_channel", (e) =>
            {
                e.AbsoluteExpiration = DateTime.Now.AddMinutes(_configuracoes.TempoCacheUsuarios);

                return _servico.ObterUsuariosAtivosAsync();
            });
        }
    }
}
