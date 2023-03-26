using CA.Core.Valores;
using CA.Identity.Models;
using System.Security.Principal;

namespace CA.Identity.Interfaces
{
    public interface IServicoIdentidade
    {
        Task<IEnumerable<UsuarioApp>> ObterTodosUsuariosAsync();
        Task<Resultado<UsuarioApp>> ImportarUsuarioAsync(IPrincipal usuarioLogado);
        Task<Resultado<UsuarioApp>> ObterUsuarioPorEmailAsync(string email);
        Task<Resultado<UsuarioApp>> ObterUsuarioPorIdAsync(Guid id);
        Task<Resultado<CaJwt>> LoginAsync(string email);
        Resultado<CaJwt> Login(UsuarioApp usuario);
    }
}
