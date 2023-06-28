using CA.Core.Entidades.CA;
using CA.Repositorios.Contexto;
using Microsoft.EntityFrameworkCore;

namespace CA.Repositorios
{
    public abstract class RepositorioBase
    {
        private readonly ContextoDadosCA _contexto;
        private bool _disposed;

        public RepositorioBase(ContextoDadosCA contexto)
        {
            _contexto = contexto;
        }

        protected IQueryable<TEntidade> Set<TEntidade>() where TEntidade : class
        {
            return _contexto.Set<TEntidade>();
        }

        protected async Task InserirAsync<TEntidade>(TEntidade entidade) where TEntidade : class
        {
            _contexto.Entry(entidade).State = EntityState.Added;

            await _contexto.Set<TEntidade>().AddAsync(entidade);
        }

        protected async Task InserirAsync<TEntidade>(IEnumerable<TEntidade> entidades) where TEntidade : class
        {
            foreach (var entidade in entidades)
            {
                _contexto.Entry(entidade).State = EntityState.Added;
            }

            await _contexto.Set<TEntidade>().AddRangeAsync(entidades);
        }

        protected void Atualizar<TEntidade>(TEntidade entidade) where TEntidade : class
        {
            _contexto.Entry(entidade).State = EntityState.Modified;

            _contexto.Set<TEntidade>().Update(entidade);
        }

        protected void Atualizar<TEntidade>(IEnumerable<TEntidade> entidades) where TEntidade : class
        {
            foreach(var entidade in entidades)
            {
                _contexto.Entry(entidade).State = EntityState.Modified;
            }            

            _contexto.Set<TEntidade>().UpdateRange(entidades);
        }

        protected void Excluir<TEntidade>(TEntidade entidade) where TEntidade : class
        {
            _contexto.Entry(entidade).State = EntityState.Deleted;

            _contexto.Set<TEntidade>().Remove(entidade);
        }

        protected void Excluir<TEntidade>(IEnumerable<TEntidade> entidades) where TEntidade : class
        {
            foreach (var entidade in entidades)
            {
                _contexto.Entry(entidade).State = EntityState.Deleted;
            }

            _contexto.Set<TEntidade>().RemoveRange(entidades);
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
