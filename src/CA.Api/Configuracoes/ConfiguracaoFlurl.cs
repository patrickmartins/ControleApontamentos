using Flurl.Http;

namespace CA.Api.Configuracoes
{
    public static class ConfiguracaoFlurl
    {
        public static void ConfigurarFlurl(this IApplicationBuilder app)
        {
            FlurlHttp.Configure(c =>
            {
                c.Timeout = TimeSpan.FromMinutes(1);
            });
        }
    }
}
