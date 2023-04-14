using CA.Api.Configuracoes;
using CA.Core.Entidades.Channel;
using CA.Identity.Entidades;
using CA.Jobs.Interfaces;
using Hangfire.Common;
using Hangfire.Server;

namespace CA.Api.Hangfire.Jobs
{
    public class JobImportacaoUsuariosMicrosoft
    {
        private readonly ConfiguracaoHangfire _config;

        private readonly IJob<Usuario> _job;
        private readonly ILogger<JobCargaCompletaChannel> _logger;

        public JobImportacaoUsuariosMicrosoft(ConfiguracaoHangfire config, IJob<Usuario> job, ILogger<JobCargaCompletaChannel> logger)
        {
            _config = config;
            _job = job;  

            _logger = logger;
        }

        public async Task ExecutarAsync(PerformContext context)
        {
            using (_logger.BeginScope(context))
            {
                if (_config.HabilitarJobsImportacaoUsuarios)
                {
                    _logger.LogInformation("===> Iniciando a execução do Job de Importação de Usuários Microsoft. <===");

                    await _job.ExecutarAsync();

                    _logger.LogInformation("===> Finalizando a execução dos Job de Importação de Usuários Microsoft. <===");
                }
                else
                {
                    _logger.LogInformation("===> A execução dos Job de Importação de Usuários Microsoft foi desabilitada. <===");
                }
            }
        }
    }
}
