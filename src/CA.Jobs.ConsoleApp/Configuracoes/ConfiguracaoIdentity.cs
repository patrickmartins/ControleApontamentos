using CA.Identity.Entidades;
using CA.Repositorios.Contexto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CA.Jobs.ConsoleApp.Configuracoes
{
    public static class ConfiguracaoIdentity
    {
        public static void AdicionarIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ConexaoBd"), options => options.MigrationsAssembly("CA.Seguranca"));
            });

            services.AddIdentityCore<UsuarioIdentity>()
                    .AddRoles<IdentityRole<string>>()
                    .AddEntityFrameworkStores<ContextoDadosCA>();
        }
    }
}
