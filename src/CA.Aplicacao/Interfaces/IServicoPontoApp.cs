using CA.Aplicacao.Models;
using CA.Core.Entidades.Ponto;
using CA.Core.Valores;

namespace CA.Aplicacao.Interfaces
{
    public interface IServicoPontoApp
    {
        Task<Resultado<BatidasPontoDiaModel>> ObterBatidasPorDiaAsync(string pisFuncionario, DateOnly data);
        Task<Resultado<BatidasPontoMesModel>> ObterBatidasPorMesAsync(string pisFuncionario, int mes, int ano);
        Task<Resultado<Funcionario?>> ObterFuncionarioPorNomeAsync(string nome);
        Task<IEnumerable<Funcionario>> ObterTodosFuncionariosAsync();
    }
}
