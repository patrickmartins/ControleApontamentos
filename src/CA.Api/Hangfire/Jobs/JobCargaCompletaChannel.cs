using CA.Api.Configuracoes;
using CA.Core.Entidades.Channel;
using CA.Jobs.Interfaces;
using Hangfire.Server;

namespace CA.Api.Hangfire.Jobs
{
    public class JobCargaCompletaChannel
    {
        private readonly ConfiguracaoHangfire _config;

        private readonly IJob<UsuarioChannel> _jobUsuario;
        private readonly IJob<ProjetoChannel> _jobProjeto;
        private readonly IJob<ApontamentoChannel> _jobApontamento;
        private readonly ILogger<JobCargaCompletaChannel> _logger;

        public JobCargaCompletaChannel(ConfiguracaoHangfire config, IJob<UsuarioChannel> jobUsuario, IJob<ProjetoChannel> jobProjeto, IJob<ApontamentoChannel> jobApontamento, ILogger<JobCargaCompletaChannel> logger)
        {
            _config = config;

            _jobApontamento = jobApontamento;            
            _jobUsuario = jobUsuario;
            _jobProjeto = jobProjeto;            

            _logger = logger;
        }

        public async Task ExecutarAsync(PerformContext context)
        {
            using (_logger.BeginScope(context))
            {
                if (_config.HabilitarJobs)
                {
                    _logger.LogInformation("===> Iniciando a execução dos Jobs de Carga do Channel. <===");

                    await _jobUsuario.ExecutarAsync();
                    await _jobProjeto.ExecutarAsync();
                    await _jobApontamento.ExecutarAsync();

                    _logger.LogInformation("===> Finalizando a execução dos Jobs de Carga do Channel. <===");
                }
                else
                {
                    _logger.LogInformation("===> A execução dos Jobs de Carga do Channel foi desabilitada. <===");
                }
            }
        }
    }
}
