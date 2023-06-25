using CA.Core.Entidades.CA;
using CA.Core.Interfaces.Channel;
using CA.Repositorios.Contexto;
using CA.Util.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CA.Repositorios.CA
{
    public class RepositorioAdministracao : RepositorioBase, IRepositorioAdministracao
    {
        public RepositorioAdministracao(ContextoDadosCA contexto) : base(contexto)
        { }

        public void AtualizarUnidade(Unidade unidade)
        {
            Atualizar(unidade);
        }

        public Task InserirUnidadeAsync(Unidade unidade)
        {
            return InserirAsync(unidade);
        }

        public Task<IEnumerable<Unidade>> ObterTodasUnidadesAsync()
        {
            return Set<Unidade>()
                    .ToIListAsync();
        }

        public Unidade? ObterUnidadePorId(string id)
        {
            return Set<Unidade>()
                    .FirstOrDefault(c => c.Id == id);
        }
    }
}
