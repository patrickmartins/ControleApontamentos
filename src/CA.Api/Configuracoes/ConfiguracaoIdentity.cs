using CA.Seguranca.Identity;

namespace CA.Api.Configuracoes
{
    public static class ConfiguracaoIdentity
    {
        public static void AdicionarIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            Seguranca.Identity.ConfiguracaoIdentity.ConfigurarIdentity(services, configuration);
        }
    }
}
