using CA.Core.Entidades.Tfs;
using CA.Core.Valores;

namespace CA.Core.Interfaces.Tfs
{
    public interface IRepositorioItensTrabalho : IRepositorioTfs
    {
        Task<IEnumerable<CampoTfs>> ObterCamposSuportadosPorColecaoAsync(string colecao);
        Task<IEnumerable<ItemTrabalhoLink>> ExecutarQueryDeLinksAsync(string colecao, string query);
        Task<int[]> ExecutarQueryDeIdsAsync(string colecao, string query);
        Task<IEnumerable<ItemTrabalho>> ObterItensTrabalhoPorIdAsync(string colecao, params int[] ids);
        Task AtualizarItemTrabalhoAsync(UsuarioTfs usuario, ItemTrabalho itemTrabalho);
    }
}