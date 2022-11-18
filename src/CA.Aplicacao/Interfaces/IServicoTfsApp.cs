using CA.Aplicacao.Models;
using CA.Core.Entidades.Tfs;
using CA.Core.Valores;

namespace CA.Aplicacao.Interfaces
{
    public interface IServicoTfsApp
    {
        Task<Resultado<UsuarioTfs>> ObterUsuarioPorNomeAsync(string colecao, string nomeUsuario);
        Task<Resultado<ICollection<TarefaModel>>> ObterTarefasPorIdAsync(UsuarioTfs usuario, string colecao, params int[] ids);
        Task<Resultado<ICollection<GrupoTarefasModel>>> ObterTarefasAtivasPorUsuarioAsync(UsuarioTfs usuario);
        Task<Resultado<ApontamentosDiaModel>> ObterApontamentosPorDiaAsync(UsuarioTfs usuario, DateOnly data);
        Task<Resultado<ApontamentosMesModel>> ObterApontamentosPorMesAsync(UsuarioTfs usuario, int mes, int ano);
        Task<Resultado<Pagina<TarefaModel>>> BuscarTarefasAsync(UsuarioTfs usuario, string colecao, string palavraChave, StatusItemTrabalho[] status, int pagina = 1, int tamanhoPagina = 10);
        Task<Resultado> AdicionarNovoApontamentoAsync(UsuarioTfs usuario, ApontamentoNovoModel apontamento);
    }
}
