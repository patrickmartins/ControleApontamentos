using CA.Api.Hangfire.Logging;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Configuration;

namespace CA.Api.Configuracoes
{
    public static class ConfiguracaoLogging
    {
        public static ILoggingBuilder AdicionarHangfireConsoleLogger(this ILoggingBuilder builder)
        {
            builder.AddConfiguration();

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, HangfireConsoleLogProvider>());

            return builder;
        }
    }
}
