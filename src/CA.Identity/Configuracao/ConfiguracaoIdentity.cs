using CA.Identity.Entidades;
using CA.Repositorios.Channel.Contexto;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CA.Identity.Configuracao
{
    public static class ConfiguracaoIdentity
    {
        public static void ConfigurarIdentity(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ConexaoBd"), options => options.MigrationsAssembly("CA.Seguranca"));
            });

            services.AddIdentityCore<Usuario>()
                    .AddRoles<Role>()
                    .AddEntityFrameworkStores<ContextoDadosCA>();
        }
    }
}
