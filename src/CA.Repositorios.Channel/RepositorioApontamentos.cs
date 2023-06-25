using CA.Core.Entidades.Channel;
using CA.Core.Interfaces.Channel;
using CA.Repositorios.Contexto;
using CA.Util.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CA.Repositorios.Channel
{
    public class RepositorioApontamentos : RepositorioBase, IRepositorioApontamentos
    {
        public RepositorioApontamentos(ContextoDadosCA contexto) : base(contexto) { }

        public Task InserirApontamentoAsync(ApontamentoChannel apontamento)
        {
            return InserirAsync(apontamento);
        }

        public Task InserirApontamentosAsync(IEnumerable<ApontamentoChannel> apontamentos)
        {
            return InserirAsync(apontamentos);
        }

        public void AtualizarApontamento(ApontamentoChannel apontamento)
        {
            Atualizar(apontamento);
        }

        public void AtualizarApontamentos(IEnumerable<ApontamentoChannel> apontamentos)
        {
            Atualizar(apontamentos);
        }

        public Task<IEnumerable<ApontamentoChannel>> ObterTodosApontamentosPorPeriodoAsync(DateOnly inicio, DateOnly fim)
        {
            return Set<ApontamentoChannel>()
                        .Include(c => c.Atividade)
                        .Include(c => c.Projeto)
                        .Include(c => c.Usuario)
                        .Where(c => c.Data >= inicio.ToDateTime(new TimeOnly(0)) && c.Data <= fim.ToDateTime(new TimeOnly(0))).ToIListAsync();
        }

        public Task<IEnumerable<ApontamentoChannel>> ObterApontamentosPorDataAsync(int idUsuario, DateOnly data)
        {
            return Set<ApontamentoChannel>()
                        .Include(c => c.Atividade)
                        .Include(c => c.Projeto)
                        .Include(c => c.Usuario)
                        .Where(c => c.Usuario.Id == idUsuario && c.Data == data.ToDateTime(new TimeOnly(0))).ToIListAsync();
        }

        public Task<IEnumerable<ApontamentoChannel>> ObterApontamentosPorPeriodoAsync(int idUsuario, DateOnly inicio, DateOnly fim)
        {
            return Set<ApontamentoChannel>()
                        .Include(c => c.Atividade)
                        .Include(c => c.Projeto)
                        .Include(c => c.Usuario)
                        .Where(c => c.Usuario.Id == idUsuario && c.Data >= inicio.ToDateTime(new TimeOnly(0)) && c.Data <= fim.ToDateTime(new TimeOnly(0))).ToIListAsync();
        }
    }
}
