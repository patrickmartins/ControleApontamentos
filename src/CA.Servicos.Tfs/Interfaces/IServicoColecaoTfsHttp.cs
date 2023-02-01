using CA.Core.Entidades.Tfs;

namespace CA.Servicos.Tfs.Interfaces
{
    public interface IServicoColecaoTfsHttp
    {
        Task<string[]> ObterColecoesPorUsuarioAsync(UsuarioTfs usuario);
        Task<string[]> ObterTodasColecoesAsync();
    }
}