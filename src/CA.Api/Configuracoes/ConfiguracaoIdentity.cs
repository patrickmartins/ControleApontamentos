using CA.Identity.Entidades;
using CA.Repositorios.Contexto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CA.Api.Configuracoes
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
