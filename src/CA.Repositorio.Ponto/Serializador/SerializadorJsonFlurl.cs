using Flurl.Http.Configuration;
using System.Text.Json;

namespace CA.Repositorios.Ponto.Serializador
{
    public class SerializadorJsonFlurl : ISerializer
    {
        private readonly JsonSerializerOptions _options;

        public SerializadorJsonFlurl(JsonSerializerOptions options)
        {
            _options = options;
        }

        public T? Deserialize<T>(string s)
        {
            return JsonSerializer.Deserialize<T>(s, _options);
        }

        public T? Deserialize<T>(Stream stream)
        {
            return JsonSerializer.Deserialize<T>(stream, _options);
        }

        public string Serialize(object obj)
        {
            return JsonSerializer.Serialize(obj, _options);
        }
    }
}
