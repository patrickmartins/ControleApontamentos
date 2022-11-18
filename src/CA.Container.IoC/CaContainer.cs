using CA.Aplicacao.Interfaces;
using CA.Aplicacao.Servicos;
using CA.Core.Configuracoes;
using CA.Core.Interfaces.Ponto;
using CA.Core.Interfaces.Tfs;
using CA.Core.Servicos.Ponto;
using CA.Core.Servicos.Tfs;
using CA.Repositorios.Ponto;
using CA.Repositorios.Ponto.Cache;
using CA.Repositorios.Ponto.Interfaces;
using CA.Repositorios.Ponto.ServicosHttp;
using CA.Repositorios.Tfs;
using CA.Repositorios.Tfs.Interfaces;
using CA.Repositorios.Tfs.ServicosHttp;
using CA.Repositorios.Tfs.ServicosSoap;
using CA.Seguranca.Interfaces;
using CA.Seguranca.Servicos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CA.Container.IoC
{
    public static class CaContainer
    {
        public static void RegistrarServicos(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRepositorioPonto, RepositorioSecullum>();
            services.AddScoped<IRepositorioItensTrabalho, RepositorioItensTrabalhoTfs>();
            services.AddScoped<IRepositorioUsuariosTfs, RepositorioUsuariosTfs>();
            services.AddScoped<IRepositorioColecoes, RepositorioColecoesCache>();
            
            services.AddScoped<IServicoPonto, ServicoSecullum>();
            services.AddScoped<IServicoTfs, ServicoTfs>();

            services.AddScoped<IServicoPontoApp, ServicoSecullumApp>();
            services.AddScoped<IServicoTfsApp, ServicoTfsApp>();
            services.AddScoped<IServicoIdentidade, ServicoIdentidade>();

            services.AddScoped<IServicoIdentidadeTfsSoap, ServicoIdentidadeTfsSoap>();
            services.AddScoped<IServicoWorkitemTfsSoap, ServicoWorkitemTfsSoap>();
            
            services.AddScoped<IServicoColecaoTfsHttp, ServicoColecaoTfsHttp>();
            services.AddScoped<IServicoSecullumHttp, ServicoSecullumHttp>();

            services.Decorate<IServicoSecullumHttp, ServicoSecullumHttpCache>();
            services.Decorate<IServicoColecaoTfsHttp, ServicoColecaoTfsHttpCache>();
            services.Decorate<IServicoWorkitemTfsSoap, ServicoWorkitemTfsSoapCache>();

            services.Configure<ConfiguracaoCA>(configuration.GetSection(ConfiguracaoCA.SecaoConfig));

            services.AddSingleton((service) =>
            {
                var config = service.GetService<IOptions<ConfiguracaoCA>>();

                if (config == null)
                    throw new ArgumentNullException("A configuração da aplicação não foram encontradas.");

                config.Value.Secullum.Validar();

                return config.Value.Secullum;
            });

            services.AddSingleton((service) =>
            {
                var config = service.GetService<IOptions<ConfiguracaoCA>>();

                if (config == null)
                    throw new ArgumentNullException("A configuração da aplicação não foram encontradas.");

                config.Value.Tfs.Validar();

                return config.Value.Tfs;
            });

            services.AddSingleton((service) =>
            {
                var config = service.GetService<IOptions<ConfiguracaoCA>>();

                if (config == null)
                    throw new ArgumentNullException("A configuração da aplicação não foram encontradas.");

                config.Value.Jwt.Validar();

                return config.Value.Jwt;
            });

            services.AddMemoryCache();
        }
    }
}
