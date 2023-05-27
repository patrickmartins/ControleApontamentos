using CA.Core.Entidades.Channel;
using CA.Core.Valores;

namespace CA.Core.Interfaces.Channel
{
    public interface IServicoChannel
    {
        Task<IEnumerable<UsuarioChannel>> ObterTodosUsuariosAsync();
        Resultado<UsuarioChannel?> ObterUsuarioPorEmail(string email);
        Resultado<UsuarioChannel?> ObterUsuarioPorNomeCompleto(string nomeCompleto);
        Task<Resultado<IEnumerable<AtividadeChannel>>> ObterAtividadesApontadasPorUsuarioPorDiaAsync(int idUsuario, DateOnly data);
        Task<Resultado<IEnumerable<AtividadeChannel>>> ObterAtividadesApontadasPorUsuarioPorPeriodoAsync(int idUsuario, DateOnly inicio, DateOnly fim);
        Task<Resultado<IEnumerable<ApontamentoChannel>>> ObterApontamentosPorDataAsync(int idUsuario, DateOnly data);
        Task<Resultado<IEnumerable<ApontamentoChannel>>> ObterApontamentosPorPeriodoAsync(int idUsuario, DateOnly inicio, DateOnly fim);
    }
}
