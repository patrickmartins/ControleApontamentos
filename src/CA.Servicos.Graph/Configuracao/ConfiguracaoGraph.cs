using Azure.Identity;
using CA.Core.Configuracoes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;

namespace CA.Servicos.Graph.Configuracao
{
    public static class ConfiguracaoGraph
    {
        public static void ConfigurarMicrosoftGraph(IServiceCollection services)
        {            
            services.AddSingleton(provider =>
            {
                var configuracao = provider.GetService<ConfiguracaoMicrosoftGraph>();

                var credenciais = new UsernamePasswordCredential($"{configuracao.Usuario}@{configuracao.Dominio}", configuracao.Senha, configuracao.TenantId, configuracao.ClientId);
                
                return new GraphServiceClient(credenciais, new[] { "User.Read", "User.ReadBasic.All" });
            });
        }
    }
}
