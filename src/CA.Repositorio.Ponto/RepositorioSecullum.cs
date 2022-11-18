using CA.Core.Configuracoes;
using CA.Core.Entidades.Ponto;
using CA.Core.Interfaces.Ponto;
using CA.Repositorios.Ponto.Interfaces;

namespace CA.Repositorios.Ponto
{
    public class RepositorioSecullum : IRepositorioPonto
    {
        private readonly IServicoSecullumHttp _servico;

        public RepositorioSecullum(IServicoSecullumHttp servico)
        {
            _servico = servico;
        }

        public async Task<BatidasPontoDia?> ObterBatidasPorDataAsync(string pisFuncionario, DateOnly data)
        {
            var batidas = await _servico.ObterBatidasPorPeriodoAsync(pisFuncionario, data, data);

            return batidas.FirstOrDefault();
        }

        public Task<ICollection<BatidasPontoDia>> ObterBatidasPorPeriodoAsync(string pisFuncionario, DateOnly inicio, DateOnly fim)
        {
            return _servico.ObterBatidasPorPeriodoAsync(pisFuncionario, inicio, fim);
        }

        public async Task<Funcionario?> ObterFuncionarioPorNomeAsync(string nome)
        {
            var funcionarios = await _servico.ObterFuncionariosAsync();

            return funcionarios.FirstOrDefault(c => c.Nome.Equals(nome));
        }
    }
}
