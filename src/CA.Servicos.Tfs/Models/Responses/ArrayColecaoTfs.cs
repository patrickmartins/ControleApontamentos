using System.Text.Json.Serialization;

namespace CA.Servicos.Tfs.Models.Responses
{
    public class ArrayColecaoTfs
    {
        [JsonPropertyName("__wrappedArray")]
        public List<ColecaoTfs> Colecoes { get; set; }

        public ArrayColecaoTfs()
        {
            Colecoes = new List<ColecaoTfs>();
        }
    }
}
