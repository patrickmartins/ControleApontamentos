using CA.Aplicacao.Models;
using CA.Core.Valores;

namespace CA.Aplicacao.Interfaces
{
    public interface IServicoAdministracaoApp
    {
        Task<IEnumerable<UnidadeModel>> ObterTodasUnidadesAsync();
        Resultado<UnidadeModel> ObterUnidadePorId(Guid id);
        Task<Resultado<UnidadeModel>> AdicionarUnidadeAsync(UnidadeModel model);
        Task<Resultado<UnidadeModel>> AtualizarUnidadeAsync(UnidadeModel model);
    }
}
