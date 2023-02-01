using CA.Core.Entidades.Channel;

namespace CA.Facade.Channel.Interfaces
{
    public interface IServicoChannelFacade
    {
        Task<IEnumerable<ProjetoChannel>> ObterProjetosAtivosAsync();
        Task<IEnumerable<AtividadeProjetoChannel>> ObterAtividadesPorProjetoAsync(int idProjeto);
        Task<IEnumerable<UsuarioChannel>> ObterUsuariosAtivosAsync();
        Task<IEnumerable<ApontamentoChannel>> ObterApontamentosPorPeriodoAsync(DateOnly inicio, DateOnly fim);
    }
}
