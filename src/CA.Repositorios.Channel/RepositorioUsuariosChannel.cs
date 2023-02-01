using CA.Core.Entidades.Channel;
using CA.Core.Interfaces.Channel;
using CA.Repositorios.Channel.Contexto;
using CA.Util.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CA.Repositorios.Channel
{
    public class RepositorioUsuariosChannel : Core.Interfaces.Channel.IRepositorioUsuariosChannel
    {
        private readonly ContextoDadosChannel _contexto;
        private readonly DbSet<UsuarioChannel> _set;

        private bool _disposed;

        public RepositorioUsuariosChannel(ContextoDadosChannel contexto)
        {
            _contexto = contexto;

            _set = contexto.Set<UsuarioChannel>();
        }

        public void AtualizarUsuario(UsuarioChannel apontamento)
        {
            _set.Add(apontamento);
        }

        public void AtualizarUsuarios(IEnumerable<UsuarioChannel> usuarios)
        {
            _set.UpdateRange(usuarios);
        }

        public async Task InserirUsuarioAsync(UsuarioChannel usuario)
        {
            await _set.AddAsync(usuario);
        }

        public async Task InserirUsuariosAsync(IEnumerable<UsuarioChannel> usuarios)
        {
            await _set.AddRangeAsync(usuarios);
        }

        public Task<IEnumerable<UsuarioChannel>> ObterTodosUsuariosAsync()
        {
            return _set.ToIListAsync();
        }

        public UsuarioChannel? ObterUsuarioPorEmail(string email)
        {
            return _set.FirstOrDefault(c => c.Email.Equals(email));
        }

        public UsuarioChannel? ObterUsuarioPorNomeCompleto(string nomeCompleto)
        {
            return _set.FirstOrDefault(c => c.NomeCompleto.Equals(nomeCompleto));
        }

        public Task<int> SalvarAlteracoesAsync()
        {
            return _contexto.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                _contexto.Dispose();

            _disposed = true;
        }
    }
}
