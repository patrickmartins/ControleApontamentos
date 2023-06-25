using CA.Aplicacao.Models;
using CA.Core.Valores;

namespace CA.Aplicacao.Interfaces
{
    public interface IServicoUsuariosCaApp
    {
        Task<Resultado<UsuarioModel>> AtualizarUsuarioAsync(AtualizarUsuarioModel usuario);
        Task<IEnumerable<UsuarioModel>> ObterTodosUsuariosAsync();
        Task<IEnumerable<UsuarioModel>> ObterTodosGerentesAsync();
        Resultado<UsuarioModel?> ObterUsuarioPorId(Guid id);
        Task<Resultado> ExcluirUsuarioPorIdAsync(Guid id);
        Task<IEnumerable<UsuarioModel>> ObterUsuariosPorUnidadeAsync(string idUnidade);
        Task<Resultado<UsuarioModel>> ImportarUsuarioAsync(string email, string nomeCompleto);
    }
}
