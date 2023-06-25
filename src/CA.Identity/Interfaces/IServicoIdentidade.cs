using CA.Core.Valores;
using CA.Identity.Models;

namespace CA.Identity.Interfaces
{
    public interface IServicoIdentidade
    {
        Task<IEnumerable<UsuarioIdentityModel>> ObterTodasContaUsuarioAsync();
        Task<Resultado<UsuarioIdentityModel>> ObterContaUsuarioPorIdAsync(Guid id);
        Task<Resultado<UsuarioIdentityModel>> ObterContaUsuarioPorEmailAsync(string email);
        Task<Resultado<CaJwt>> LoginAsync(string email);
        Task<Resultado> ExcluirContaUsuarioAsync(string email);
        Task<Resultado<UsuarioIdentityModel>> CriarContaUsuarioAsync(string email);
        Task<Resultado> ConcederAcessoDeAdministradorAsync(string email);
        Task<Resultado> RemoverAcessoDeAdministradorAsync(string email);
    }
}
