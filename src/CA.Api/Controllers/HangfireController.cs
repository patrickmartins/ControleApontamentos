﻿using CA.Api.Configuracoes;
using CA.Api.Models;
using CronExpressionDescriptor;
using Hangfire;
using Hangfire.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CA.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HangfireController : ControllerBase
    {
        private readonly ConfiguracaoHangfire _configs;

        public HangfireController(ConfiguracaoHangfire configs)
        {
            _configs = configs;
        }

        [HttpGet]
        [Route("obter-job-carga")]
        public ActionResult ObterJobCarga()
        {
            using (var connection = JobStorage.Current.GetConnection())
            {
                var job = connection.GetRecurringJobs().FirstOrDefault(p => p.Id == _configs.IdJobCargaChannelCompleta && p.LastJobState == "Succeeded");

                if (job != null)
                {
                    return Ok(new JobInfo
                    {
                        Id = job.Id,
                        UltimaExecucao = job.LastExecution,
                        ProximaExecucao = job.NextExecution,
                        IntervaloExecucao = ExpressionDescriptor.GetDescription(_configs.FrequenciaExecucaoJobsChannelCompleta).ToLower()
                    });
                }
                else
                    return NoContent();
            }
        }
    }
}
