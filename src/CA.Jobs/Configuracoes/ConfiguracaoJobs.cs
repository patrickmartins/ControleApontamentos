using CA.Core.Entidades.CA;
using CA.Core.Entidades.Channel;
using CA.Jobs.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CA.Jobs.Configuracoes
{
    public static class ConfiguracaoJobs
    {
        public static void AdicionarJobs(this IServiceCollection services)
        {
            services.AddScoped<IJob<UsuarioChannel>, JobCargaUsuarios>();
            services.AddScoped<IJob<ApontamentoChannel>, JobCargaApontamentos>();
            services.AddScoped<IJob<ProjetoChannel>, JobCargaProjetos>();
            services.AddScoped<IJob<UsuarioCA>, JobImportacaoUsuariosMicrosoft>();
        }
    }
}
