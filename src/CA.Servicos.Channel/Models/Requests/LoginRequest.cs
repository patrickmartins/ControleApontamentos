using System.Text.Json.Serialization;

namespace CA.Servicos.Channel.Models.Requests
{
    internal class LoginRequest
    {
        [JsonPropertyName("email")]
        public string Usuario { get; set; }

        [JsonPropertyName("password")]
        public string Senha { get; set; }
    }
}
