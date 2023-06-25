using CA.Core.Entidades.CA;

namespace CA.Core.Interfaces.Channel
{
    public interface IRepositorioAdministracao
    {
        Task InserirUnidadeAsync(Unidade unidade);
        void AtualizarUnidade(Unidade unidade);
        Task<IEnumerable<Unidade>> ObterTodasUnidadesAsync();
        Unidade? ObterUnidadePorId(string id);
        Task<int> SalvarAlteracoesAsync();
    }
}
