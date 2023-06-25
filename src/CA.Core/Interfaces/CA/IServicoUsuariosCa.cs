using CA.Core.Entidades.CA;
using CA.Core.Valores;

namespace CA.Core.Interfaces.CA
{
    public interface IServicoUsuariosCa
    {
        Task<Resultado<UsuarioCA>> AdicionarUsuarioAsync(UsuarioCA usuario);
        Task<Resultado<UsuarioCA>> AtualizarUsuarioAsync(UsuarioCA usuario);
        Task<IEnumerable<UsuarioCA>> ObterTodosUsuariosAsync();
        Task<IEnumerable<UsuarioCA>> ObterTodosGerentesAsync();
        Resultado<UsuarioCA?> ObterUsuarioPorId(string id);
        Resultado<UsuarioCA?> ObterUsuarioPorEmail(string email);
        Task<IEnumerable<UsuarioCA>> ObterUsuariosPorUnidadeAsync(string idUnidade);
        Task<Resultado> ExcluirUsuarioPorIdAsync(string id);        
    }
}
