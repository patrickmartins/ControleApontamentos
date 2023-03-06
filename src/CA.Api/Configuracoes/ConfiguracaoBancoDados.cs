using CA.Repositorios.Channel.Contexto;
using Microsoft.EntityFrameworkCore;

namespace CA.Api.Configuracoes
{
    public static class ConfiguracaoBancoDados
    {
        public static void AdicionarContextoDeDados(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ContextoDadosChannel>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ConexaoBd"), options => options.MigrationsAssembly("CA.Repositorios.Channel"));
            });
        }
    }
}
