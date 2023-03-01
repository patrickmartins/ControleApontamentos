using CA.Core.Entidades.Channel;
using CA.Core.Interfaces.Channel;
using CA.Repositorios.Channel.Contexto;
using CA.Util.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CA.Repositorios.Channel
{
    public class RepositorioApontamentos : IRepositorioApontamentos
    {
        private readonly ContextoDadosChannel _contexto;
        private readonly DbSet<ApontamentoChannel> _set;
        private bool _disposed;

        public RepositorioApontamentos(ContextoDadosChannel contexto)
        {
            _contexto = contexto;

            _set = contexto.Set<ApontamentoChannel>();
        }

        public async Task InserirApontamentoAsync(ApontamentoChannel apontamento)
        {
            await _set.AddAsync(apontamento);
        }

        public async Task InserirApontamentosAsync(IEnumerable<ApontamentoChannel> apontamentos)
        {
            await _set.AddRangeAsync(apontamentos);
        }

        public void AtualizarApontamento(ApontamentoChannel apontamento)
        {
            _set.Update(apontamento);
        }

        public void AtualizarApontamentos(IEnumerable<ApontamentoChannel> apontamentos)
        {
            _set.UpdateRange(apontamentos);
        }

        public void RemoverApontamento(ApontamentoChannel apontamento)
        {
            _set.Remove(apontamento);
        }

        public void RemoverApontamentos(IEnumerable<ApontamentoChannel> apontamentos)
        {
            _set.RemoveRange(apontamentos);
        }

        public Task<IEnumerable<ApontamentoChannel>> ObterTodosApontamentosPorPeriodoAsync(DateOnly inicio, DateOnly fim)
        {
            return _set.IgnoreQueryFilters().Where(c => c.Data >= inicio.ToDateTime(new TimeOnly(0)) && c.Data <= fim.ToDateTime(new TimeOnly(0))).ToIListAsync();
        }

        public Task<IEnumerable<ApontamentoChannel>> ObterApontamentosPorDataAsync(int idUsuario, DateOnly data)
        {
            return _set.Include(c => c.Atividade)
                        .Include(c => c.Projeto)
                        .Include(c => c.Usuario)
                        .Where(c => c.Usuario.Id == idUsuario && c.Data == data.ToDateTime(new TimeOnly(0))).ToIListAsync();
        }

        public Task<IEnumerable<ApontamentoChannel>> ObterApontamentosPorPeriodoAsync(int idUsuario, DateOnly inicio, DateOnly fim)
        {
            return _set.Include(c => c.Atividade)
                        .Include(c => c.Projeto)
                        .Include(c => c.Usuario)
                        .Where(c => c.Usuario.Id == idUsuario && c.Data >= inicio.ToDateTime(new TimeOnly(0)) && c.Data <= fim.ToDateTime(new TimeOnly(0))).ToIListAsync();
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
