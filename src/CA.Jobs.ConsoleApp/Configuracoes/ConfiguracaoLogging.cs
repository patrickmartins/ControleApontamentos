using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CA.Jobs.ConsoleApp.Configuracoes
{
    public static class ConfiguracaoLogging
    {
        public static void AdicionarConsoleLogger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(config =>
            {
                config.AddSimpleConsole().AddConfiguration(configuration.GetSection("Logging"));
            });
        }
    }
}
