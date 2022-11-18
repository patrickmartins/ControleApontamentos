using CA.Repositorios.Tfs.ServicosSoap.Models.Requests;
using CA.Repositorios.Tfs.ServicosSoap.Models.Responses;

namespace CA.Repositorios.Tfs.Interfaces
{
    public interface IServicoIdentidadeTfsSoap
    {
        Task<ReadIdentitiesResponse> ObterIdentidadeDeUsuarioTfs(ReadIdentitiesRequest request, string colecao);
    }
}
