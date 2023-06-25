using CA.Core.Configuracoes;
using CA.Core.Entidades.Ponto;
using CA.Core.Valores;
using CA.Servicos.Secullum.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace CA.Servicos.Secullum.Cache
{
    public class ServicoSecullumHttpCache : IServicoSecullumHttp
    {
        private readonly ConfiguracaoClienteSecullum _configuracoes;

        private readonly IServicoSecullumHttp _servico;
        private readonly IMemoryCache _cache;

        public ServicoSecullumHttpCache(ConfiguracaoClienteSecullum configuracoes, IServicoSecullumHttp servico, IMemoryCache cache)
        {
            _servico = servico;
            _cache = cache;
            _configuracoes = configuracoes;
        }

        public Task<Resultado<IEnumerable<BatidasPontoDia>>> ObterBatidasPorPeriodoAsync(string pisFuncionario, DateOnly inicio, DateOnly fim)
        {
            return _servico.ObterBatidasPorPeriodoAsync(pisFuncionario, inicio, fim);
        }

        public Task<Resultado<IEnumerable<BatidasPontoDia>>> ObterTodasBatidasPorPeriodoAsync(DateOnly inicio, DateOnly fim)
        {
            return _cache.GetOrCreateAsync($"funcionarios_batidas_{inicio}_{fim}", (e) =>
            {
                e.AbsoluteExpiration = DateTime.Now.AddMinutes(_configuracoes.TempoCacheTodasBatidas);

                return _servico.ObterTodasBatidasPorPeriodoAsync(inicio, fim);
            });
        }

        public Task<IEnumerable<Funcionario>> ObterFuncionariosAsync()
        {
            return _cache.GetOrCreateAsync("funcionarios_secullum", (e) =>
            {
                e.AbsoluteExpiration = DateTime.Now.AddMinutes(_configuracoes.TempoCacheFuncionarios);

                return _servico.ObterFuncionariosAsync();
            });
        }
    }
}
