using CA.Servicos.Tfs.Models.Requests;
using CA.Servicos.Tfs.Models.Responses;

namespace CA.Servicos.Tfs.Interfaces
{
    public interface IServicoIdentidadeTfsSoap
    {
        Task<ReadIdentitiesResponse> ObterIdentidadeDeUsuarioTfs(ReadIdentitiesRequest request, string colecao);
    }
}
