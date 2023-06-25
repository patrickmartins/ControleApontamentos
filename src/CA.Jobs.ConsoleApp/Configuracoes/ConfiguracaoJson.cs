using CA.Api.Configuracoes.Json;
using CA.Util.Serializador;
using Flurl.Http;
using System.Text.Json;

namespace CA.Jobs.ConsoleApp.Configuracoes
{
    public static class ConfiguracaoJson
    {
        public static void ConfigurarSerializacaoJson()
        {
            JsonSerializerOptions options = new(JsonSerializerDefaults.Web)
            {
                WriteIndented = true
            };

            options.Converters.Add(new DateOnlyConverter("yyyy-MM-dd"));
            options.Converters.Add(new TimeSpanConverter());

            FlurlHttp.Configure(configFlurl =>
            {
                configFlurl.JsonSerializer = new SerializadorJsonFlurl(options);
            });
        }
    }
}
