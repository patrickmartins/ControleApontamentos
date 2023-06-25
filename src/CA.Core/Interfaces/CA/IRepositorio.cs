namespace CA.Core.Interfaces.CA
{
    public interface IRepositorio
    {
        void Atualizar<TEntidade>(IEnumerable<TEntidade> entidades) where TEntidade : class;
        void Atualizar<TEntidade>(TEntidade entidade) where TEntidade : class;
        void Excluir<TEntidade>(IEnumerable<TEntidade> entidades) where TEntidade : class;
        void Excluir<TEntidade>(TEntidade entidade) where TEntidade : class;
        Task InserirAsync<TEntidade>(IEnumerable<TEntidade> entidades) where TEntidade : class;
        Task InserirAsync<TEntidade>(TEntidade entidade) where TEntidade : class;
        Task<int> SalvarAlteracoesAsync();
        IQueryable<TEntidade> Set<TEntidade>() where TEntidade : class;
    }
}