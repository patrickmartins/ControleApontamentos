using Newtonsoft.Json;

namespace CA.Servicos.Channel.Models.Responses
{
    internal class EscopoResponse
    {
        [JsonProperty("projeto")]
        public ProjetoResponse Projeto { get; set; }
    }

    public class AtividadeResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        public int IdProjeto { get; set; }
    }
}
