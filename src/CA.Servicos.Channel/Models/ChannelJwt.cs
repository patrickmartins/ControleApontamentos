using System.Text.Json.Serialization;

namespace CA.Servicos.Channel.Models
{
    internal class ChannelJwt
    {
        [JsonPropertyName("jwt")]
        public string TokenAcesso { get; set; }

        [JsonPropertyName("defaultService")]
        public ChannelEndpoint Endpoint { get; set; }

        public string ObterUrlAutenticacao()
        {
            return Endpoint.Url.Replace("{{token}}", TokenAcesso);
        }
    }

    internal class ChannelEndpoint
    {
        [JsonPropertyName("endpoint")]
        public string Url { get; set; }
    }
}
