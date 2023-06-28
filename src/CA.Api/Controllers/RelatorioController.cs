using CA.Aplicacao.Enums;
using CA.Aplicacao.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CA.Api.Controllers
{
    [Authorize(Roles = "administrador")]
    [Route("api/relatorio")]
    [ApiController]
    public class RelatorioController : ControllerBase
    {
        private readonly IServicoRelatorioApp _servico;

        public RelatorioController(IServicoRelatorioApp servico)
        {
            _servico = servico;
        }

        [HttpGet]
        [Route("apontamentos-por-mes")]
        public async Task<ActionResult> ObterRelatorioDeApontamentosPorMesAsync(int mes, int ano, TipoOrdenacaoRelatorio ordenacao, bool somenteApontamentosAteDiaAnterior = false, bool somenteApontamentosSincronizados = false, bool somenteUsuariosComCadastroNoPonto = false, bool somenteUsuariosComTempoTrabalhado = false)
        {
            var relatorioResult = await _servico.ObterRelatorioDeApontamentosPorMesAsync(mes, ano, ordenacao, somenteApontamentosAteDiaAnterior, somenteApontamentosSincronizados, somenteUsuariosComCadastroNoPonto, somenteUsuariosComTempoTrabalhado);

            if (!relatorioResult.Sucesso)
                return BadRequest(relatorioResult.Erros);

            return Ok(relatorioResult.Valor);
        }
    }
}
