using CA.Util.Extensions;
using System.Text.Json.Serialization;

namespace CA.Repositorios.Ponto.Entidades
{
    internal class SecullumJwt
    {
        [JsonPropertyName("access_token")]
        public string TokenAcesso { get; set; }

        [JsonPropertyName("refresh_token")]
        public string TokenAtualizacao { get; set; }

        [JsonPropertyName("expires_in")]
        public int Validade { get; set; }

        [JsonIgnore]
        public DateTime DataExpiracao { get; set; }

        public bool Expirado()
        {
            return DateTime.Now.ConverterParaFusoBrasil() > DataExpiracao;
        }
    }
}
