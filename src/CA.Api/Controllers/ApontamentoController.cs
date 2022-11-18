using CA.Aplicacao.Interfaces;
using CA.Seguranca.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CA.Api.Controllers
{
    [Authorize]
    [ApiController]    
    [Route("api/apontamento")]
    public class ApontamentoController : ControllerBase
    {
        private readonly IServicoTfsApp _servico;

        public ApontamentoController(IServicoTfsApp servicoTfs)
        {
            _servico = servicoTfs;
        }

        [HttpGet]
        [Route("por-dia")]
        public async Task<ActionResult> PorDia(DateTime data)
        {
            var usuario = User.ObterUsuarioTfs();

            if (usuario is null)
                return Unauthorized();

            var resultado = await _servico.ObterApontamentosPorDiaAsync(usuario, DateOnly.FromDateTime(data));

            if(!resultado.Sucesso)
                return BadRequest(resultado.Erros);
            
            return Ok(resultado.Valor);
        }

        [HttpGet]
        [Route("por-mes")]
        public async Task<ActionResult> PorMes(int mes, int ano)
        {
            var usuario = User.ObterUsuarioTfs();

            if (usuario is null)
                return Unauthorized();

            var resultado = await _servico.ObterApontamentosPorMesAsync(usuario, mes, ano);

            if (!resultado.Sucesso)
                return BadRequest(resultado.Erros);

            return Ok(resultado.Valor);
        }
    }
}