using CA.Core.Interfaces.Ponto;
using CA.Core.Interfaces.Tfs;
using CA.Servicos.Channel.Interfaces;
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
            var politicasChannel = FabricaPoliticas.CriarPoliticasServicoChannelHttp();

            registry.Add(nameof(IRepositorioTfs), politicasTfs);
            registry.Add(nameof(IRepositorioPonto), politicasPonto);
            registry.Add(nameof(IServicoChannelHttp), politicasChannel);

            services.AddSingleton<IPolicyRegistry<string>>(registry);
        }
    }
}
