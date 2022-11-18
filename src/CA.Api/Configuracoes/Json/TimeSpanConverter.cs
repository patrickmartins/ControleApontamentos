using System.Text.Json;
using System.Text.Json.Serialization;

namespace CA.Api.Configuracoes.Json
{
    public class TimeSpanConverter : JsonConverter<TimeSpan>
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(TimeSpan) == objectType;
        }

        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.GetString() != null && TimeSpan.TryParse(reader.GetString(), out var valor))
            {
                return valor;
            }

            return TimeSpan.FromSeconds(0);
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.TotalMinutes);
        }
    }
}