using CA.Container.IoC;
using CA.Core.Entidades.Channel;
using CA.Jobs.Channel.Interfaces;
using CA.Jobs.ConsoleApp.Configuracoes;
using CA.Polly;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var servicos = new ServiceCollection();

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

var builder = new ConfigurationBuilder()
                        .AddJsonFile($"appsettings.json", true, true)
                        .AddJsonFile($"appsettings.{environment}.json", true, true)
                        .AddEnvironmentVariables();

var configuracoes = builder.Build();

CaContainer.RegistrarServicos(servicos, configuracoes);
ConfiguracaoPolly.RegistrarPoliticasPolly(servicos);
ConfiguracaoFlurl.ConfigurarFlurl();

servicos.AdicionarContextoDeDados(configuracoes);
servicos.AdicionarJobsChannel();

var provider = servicos.BuildServiceProvider();

var jobUsuarios = provider.GetService<IJobChannel<UsuarioChannel>>();
var jobProjetos = provider.GetService<IJobChannel<ProjetoChannel>>();
var jobAtividades = provider.GetService<IJobChannel<AtividadeChannel>>();
var jobApontamentos = provider.GetService<IJobChannel<ApontamentoChannel>>();

await jobUsuarios.ExecutarAsync();
await jobProjetos.ExecutarAsync();
await jobAtividades.ExecutarAsync();
await jobApontamentos.ExecutarAsync();
