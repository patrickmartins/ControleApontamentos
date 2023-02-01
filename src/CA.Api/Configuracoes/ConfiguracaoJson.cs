using CA.Api.Configuracoes.Json;
using CA.Util.Serializador;
using Flurl.Http;

namespace CA.Api.Configuracoes
{
    public static class ConfiguracaoJson
    {
        public static void ConfigurarSerializacaoJson(this IMvcBuilder builder)
        {
            builder.AddJsonOptions(config =>
            {
                config.JsonSerializerOptions.Converters.Add(new DateOnlyConverter("yyyy-MM-dd"));
                config.JsonSerializerOptions.Converters.Add(new TimeSpanConverter());
                
                FlurlHttp.Configure(configFlurl =>
                {
                    configFlurl.JsonSerializer = new SerializadorJsonFlurl(config.JsonSerializerOptions);
                });
            });
        }
    }
}
