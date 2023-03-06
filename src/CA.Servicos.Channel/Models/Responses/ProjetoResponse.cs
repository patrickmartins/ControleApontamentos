using CA.Core.Entidades.Channel;
using Newtonsoft.Json;

namespace CA.Servicos.Channel.Models.Responses
{
    public class ProjetoResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("status")]
        public StatusProjeto Status { get; set; }

        [JsonProperty("atividades")]
        public IEnumerable<AtividadeResponse> Atividades { get; set; }
    }
}
