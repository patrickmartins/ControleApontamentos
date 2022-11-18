using CA.Core.Entidades.Tfs;
using CA.Repositorios.Tfs.ServicosSoap.Models.Requests;
using CA.Repositorios.Tfs.ServicosSoap.Models.Responses;

namespace CA.Repositorios.Tfs.Interfaces
{
    public interface IServicoWorkitemTfsSoap
    {
        Task<QueryWorkitemsResponse> ExecutarQueryAsync(QueryWorkitemsRequest request, string colecao);
        Task<PageWorkitemsByIdsResponse> ObterWorkitemsPorIdsAsync(PageWorkitemsByIdsRequest request, string colecao);
        Task<GetMetadataEx2Response> ObterMetadadosPorColecaoAsync(GetMetadataEx2Request request, string colecao);
        Task AtualizarWorkitemAsync(UpdateRequest request, UsuarioTfs usuario, string colecao);
    }
}
