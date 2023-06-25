using CA.Aplicacao.Enums;
using CA.Aplicacao.Interfaces;
using CA.Container.IoC;
using CA.Core.Entidades.CA;
using CA.Core.Entidades.Channel;
using CA.Jobs.Configuracoes;
using CA.Jobs.ConsoleApp.Configuracoes;
using CA.Jobs.Interfaces;
using CA.Polly;
using CA.Servicos.Graph.Configuracao;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
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
ConfiguracaoIdentity.AdicionarIdentity(servicos, configuracoes);
ConfiguracaoJson.ConfigurarSerializacaoJson();
ConfiguracaoGraph.ConfigurarMicrosoftGraph(servicos);

servicos.AdicionarConsoleLogger(configuracoes);
servicos.AdicionarContextoDeDados(configuracoes);
servicos.AdicionarJobs();

var provider = servicos.BuildServiceProvider();

var jobUsuarios = provider.GetService<IJob<UsuarioChannel>>();
var jobProjetos = provider.GetService<IJob<ProjetoChannel>>();
var jobApontamentos = provider.GetService<IJob<ApontamentoChannel>>();
var jobImportacaoUsuarios = provider.GetService<IJob<UsuarioCA>>();

var servico = provider.GetService<IServicoRelatorioApp>();

//var relatorio = await servico.ObterRelatorioDeApontamentosPorMesAsync(5, 2023, TipoOrdenacaoRelatorio.DiferencaHoras);

//await jobImportacaoUsuarios.ExecutarAsync();
await jobProjetos.ExecutarAsync();
await jobApontamentos.ExecutarAsync();

Console.WriteLine("Digite qualquer tecla para finalizar...");
Console.ReadKey();
