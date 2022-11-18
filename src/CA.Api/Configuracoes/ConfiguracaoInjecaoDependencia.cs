using CA.Container.IoC;

namespace CA.Api.Configuracoes
{
    public static class ConfiguracaoInjecaoDependencia
    {
        public static void AdicionarServicosCA(this IServiceCollection servicos, IConfiguration configuration)
        {
            CaContainer.RegistrarServicos(servicos, configuration);
        }
    }
}
