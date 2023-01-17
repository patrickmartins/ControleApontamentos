using CA.Core.Entidades.Tfs;
using CA.Core.Valores;

namespace CA.Core.Interfaces.Tfs
{
    public interface IServicoTfs
    {
        Task<Resultado<UsuarioTfs>> ObterUsuarioPorNomeAsync(string usuario, string colecao);
        Task<Resultado<IEnumerable<ItemTrabalho>>> ObterItensTrabalhoPorIdAsync(string colecao, params int[] idsItemTrabalho);
        Task<Resultado<IEnumerable<ItemTrabalho>>> ObterItensTrabalhoPorUsuarioAsync(UsuarioTfs usuario, string colecao);
        Task<Resultado<IEnumerable<ItemTrabalho>>> ObterItensTrabalhoApontadosPorDatasAsync(UsuarioTfs usuario, string colecao, params DateOnly[] data);
        Task<Resultado<IEnumerable<ItemTrabalho>>> ObterItensTrabalhoApontadosPorPeriodoAsync(UsuarioTfs usuario, string colecao, DateOnly inicio, DateOnly fim);        
        Task<Resultado<Pagina<ItemTrabalho>>> BuscarItensTrabalhoAsync(string colecao, string palavraChave, StatusItemTrabalho[] status, int pagina = 1, int tamanhoPagina = 10);
        Task<Resultado> AdicionarNovoApontamentoAsync(UsuarioTfs usuario, string colecao, int idItemTrabalho, Apontamento apontamento);
    }
}