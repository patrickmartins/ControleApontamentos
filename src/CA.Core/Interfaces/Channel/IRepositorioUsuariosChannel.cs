using CA.Core.Entidades.Channel;

namespace CA.Core.Interfaces.Channel
{
    public interface IRepositorioUsuariosChannel : IRepositorioChannel
    {
        Task InserirUsuarioAsync(UsuarioChannel usuario);
        Task InserirUsuariosAsync(IEnumerable<UsuarioChannel> usuarios);
        void AtualizarUsuario(UsuarioChannel usuario);
        void AtualizarUsuarios(IEnumerable<UsuarioChannel> usuarios);

        Task<IEnumerable<UsuarioChannel>> ObterTodosUsuariosAsync();
        Task<IEnumerable<UsuarioChannel>> ObterUsuariosAtivosAsync();
        UsuarioChannel? ObterUsuarioPorId(int id);
        UsuarioChannel? ObterUsuarioPorEmail(string email);
        UsuarioChannel? ObterUsuarioPorNomeCompleto(string nomeCompleto);
    }
}
