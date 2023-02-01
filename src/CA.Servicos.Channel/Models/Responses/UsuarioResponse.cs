using Newtonsoft.Json;

namespace CA.Servicos.Channel.Models.Responses
{
    public class UsuarioResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nomeUsuario")]
        public string Nome { get; set; }

        [JsonProperty("contaLoginEmail")]
        public string Email { get; set; }
    }
}
