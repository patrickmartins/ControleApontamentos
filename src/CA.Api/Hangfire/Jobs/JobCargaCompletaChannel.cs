using CA.Core.Entidades.Channel;
using CA.Jobs.Channel.Interfaces;
using Hangfire.Server;

namespace CA.Api.Hangfire.Jobs
{
    public class JobCargaCompletaChannel
    {
        private readonly IJobChannel<UsuarioChannel> _jobUsuario;
        private readonly IJobChannel<ProjetoChannel> _jobProjeto;
        private readonly IJobChannel<ApontamentoChannel> _jobApontamento;
        private readonly ILogger<JobCargaCompletaChannel> _logger;

        public JobCargaCompletaChannel(IJobChannel<UsuarioChannel> jobUsuario, IJobChannel<ProjetoChannel> jobProjeto, IJobChannel<ApontamentoChannel> jobApontamento, ILogger<JobCargaCompletaChannel> logger)
        {
            _jobApontamento = jobApontamento;
            _jobUsuario = jobUsuario;
            _jobProjeto = jobProjeto;            

            _logger = logger;
        }

        public async Task ExecutarAsync(PerformContext context)
        {
            using (_logger.BeginScope(context))
            {
                _logger.LogInformation("===> Iniciando a execução dos Jobs de Carga do Channel. <===");                

                await _jobUsuario.ExecutarAsync();
                await _jobProjeto.ExecutarAsync();
                await _jobApontamento.ExecutarAsync();

                _logger.LogInformation("===> Finalizando a execução dos Jobs de Carga do Channel. <===");
            }
        }
    }
}
