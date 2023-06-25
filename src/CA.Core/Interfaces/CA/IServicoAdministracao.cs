using CA.Core.Entidades.CA;
using CA.Core.Valores;

namespace CA.Core.Interfaces.CA
{
    public interface IServicoAdministracao
    {
        Resultado<Unidade?> ObterUnidadePorId(string id);
        Task<IEnumerable<Unidade>> ObterTodasUnidadesAsync();        
        Task<Resultado<Unidade>> AdicionarUnidadeAsync(Unidade unidade);
        Task<Resultado<Unidade>> AtualizarUnidadeAsync(Unidade unidade);
    }
}
