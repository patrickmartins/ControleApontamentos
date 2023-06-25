using CA.Core.Entidades.CA;
using CA.Core.Interfaces.CA;
using CA.Repositorios.Contexto;
using CA.Util.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CA.Repositorios.CA
{
    public class RepositorioUsuariosCa : RepositorioBase, IRepositorioUsuariosCa
    {
        public RepositorioUsuariosCa(ContextoDadosCA contexto) : base(contexto)
        { }

        public Task InserirUsuarioAsync(UsuarioCA usuario)
        {
            return InserirAsync(usuario);
        }

        public void AtualizarUsuario(UsuarioCA usuario)
        {
            Atualizar(usuario);
        }

        public void ExcluirUsuario(UsuarioCA usuario)
        {
            Excluir(usuario);
        }

        public Task<IEnumerable<UsuarioCA>> ObterTodosUsuariosAsync()
        {
            return Set<UsuarioCA>()
                    .Include(c => c.ParametrosIntegracoes)
                    .Include(c => c.Unidade)
                    .Include(c => c.Gerente)
                    .ToIListAsync();
        }

        public Task<IEnumerable<UsuarioCA>> ObterTodosGerentesAsync()
        {
            return Set<UsuarioCA>()
                    .Where(c => c.Gerente != null)
                    .Select(c =>  c.Gerente) 
                    .Distinct()
                    .ToIListAsync();
        }

        public UsuarioCA? ObterUsuarioPorEmail(string email)
        {
            return Set<UsuarioCA>()
                    .Include(c => c.ParametrosIntegracoes)
                    .Include(c => c.Unidade)
                    .FirstOrDefault(c => c.Email == email);
        }

        public UsuarioCA? ObterUsuarioPorId(string id)
        {
            return Set<UsuarioCA>()
                    .Include(c => c.ParametrosIntegracoes)
                    .Include(c => c.Unidade)
                    .FirstOrDefault(c => c.Id == id);
        }

        public Task<IEnumerable<UsuarioCA>> ObterUsuarioPorUnidadeAsync(string idUnidade)
        {
            return Set<UsuarioCA>()
                    .Include(c => c.ParametrosIntegracoes)
                    .Include(c => c.Unidade)
                    .Where(c => c.Unidade.Id == idUnidade).ToIListAsync();
        }

    }
}
