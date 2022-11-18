using CA.Core.Entidades.Tfs;

namespace CA.Core.Interfaces.Tfs
{
    public interface IRepositorioColecoes : IRepositorioTfs
    {
        Task<string[]> ObterColecoesPorUsuarioAsync(UsuarioTfs usuario);
        Task<string[]> ObterTodasColecoesAsync();
    }
}
