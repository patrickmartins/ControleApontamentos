using CA.Core.Entidades.Channel;
using CA.Jobs.Channel;
using CA.Jobs.Channel.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CA.Jobs.ConsoleApp.Configuracoes
{
    public static class ConfiguracaoJobs
    {
        public static void AdicionarJobsChannel(this IServiceCollection services)
        {
            services.AddScoped<IJobChannel<UsuarioChannel>, JobCargaUsuarios>();
            services.AddScoped<IJobChannel<ApontamentoChannel>, JobCargaApontamentos>();
            services.AddScoped<IJobChannel<ProjetoChannel>, JobCargaProjetos>();
        }
    }
}
