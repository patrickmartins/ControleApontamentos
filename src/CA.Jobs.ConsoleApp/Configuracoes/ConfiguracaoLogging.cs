using CA.Repositorios.Channel.Contexto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
