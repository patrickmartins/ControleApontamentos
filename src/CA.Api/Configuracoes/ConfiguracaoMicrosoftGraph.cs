using CA.Servicos.Graph.Configuracao;

namespace CA.Api.Configuracoes
{
    public static class ConfiguracaoMicrosoftGraph
    {
        public static void AdicionarMicrosoftGraph(this IServiceCollection services)
        {
            ConfiguracaoGraph.ConfigurarMicrosoftGraph(services);
        }
    }
}
