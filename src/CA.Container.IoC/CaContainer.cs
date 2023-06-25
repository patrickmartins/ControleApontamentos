using CA.Aplicacao.Interfaces;
using CA.Aplicacao.Servicos;
using CA.Core.Configuracoes;
using CA.Core.Interfaces.CA;
using CA.Core.Interfaces.Channel;
using CA.Core.Interfaces.Microsoft;
using CA.Core.Interfaces.Ponto;
using CA.Core.Interfaces.Tfs;
using CA.Core.Servicos.CA;
using CA.Core.Servicos.Channel;
using CA.Core.Servicos.Ponto;
using CA.Core.Servicos.Tfs;
using CA.Identity.Interfaces;
using CA.Identity.Servicos;
using CA.Repositorios.CA;
using CA.Repositorios.Channel;
using CA.Repositorios.Ponto;
using CA.Repositorios.Tfs;
using CA.Servicos.Channel;
using CA.Servicos.Channel.Cache;
using CA.Servicos.Channel.Interfaces;
using CA.Servicos.Graph;
using CA.Servicos.Secullum;
using CA.Servicos.Secullum.Cache;
using CA.Servicos.Secullum.Interfaces;
using CA.Servicos.Tfs;
using CA.Servicos.Tfs.Cache;
using CA.Servicos.Tfs.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CA.Container.IoC
{
    public static class CaContainer
    {
        public static void RegistrarServicos(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRepositorioPonto, RepositorioPonto>();
            services.AddScoped<IRepositorioItensTrabalho, RepositorioItensTrabalhoTfs>();
            services.AddScoped<IRepositorioUsuariosTfs, RepositorioUsuariosTfs>();
            services.AddScoped<IRepositorioColecoes, RepositorioColecoesCache>();
            services.AddScoped<IRepositorioApontamentos, RepositorioApontamentos>();            
            services.AddScoped<IRepositorioUsuariosChannel, RepositorioUsuariosChannel>();
            services.AddScoped<IRepositorioProjetos, RepositorioProjetos>();
            services.AddScoped<IRepositorioUsuariosCa, RepositorioUsuariosCa>();
            services.AddScoped<IRepositorioAdministracao, RepositorioAdministracao>();

            services.AddScoped<IServicoPonto, ServicoSecullum>();
            services.AddScoped<IServicoTfs, ServicoTfs>();
            services.AddScoped<IServicoIdentidade, ServicoIdentidade>();
            services.AddScoped<IServicoChannel, ServicoChannel>();
            services.AddScoped<IServicoUsuariosCa, ServicoUsuariosCa>();
            services.AddScoped<IServicoAdministracao, ServicoAdministracao>();

            services.AddScoped<IServicoPontoApp, ServicoSecullumApp>();
            services.AddScoped<IServicoTfsApp, ServicoTfsApp>();
            services.AddScoped<IServicoChannelApp, ServicoChannelApp>();
            services.AddScoped<IServicoRelatorioApp, ServicoRelatorioApp>();
            services.AddScoped<IServicoUsuariosCaApp, ServicoUsuariosCaApp>();
            services.AddScoped<IServicoAdministracaoApp, ServicoAdministracaoApp>();

            services.AddScoped<IServicoIdentidadeTfsSoap, ServicoIdentidadeTfsSoap>();
            services.AddScoped<IServicoWorkitemTfsSoap, ServicoWorkitemTfsSoap>();
            
            services.AddScoped<IServicoColecaoTfsHttp, ServicoColecaoTfsHttp>();
            services.AddScoped<IServicoSecullumHttp, ServicoSecullumHttp>();
            services.AddScoped<IServicoSecullumHttp, ServicoSecullumHttp>();
            services.AddScoped<IServicoChannelHttp, ServicoChannelHttp>();
            services.AddScoped<IServicoMicrosoftGraph, ServicoMicrosoftGraph>();

            services.Decorate<IServicoSecullumHttp, ServicoSecullumHttpCache>();
            services.Decorate<IServicoColecaoTfsHttp, ServicoColecaoTfsHttpCache>();
            services.Decorate<IServicoChannelHttp, ServicoChannelHttpCache>();

            services.Configure<ConfiguracaoCA>(configuration.GetSection(ConfiguracaoCA.SecaoConfig));

            services.AddSingleton((service) =>
            {
                var config = service.GetService<IOptions<ConfiguracaoCA>>();

                if (config == null)
                    throw new ArgumentNullException("As configurações da aplicação não foram encontradas.");

                if (config.Value.Secullum == null)
                    throw new ArgumentNullException("As configurações da integração com o Secullum não foram encontradas.");

                config.Value.Secullum.Validar();

                return config.Value.Secullum;
            });

            services.AddSingleton((service) =>
            {
                var config = service.GetService<IOptions<ConfiguracaoCA>>();

                if (config == null)
                    throw new ArgumentNullException("As configurações da aplicação não foram encontradas.");

                if (config.Value.Channel == null)
                    throw new ArgumentNullException("As configurações da integração com o Channel não foram encontradas.");

                config.Value.Channel.Validar();

                return config.Value.Channel;
            });

            services.AddSingleton((service) =>
            {
                var config = service.GetService<IOptions<ConfiguracaoCA>>();

                if (config == null)
                    throw new ArgumentNullException("As configurações da aplicação não foram encontradas.");

                if (config.Value.Tfs == null)
                    throw new ArgumentNullException("As configurações da integração com o TFS não foram encontradas.");

                config.Value.Tfs.Validar();

                return config.Value.Tfs;
            });

            services.AddSingleton((service) =>
            {
                var config = service.GetService<IOptions<ConfiguracaoCA>>();

                if (config == null)
                    throw new ArgumentNullException("As configurações da aplicação não foram encontradas.");

                if (config.Value.Jwt == null)
                    throw new ArgumentNullException("As configurações de segurança da aplicação não foram encontradas.");

                config.Value.Jwt.Validar();

                return config.Value.Jwt;
            });

            services.AddSingleton((service) =>
            {
                var config = service.GetService<IOptions<ConfiguracaoCA>>();

                if (config == null)
                    throw new ArgumentNullException("As configurações da aplicação não foram encontradas.");

                if (config.Value.Geral == null)
                    throw new ArgumentNullException("As configurações gerais da aplicação não foram encontradas.");

                config.Value.Geral.Validar();

                return config.Value.Geral;
            });

            services.AddSingleton((service) =>
            {
                var config = service.GetService<IOptions<ConfiguracaoCA>>();

                if (config == null)
                    throw new ArgumentNullException("As configurações da aplicação não foram encontradas.");

                if (config.Value.Graph == null)
                    throw new ArgumentNullException("As configurações do cliente do Microsoft Graph não foram encontradas.");

                config.Value.Graph.Validar();

                return config.Value.Graph;
            });

            services.AddMemoryCache();
        }
    }
}
