using CA.Core.Configuracoes;
using CA.Core.Entidades.Tfs;
using CA.Repositorios.Tfs.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace CA.Repositorios.Tfs.ServicosHttp
{
    public class ServicoColecaoTfsHttpCache : IServicoColecaoTfsHttp
    {
        private readonly ConfiguracaoClienteTfs _configuracoes;
        private readonly IServicoColecaoTfsHttp _servico;
        private readonly IMemoryCache _cache;

        public ServicoColecaoTfsHttpCache(ConfiguracaoClienteTfs configuracoes, IServicoColecaoTfsHttp servico, IMemoryCache cache)
        {
            configuracoes.Validar();

            _configuracoes = configuracoes;
            _servico = servico;
            _cache = cache;
        }

        public Task<string[]> ObterTodasColecoesAsync()
        {
            return _cache.GetOrCreateAsync("colecoes_tfs", (e) =>
            {
                e.AbsoluteExpiration = DateTime.Now.AddMinutes(_configuracoes.TempoCacheColecoes);

                return _servico.ObterTodasColecoesAsync();
            });
        }

        public Task<string[]> ObterColecoesPorUsuarioAsync(UsuarioTfs usuario)
        {
            return _servico.ObterColecoesPorUsuarioAsync(usuario);
        }
    }
}
