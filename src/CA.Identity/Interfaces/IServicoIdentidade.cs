using CA.Core.Valores;
using CA.Identity.Models;

namespace CA.Identity.Interfaces
{
    public interface IServicoIdentidade
    {
        Task<IEnumerable<UsuarioApp>> ObterTodosUsuariosAsync();
        Task<Resultado<UsuarioApp>> ImportarUsuarioAsync(string email, string nomeUsuario, string nomeCompleto);
        Task<Resultado<UsuarioApp>> ObterUsuarioPorEmailAsync(string email);
        Task<Resultado<UsuarioApp>> ObterUsuarioPorIdAsync(Guid id);
        Task<Resultado<UsuarioApp>> ExcluirUsuarioPorIdAsync(Guid id);
        Task<Resultado<CaJwt>> LoginAsync(string email);
        Resultado<CaJwt> Login(UsuarioApp usuario);
    }
}
