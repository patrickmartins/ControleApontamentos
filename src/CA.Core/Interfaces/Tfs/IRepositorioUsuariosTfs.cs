using CA.Core.Entidades.Tfs;

namespace CA.Core.Interfaces.Tfs
{
    public interface IRepositorioUsuariosTfs : IRepositorioTfs
    {
        Task<IEnumerable<UsuarioTfs>> ObterTodosUsuariosAsync(string colecao);
        Task<UsuarioTfs?> ObterUsuarioPorNomeAsync(string colecao, string usuario);
        Task<UsuarioTfs?> ObterUsuarioPorIdAsync(string colecao, string id);
    }
}