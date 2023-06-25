using CA.Core.Entidades.CA;

namespace CA.Core.Interfaces.CA
{
    public interface IRepositorioUsuariosCa
    {
        Task InserirUsuarioAsync(UsuarioCA usuario);
        void AtualizarUsuario(UsuarioCA usuario);
        Task<IEnumerable<UsuarioCA>> ObterTodosUsuariosAsync();
        Task<IEnumerable<UsuarioCA>> ObterTodosGerentesAsync();
        UsuarioCA? ObterUsuarioPorId(string id);
        UsuarioCA? ObterUsuarioPorEmail(string email);
        Task<IEnumerable<UsuarioCA>> ObterUsuarioPorUnidadeAsync(string idUnidade);
        void ExcluirUsuario(UsuarioCA usuario);
        Task<int> SalvarAlteracoesAsync();
    }
}
