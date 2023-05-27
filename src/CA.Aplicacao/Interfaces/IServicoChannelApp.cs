using CA.Aplicacao.Models;
using CA.Core.Entidades.Channel;
using CA.Core.Valores;

namespace CA.Aplicacao.Interfaces
{
    public interface IServicoChannelApp
    {
        Task<IEnumerable<UsuarioChannel>> ObterTodosUsuariosAsync();
        Resultado<UsuarioChannel?> ObterUsuarioPorEmail(string emailUsuario);
        Resultado<UsuarioChannel?> ObterUsuarioPorNomeCompleto(string nomeCompleto);
        Task<Resultado<ApontamentosChannelDiaModel>> ObterApontamentosPorDiaAsync(int idUsuario, DateOnly data);
        Task<Resultado<ApontamentosChannelMesModel>> ObterApontamentosPorMesAsync(int idUsuario, int mes, int ano);
    }
}
