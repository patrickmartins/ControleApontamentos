using System.Text.Json.Serialization;

namespace CA.Servicos.Secullum.Entidades
{
    internal class ErroRequisicao
    {
        [JsonPropertyName("Property")]
        public string Propriedade { get; set; }

        [JsonPropertyName("Message")]
        public string Mensagem { get; set; }
    }
}
