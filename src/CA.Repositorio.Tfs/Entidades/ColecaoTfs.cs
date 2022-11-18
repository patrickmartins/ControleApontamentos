
using System.Text.Json.Serialization;

namespace CA.Repositorios.Tfs.Entidades
{
    public class ColecaoTfs
    {
        [JsonPropertyName("name")]
        public string Nome { get; set; }

        [JsonPropertyName("uri")]
        public string Uri { get; set; }
    }
}
