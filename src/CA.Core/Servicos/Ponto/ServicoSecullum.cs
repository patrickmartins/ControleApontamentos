using CA.Core.Entidades.Ponto;
using CA.Core.Interfaces.Ponto;
using CA.Core.Valores;

namespace CA.Core.Servicos.Ponto
{
    public class ServicoSecullum : IServicoPonto
    {
        private readonly IRepositorioPonto _repositorio;

        public ServicoSecullum(IRepositorioPonto repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<Resultado<BatidasPontoDia?>> ObterBatidasPorDataAsync(string pisFuncionario, DateOnly data)
        {
            if (string.IsNullOrEmpty(pisFuncionario))
                return Resultado.DeErros<BatidasPontoDia?>(new Erro("O pis do funcionário não foi informado.", nameof(pisFuncionario)));

            var batidas = await _repositorio.ObterBatidasPorDataAsync(pisFuncionario, data);

            return Resultado.DeValor(batidas);
        }

        public async Task<Resultado<ICollection<BatidasPontoDia>>> ObterBatidasPorPeriodoAsync(string pisFuncionario, DateOnly inicio, DateOnly fim)
        {
            if (string.IsNullOrEmpty(pisFuncionario))
                return Resultado.DeErros<ICollection<BatidasPontoDia>>(new Erro("O pis do funcionário não foi informado.", nameof(pisFuncionario)));

            var batidas = (await _repositorio.ObterBatidasPorPeriodoAsync(pisFuncionario, inicio, fim)).OrderBy(c => c.Data).ToList();

            return Resultado.DeValor<ICollection<BatidasPontoDia>>(batidas);
        }

        public async Task<Resultado<Funcionario?>> ObterFuncionarioPorNomeAsync(string nome)
        {
            if (string.IsNullOrEmpty(nome))
                return Resultado.DeErros<Funcionario?>(new Erro("O nome do funcionário não foi informado.", nameof(nome)));

            var funcionario = await _repositorio.ObterFuncionarioPorNomeAsync(nome);

            return Resultado.DeValor(funcionario);
        }
    }
}