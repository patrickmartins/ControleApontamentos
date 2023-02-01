using CA.Core.Entidades.Ponto;

namespace CA.Servicos.Secullum.Interfaces
{
    public interface IServicoSecullumHttp
    {
        Task<IEnumerable<BatidasPontoDia>> ObterBatidasPorPeriodoAsync(string pisFuncionario, DateOnly inicio, DateOnly fim);
        Task<IEnumerable<Funcionario>> ObterFuncionariosAsync();
    }
}