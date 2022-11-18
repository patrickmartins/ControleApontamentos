using CA.Core.Configuracoes;
using CA.Core.Entidades.Ponto;
using CA.Repositorios.Ponto.Interfaces;
using CA.Util.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace CA.Repositorios.Ponto.Cache
{
    public class ServicoSecullumHttpCache : IServicoSecullumHttp
    {
        private readonly ConfiguracaoClienteSecullum _configuracao;

        private readonly IServicoSecullumHttp _servico;
        private readonly IMemoryCache _cache;

        public ServicoSecullumHttpCache(ConfiguracaoClienteSecullum configuracao, IServicoSecullumHttp servico, IMemoryCache cache)
        {
            _servico = servico;
            _cache = cache;
            _configuracao = configuracao;
        }

        public Task<ICollection<BatidasPontoDia>> ObterBatidasPorPeriodoAsync(string pisFuncionario, DateOnly inicio, DateOnly fim)
        {
            return _servico.ObterBatidasPorPeriodoAsync(pisFuncionario, inicio, fim);
        }

        public Task<ICollection<Funcionario>> ObterFuncionariosAsync()
        {
            return _cache.GetOrCreateAsync("funcionarios_secullum", (e) =>
            {
                e.AbsoluteExpiration = DateTime.Now.AddMinutes(_configuracao.TempoCacheFuncionarios);

                return _servico.ObterFuncionariosAsync();
            });
        }
    }
}
