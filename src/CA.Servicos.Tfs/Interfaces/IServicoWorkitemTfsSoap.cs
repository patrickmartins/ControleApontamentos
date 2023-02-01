using CA.Core.Entidades.Tfs;
using CA.Servicos.Tfs.Models.Requests;
using CA.Servicos.Tfs.Models.Responses;

namespace CA.Servicos.Tfs.Interfaces
{
    public interface IServicoWorkitemTfsSoap
    {
        Task<QueryWorkitemsResponse> ExecutarQueryAsync(QueryWorkitemsRequest request, string colecao);
        Task<PageWorkitemsByIdsResponse> ObterWorkitemsPorIdsAsync(PageWorkitemsByIdsRequest request, string colecao);
        Task<GetMetadataEx2Response> ObterMetadadosPorColecaoAsync(GetMetadataEx2Request request, string colecao);
        Task AtualizarWorkitemAsync(UpdateRequest request, UsuarioTfs usuario, string colecao);
    }
}
