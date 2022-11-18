using CA.Core.Interfaces.Ponto;
using CA.Core.Interfaces.Tfs;
using Microsoft.Extensions.DependencyInjection;
using Polly.Registry;

namespace CA.Polly
{
    public static class ConfiguracaoPolly
    {
        public static void RegistrarPoliticasPolly(IServiceCollection services)
        {
            var registry = new PolicyRegistry();

            var politicasTfs = FabricaPoliticas.CriarPoliticasTfs();
            var politicasPonto = FabricaPoliticas.CriarPoliticasPonto();

            registry.Add(nameof(IRepositorioTfs), politicasTfs);
            registry.Add(nameof(IRepositorioPonto), politicasPonto);

            services.AddSingleton<IPolicyRegistry<string>>(registry);
        }
    }
}
