using CA.Container.IoC;
using CA.Core.Entidades.Channel;
using CA.Jobs.ConsoleApp.Configuracoes;
using CA.Jobs.Interfaces;
using CA.Polly;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CA.Jobs.Configuracoes;

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

servicos.AdicionarConsoleLogger(configuracoes);
servicos.AdicionarContextoDeDados(configuracoes);
servicos.AdicionarJobsChannel();

var provider = servicos.BuildServiceProvider();

var jobUsuarios = provider.GetService<IJob<UsuarioChannel>>();
var jobProjetos = provider.GetService<IJob<ProjetoChannel>>();
var jobApontamentos = provider.GetService<IJob<ApontamentoChannel>>();

await jobUsuarios.ExecutarAsync();
await jobProjetos.ExecutarAsync();
await jobApontamentos.ExecutarAsync();

Console.WriteLine("Digite qualquer tecla para finalizar...");
Console.ReadKey();
