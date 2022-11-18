using CA.Core.Entidades.Ponto;
using CA.Core.Valores;

namespace CA.Core.Interfaces.Ponto
{
    public interface IServicoPonto
    {
        Task<Resultado<ICollection<BatidasPontoDia>>> ObterBatidasPorPeriodoAsync(string pisFuncionario, DateOnly inicio, DateOnly fim);
        Task<Resultado<BatidasPontoDia?>> ObterBatidasPorDataAsync(string pisFuncionario, DateOnly data);
        Task<Resultado<Funcionario?>> ObterFuncionarioPorNomeAsync(string nome);
    }
}
