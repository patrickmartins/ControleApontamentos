using System.Text.Json;
using System.Text.Json.Serialization;

namespace CA.Api.Configuracoes.Json
{
    public class DateOnlyConverter : JsonConverter<DateOnly>
    {
        private readonly string _formato;

        public DateOnlyConverter(string formato)
        {
            _formato = formato;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(DateOnly) == objectType;
        }

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var valorString = reader.GetString();

            if (valorString != null && DateOnly.TryParse(valorString, out var valor))
            {
                return valor;
            }

            return new DateOnly();
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(_formato));
        }
    }
}
