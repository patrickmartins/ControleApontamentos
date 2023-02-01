using CA.Core.Entidades.Channel;

namespace CA.Core.Interfaces.Channel
{
    public interface IRepositorioProjetos : IRepositorioChannel
    {
        Task InserirProjetoAsync(ProjetoChannel projeto);
        Task InserirProjetosAsync(IEnumerable<ProjetoChannel> projetos);
        void AtualizarProjeto(ProjetoChannel projeto);
        void AtualizarProjetos(IEnumerable<ProjetoChannel> projetos);

        Task InserirAtividadeAsync(AtividadeChannel atividade);
        Task InserirAtividadesAsync(IEnumerable<AtividadeChannel> atividades);
        void AtualizarAtividade(AtividadeChannel atividade);
        void AtualizarAtividades(IEnumerable<AtividadeChannel> atividades);

        Task<IEnumerable<ProjetoChannel>> ObterTodosProjetosAsync();
        Task<IEnumerable<ProjetoChannel>> ObterTodosProjetosAtivosAsync();

        Task<IEnumerable<AtividadeChannel>> ObterAtividadesApontadasPorPorUsuarioPorDiaAsync(int idUsuario, DateOnly data);
        Task<IEnumerable<AtividadeChannel>> ObterAtividadesApontadasPorPorUsuarioPorPeriodoAsync(int idUsuario, DateOnly inicio, DateOnly fim);

        Task<IEnumerable<AtividadeChannel>> ObterTodasAtividadesAsync();
        Task<IEnumerable<AtividadeChannel>> ObterAtividadesPorCodigoAsync(string codigo);
        Task<IEnumerable<AtividadeChannel>> ObterAtividadesPorProjetoAsync(params int[] idProjeto);

    }
}
