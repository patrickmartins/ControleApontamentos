using CA.Core.Entidades.Channel;
using CA.Core.Valores;

namespace CA.Core.Interfaces.Channel
{
    public interface IRepositorioApontamentos : IRepositorioChannel
    {
        Task InserirApontamentoAsync(ApontamentoChannel apontamento);
        Task InserirApontamentosAsync(IEnumerable<ApontamentoChannel> apontamentos);

        void AtualizarApontamento(ApontamentoChannel apontamento);
        void AtualizarApontamentos(IEnumerable<ApontamentoChannel> apontamentos);

        void RemoverApontamento(ApontamentoChannel apontamento);
        void RemoverApontamentos(IEnumerable<ApontamentoChannel> apontamentos);

        Task<IEnumerable<ApontamentoChannel>> ObterTodosApontamentosPorPeriodoAsync(DateOnly inicio, DateOnly fim);
        Task<IEnumerable<ApontamentoChannel>> ObterApontamentosPorDataAsync(int idUsuario, DateOnly data);
        Task<IEnumerable<ApontamentoChannel>> ObterApontamentosPorPeriodoAsync(int idUsuario, DateOnly inicio, DateOnly fim);
    }
}
