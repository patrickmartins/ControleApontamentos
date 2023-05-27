using CA.Core.Entidades.Channel;
using CA.Core.Interfaces.Channel;
using CA.Repositorios.Channel.Contexto;
using CA.Util.Extensions;

namespace CA.Repositorios.Channel
{
    public class RepositorioUsuariosChannel : RepositorioBase, IRepositorioUsuariosChannel
    {
        public RepositorioUsuariosChannel(ContextoDadosCA contexto) : base(contexto) { }

        public void AtualizarUsuario(UsuarioChannel apontamento)
        {
            Atualizar(apontamento);
        }

        public void AtualizarUsuarios(IEnumerable<UsuarioChannel> usuarios)
        {
            Atualizar(usuarios);
        }

        public Task InserirUsuarioAsync(UsuarioChannel usuario)
        {
            return InserirAsync(usuario);
        }

        public Task InserirUsuariosAsync(IEnumerable<UsuarioChannel> usuarios)
        {
            return InserirAsync(usuarios);
        }

        public Task<IEnumerable<UsuarioChannel>> ObterTodosUsuariosAsync()
        {
            return Set<UsuarioChannel>().ToIListAsync();
        }

        public Task<IEnumerable<UsuarioChannel>> ObterUsuariosAtivosAsync()
        {
            return Set<UsuarioChannel>().Where(C => C.Ativo).ToIListAsync();
        }

        public UsuarioChannel? ObterUsuarioPorId(int id)
        {
            return Set<UsuarioChannel>().FirstOrDefault(c => c.Id == id);
        }

        public UsuarioChannel? ObterUsuarioPorEmail(string email)
        {
            return Set<UsuarioChannel>().FirstOrDefault(c => c.Email.ToLower().Equals(email.ToLower()));
        }

        public UsuarioChannel? ObterUsuarioPorNomeCompleto(string nomeCompleto)
        {
            return Set<UsuarioChannel>().FirstOrDefault(c => c.NomeCompleto.ToLower().Equals(nomeCompleto.ToLower()));
        }
    }
}
