using System.Text.Json.Serialization;

namespace CA.Servicos.Tfs.Models.Responses
{
    public class ColecaoTfs
    {
        [JsonPropertyName("name")]
        public string Nome { get; set; }

        [JsonPropertyName("uri")]
        public string Uri { get; set; }
    }
}
