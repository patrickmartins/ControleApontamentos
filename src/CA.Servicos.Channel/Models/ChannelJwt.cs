using System.Text.Json.Serialization;
using CA.Util.Extensions;

namespace CA.Servicos.Channel.Models
{
    internal class ChannelJwt
    {
        [JsonPropertyName("jwt")]
        public string TokenAcesso { get; set; }

        [JsonPropertyName("defaultService")]
        public ChannelEndpoint Endpoint { get; set; }

        public DateTime DataExpiracao { get; set; }

        public string ObterUrlAutenticacao()
        {
            return Endpoint.Url.Replace("{{token}}", TokenAcesso);
        }

        public bool Expirado()
        {
            return DateTime.Now.ConverterParaFusoBrasil() > DataExpiracao;
        }
    }

    internal class ChannelEndpoint
    {
        [JsonPropertyName("endpoint")]
        public string Url { get; set; }
    }
}
