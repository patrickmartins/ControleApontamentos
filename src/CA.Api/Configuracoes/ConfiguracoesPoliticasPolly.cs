using CA.Polly;

namespace CA.Api.Configuracoes
{
    public static class ConfiguracoesPoliticasPolly
    {
        public static void AdicionarPoliticasPolly(this IServiceCollection services)
        {
            ConfiguracaoPolly.RegistrarPoliticasPolly(services);
        }
    }
}
