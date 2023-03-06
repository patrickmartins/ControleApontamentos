using CA.Servicos.Channel.Models.Responses;

namespace CA.Servicos.Channel.Interfaces
{
    public interface IServicoChannelHttp
    {
        Task<ProjetoResponse?> ObterProjetoPorIdAsync(int idProjeto);
        Task<IEnumerable<ProjetoResponse>> ObterProjetosAsync();        
        Task<IEnumerable<AtividadeResponse>> ObterAtividadesPorProjetoAsync(int idProjeto);
        Task<IEnumerable<UsuarioResponse>> ObterUsuariosAtivosAsync();
        Task<IEnumerable<ApontamentoResponse>> ObterApontamentosPorPeriodoAsync(DateOnly inicio, DateOnly fim);
    }
}
