using CA.Api.Hangfire.Jobs;
using Hangfire;
using Hangfire.Console;
using Hangfire.Storage.SQLite;
using HangfireBasicAuthenticationFilter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace CA.Api.Configuracoes
{
    public class ConfiguracaoHangfire
    {
        public static string SecaoConfig = "Hangfire";

        public bool HabilitarJobs { get; set; }
        public string IdJobCargaCompleta { get; set; }
        public string UsuarioDashboard { get; set; }
        public string SenhaDashboard { get; set; }
        public int IntervaloExecucao { get; set; }
    }

    public static class ConfiguracaoHangfireExtensions
    {
        public static void AdicionarHangfire(this IServiceCollection servicos, IConfiguration configuracoes)
        {
            servicos.AddHangfire(config =>
            {
                config.UseFilter(new AutomaticRetryAttribute
                {
                    DelaysInSeconds = new int [] { 300, 600 },
                    Attempts = 3,
                });

                config.UseSQLiteStorage(configuracoes.GetConnectionString("ConexaoBdHangfire"));
                config.UseConsole(new ConsoleOptions
                {
                    BackgroundColor = "#0d3163",
                    TextColor = "#ffffff",
                    TimestampColor = "#fff708;"
                });
            });

            servicos.AddHangfireServer();
        }

        public static void AdicionarJobHangfire(this IServiceCollection servicos, IConfiguration configuracoes)
        {
            servicos.Configure<ConfiguracaoHangfire>(configuracoes.GetSection(ConfiguracaoHangfire.SecaoConfig));

            servicos.AddScoped<JobCargaCompletaChannel>();

            servicos.AddSingleton((service) =>
            {
                var config = service.GetService<IOptions<ConfiguracaoHangfire>>();

                if (config == null)
                    throw new ArgumentNullException("As configurações do Hangfire não foram encontradas.");

                return config.Value;
            });
        }

        public static void UsarDashboardHanfire(this IApplicationBuilder app)
        {
            var configs = app.ApplicationServices.GetService<IOptions<ConfiguracaoHangfire>>()?.Value;

            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                DashboardTitle = "Hangfire Dashboard",
                
                Authorization = new []
                {
                    new HangfireCustomBasicAuthenticationFilter 
                    {
                        User = configs.UsuarioDashboard,
                        Pass = configs.SenhaDashboard
                    }
                }
            });
        }

        public static void UsarHangfire(this IApplicationBuilder app)
        {
            var configs = app.ApplicationServices.GetService<ConfiguracaoHangfire>();
            
            RecurringJob.AddOrUpdate<JobCargaCompletaChannel>(recurringJobId: configs.IdJobCargaCompleta, job => job.ExecutarAsync(null), $"*/{configs.IntervaloExecucao} * * * *");
        }
    }
}
