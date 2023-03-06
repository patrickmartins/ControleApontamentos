using CA.Core.Entidades.Channel;

namespace CA.Core.Interfaces.Channel
{
    public interface IRepositorioChannel : IDisposable
    {
        Task<int> SalvarAlteracoesAsync();
    }
}
