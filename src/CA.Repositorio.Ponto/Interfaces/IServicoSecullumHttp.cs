using CA.Core.Entidades.Ponto;

namespace CA.Repositorios.Ponto.Interfaces
{
    public interface IServicoSecullumHttp
    {
        Task<ICollection<BatidasPontoDia>> ObterBatidasPorPeriodoAsync(string pisFuncionario, DateOnly inicio, DateOnly fim);
        Task<ICollection<Funcionario>> ObterFuncionariosAsync();
    }
}