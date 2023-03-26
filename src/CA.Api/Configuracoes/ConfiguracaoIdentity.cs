namespace CA.Api.Configuracoes
{
    public static class ConfiguracaoIdentity
    {
        public static void AdicionarIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            CA.Identity.Configuracao.ConfiguracaoIdentity.ConfigurarIdentity(services, configuration);
        }
    }
}
