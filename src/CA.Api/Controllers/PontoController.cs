using CA.Aplicacao.Interfaces;
using CA.Seguranca.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CA.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/ponto")]
    public class PontoController : ControllerBase
    {
        private readonly IServicoPontoApp _servico;

        public PontoController(IServicoPontoApp servico)
        {
            _servico = servico;
        }

        [HttpGet]
        [Route("por-dia")]
        public async Task<ActionResult> BatidasPorDiaAsync(DateTime data)
        {
            var pis = User.ObterPisFuncionario();

            if(string.IsNullOrEmpty(pis))
                return NotFound();

            var resultado = await _servico.ObterBatidasPorDiaAsync(pis, DateOnly.FromDateTime(data));

            if (!resultado.Sucesso)
                return BadRequest(resultado.Erros);

            return Ok(resultado.Valor);
        }

        [HttpGet]
        [Route("por-mes")]
        public async Task<ActionResult> BatidasPorMesAsync(int mes, int ano)
        {
            var pis = User.ObterPisFuncionario();

            if (string.IsNullOrEmpty(pis))
                return Unauthorized();

            var resultado = await _servico.ObterBatidasPorMesAsync(pis, mes, ano);

            if (!resultado.Sucesso)
                return BadRequest(resultado.Erros);

            return Ok(resultado.Valor);
        }
    }
}
