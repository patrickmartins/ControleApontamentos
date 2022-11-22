using CA.Core.Valores;
using CA.Seguranca.Entidades;
using System.Security.Principal;

namespace CA.Seguranca.Interfaces
{
    public interface IServicoIdentidade
    {
        Task<Resultado<UsuarioApp>> ImportarUsuarioAsync(IPrincipal usuarioLogado);
        Task<Resultado<UsuarioApp>> ObterUsuarioPorEmailAsync(string email);
        Task<Resultado<CaJwt>> LoginAsync(string email);
        Resultado<CaJwt> Login(UsuarioApp usuario);
    }
}
