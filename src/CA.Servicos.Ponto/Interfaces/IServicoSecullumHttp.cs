using CA.Core.Entidades.Ponto;
using CA.Core.Valores;

namespace CA.Servicos.Secullum.Interfaces
{
    public interface IServicoSecullumHttp
    {
        Task<Resultado<IEnumerable<BatidasPontoDia>>> ObterBatidasPorPeriodoAsync(string pisFuncionario, DateOnly inicio, DateOnly fim);
        Task<Resultado<IEnumerable<BatidasPontoDia>>> ObterTodasBatidasPorPeriodoAsync(DateOnly inicio, DateOnly fim);
        Task<IEnumerable<Funcionario>> ObterFuncionariosAsync();
    }
}