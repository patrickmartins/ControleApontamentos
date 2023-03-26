using System.Text.Json.Serialization;

namespace CA.Servicos.Channel.Models.Responses
{
    public class ConsultaUsuariosResponse
    {
        [JsonPropertyName("count")]
        public int Quantidade { get; set; }

        [JsonPropertyName("rows")]
        public IEnumerable<UsuarioResponse> Resultados { get; set; }
    }

    public class UsuarioResponse
    {
        [JsonPropertyName("legacyRef")]
        public int Id { get; set; }

        [JsonPropertyName("username")]
        public string NomeUsuario { get; set; }

        [JsonPropertyName("name")]
        public string NomeCompleto { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
