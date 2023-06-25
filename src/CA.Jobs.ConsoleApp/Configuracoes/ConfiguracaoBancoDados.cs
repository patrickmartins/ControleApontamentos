using CA.Repositorios.Contexto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CA.Jobs.ConsoleApp.Configuracoes
{
    public static class ConfiguracaoBancoDados
    {
        public static void AdicionarContextoDeDados(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ContextoDadosCA>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ConexaoBd"), options => options.MigrationsAssembly("CA.Repositorios"));
            });
        }
    }
}
