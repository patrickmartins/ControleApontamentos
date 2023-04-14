using CA.Servicos.Graph.Entidades;

namespace CA.Core.Interfaces.Microsoft
{
    public interface IServicoMicrosoftGraph
    {
        Task<IEnumerable<Usuario>> ObterTodosUsuariosAsync();
    }
}
