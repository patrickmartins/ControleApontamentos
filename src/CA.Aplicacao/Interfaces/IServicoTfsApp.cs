using CA.Aplicacao.Models;
using CA.Core.Entidades.Tfs;
using CA.Core.Valores;

namespace CA.Aplicacao.Interfaces
{
    public interface IServicoTfsApp
    {
        Task<IEnumerable<UsuarioTfs>> ObterTodosUsuariosAsync();
        Task<IEnumerable<UsuarioTfs>> ObterTodosUsuariosPorColecaoAsync(string colecao);
        Task<Resultado<UsuarioTfs>> ObterUsuarioPorNomeAsync(string colecao, string nomeUsuario);
        Task<Resultado<IEnumerable<TarefaModel>>> ObterTarefasPorIdAsync(UsuarioTfs usuario, string colecao, params int[] ids);
        Task<Resultado<IEnumerable<GrupoTarefasModel>>> ObterTarefasAtivasPorUsuarioAsync(UsuarioTfs usuario);
        Task<Resultado<ApontamentosTfsDiaModel>> ObterApontamentosPorDiaAsync(UsuarioTfs usuario, DateOnly data);
        Task<Resultado<ApontamentosTfsMesModel>> ObterApontamentosPorMesAsync(UsuarioTfs usuario, int mes, int ano);
        Task<Resultado<Pagina<TarefaModel>>> BuscarTarefasAsync(UsuarioTfs usuario, string colecao, string palavraChave, StatusItemTrabalho[] status, int pagina = 1, int tamanhoPagina = 10);
        Task<Resultado> AdicionarNovoApontamentoAsync(UsuarioTfs usuario, ApontamentoTfsNovoModel apontamento);
    }
}
