using CA.Core.Entidades.Tfs;
using CA.Core.Valores;

namespace CA.Core.Interfaces.Tfs
{
    public interface IRepositorioItensTrabalho : IRepositorioTfs
    {
        Task<IEnumerable<ItemTrabalhoLink>> ExecutarQueryAsync(string colecao, string query);
        Task<IEnumerable<ItemTrabalho>> ObterItensTrabalhoPorIdAsync(string colecao, params int[] ids);
        Task AtualizarItemTrabalhoAsync(UsuarioTfs usuario, ItemTrabalho itemTrabalho);
    }
}