using Newtonsoft.Json;

namespace CA.Servicos.Channel.Models.Responses
{
    public class ContaResponse
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("username")]
        public string NomeUsuario { get; set; }
    }
}
