using CA.Util.Serializador;
using Flurl.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CA.Jobs.ConsoleApp.Configuracoes
{
    public static class ConfiguracaoFlurl
    {
        public static void ConfigurarFlurl()
        {
            FlurlHttp.Configure(configFlurl =>
            {
                var serializerOptions = new JsonSerializerOptions
                {
                    MaxDepth = 32,
                    NumberHandling = JsonNumberHandling.AllowReadingFromString,
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                configFlurl.JsonSerializer = new SerializadorJsonFlurl(serializerOptions);
            });
        }
    }
}
