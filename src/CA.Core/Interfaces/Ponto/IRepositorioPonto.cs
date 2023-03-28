using CA.Core.Entidades.Ponto;

namespace CA.Core.Interfaces.Ponto
{
    public interface IRepositorioPonto
    {
        Task<IEnumerable<BatidasPontoDia>> ObterBatidasPorPeriodoAsync(string pisFuncionario, DateOnly inicio, DateOnly fim);
        Task<BatidasPontoDia?> ObterBatidasPorDataAsync(string pisFuncionario, DateOnly data);
        Task<Funcionario?> ObterFuncionarioPorNomeAsync(string nome);
        Task<Funcionario?> ObterFuncionarioPorPisAsync(string pisFuncionario);
    }
}
