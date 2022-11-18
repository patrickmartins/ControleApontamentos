
using System.Text.Json.Serialization;

namespace CA.Repositorios.Tfs.Entidades
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
