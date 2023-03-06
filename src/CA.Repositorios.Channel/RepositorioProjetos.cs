using CA.Core.Entidades.Channel;
using CA.Core.Interfaces.Channel;
using CA.Repositorios.Channel.Contexto;
using CA.Util.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CA.Repositorios.Channel
{
    public class RepositorioProjetos : IRepositorioProjetos
    {
        private readonly ContextoDadosChannel _contexto;

        private readonly DbSet<ProjetoChannel> _setProjetos;
        private readonly DbSet<AtividadeChannel> _setAtividades;

        private bool _disposed;

        public RepositorioProjetos(ContextoDadosChannel contexto)
        {
            _contexto = contexto;

            _setProjetos = contexto.Set<ProjetoChannel>();
            _setAtividades = contexto.Set<AtividadeChannel>();
        }

        public async Task InserirProjetoAsync(ProjetoChannel projeto)
        {
            await _setProjetos.AddAsync(projeto);
        }

        public async Task InserirProjetosAsync(IEnumerable<ProjetoChannel> projetos)
        {
            await _setProjetos.AddRangeAsync(projetos);
        }

        public void AtualizarProjeto(ProjetoChannel projeto)
        {
            _setProjetos.Update(projeto);
        }

        public void AtualizarProjetos(IEnumerable<ProjetoChannel> projetos)
        {
            _setProjetos.UpdateRange(projetos);
        }

        public async Task InserirAtividadeAsync(AtividadeChannel atividade)
        {
            await _setAtividades.AddAsync(atividade);
        }

        public async Task InserirAtividadesAsync(IEnumerable<AtividadeChannel> atividades)
        {
            await _setAtividades.AddRangeAsync(atividades);
        }

        public void AtualizarAtividade(AtividadeChannel atividade)
        {
            _setAtividades.Update(atividade);
        }

        public void AtualizarAtividades(IEnumerable<AtividadeChannel> atividades)
        {
            _setAtividades.UpdateRange(atividades);
        }

        public Task<IEnumerable<ProjetoChannel>> ObterTodosProjetosAsync()
        {
            return _setProjetos.Include(c => c.Atividades).ToIListAsync();
        }

        public Task<IEnumerable<ProjetoChannel>> ObterProjetosPorIdsAsync(params int[] idsProjetos)
        {
            return _setProjetos.Include(c => c.Atividades).Where(c => idsProjetos.Contains(c.Id)).ToIListAsync();
        }

        public Task<IEnumerable<ProjetoChannel>> ObterTodosProjetosAtivosAsync()
        {
            return _setProjetos.Where(c => c.Status != StatusProjeto.Finalizado).ToIListAsync();
        }

        public Task<IEnumerable<AtividadeChannel>> ObterTodasAtividadesAsync()
        {
            return _setAtividades.ToIListAsync();
        }

        public Task<IEnumerable<AtividadeChannel>> ObterAtividadesPorIdsAsync(params int[] ids)
        {
            return _setAtividades.Where(c => ids.Contains(c.Id)).Include(c => c.Projeto).ToIListAsync();
        }

        public Task<IEnumerable<AtividadeChannel>> ObterAtividadesPorCodigoAsync(string codigo)
        {
            return _setAtividades.Where(c => c.Codigo == codigo).ToIListAsync();
        }

        public Task<IEnumerable<AtividadeChannel>> ObterAtividadesPorProjetoAsync(params int[] idsProjetos)
        {
            return _setAtividades.Where(c => idsProjetos.Contains(c.Projeto.Id)).Include(c => c.Projeto).ToIListAsync();
        }

        public Task<IEnumerable<AtividadeChannel>> ObterAtividadesApontadasPorPorUsuarioPorDiaAsync(int idUsuario, DateOnly data)
        {
            return _setAtividades
                        .Include(c => c.Projeto)
                        .Include(c => c.Apontamentos.Where(a => a.Usuario.Id == idUsuario
                                        && a.Data == data.ToDateTime(new TimeOnly(0))))
                        .ThenInclude(c => c.Usuario)
                        .ToIListAsync();
        }

        public Task<IEnumerable<AtividadeChannel>> ObterAtividadesApontadasPorPorUsuarioPorPeriodoAsync(int idUsuario, DateOnly inicio, DateOnly fim)
        {
            return _setAtividades
                        .Include(c => c.Projeto)
                        .Include(c => c.Apontamentos.Where(a => a.Usuario.Id == idUsuario
                                        && a.Data >= inicio.ToDateTime(new TimeOnly(0))
                                        && a.Data <= fim.ToDateTime(new TimeOnly(0))))
                        .ThenInclude(c => c.Usuario)
                        .ToIListAsync();
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
