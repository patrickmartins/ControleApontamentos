using CA.Aplicacao.Interfaces;
using CA.Core.Valores;
using CA.Identity.Extensions;
using CA.Identity.Interfaces;
using CA.Identity.Models;
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
        private readonly IServicoIdentidade _servicoIdentidade;

        public PontoController(IServicoIdentidade servicoIdentidade, IServicoPontoApp servico)
        {
            _servico = servico;
            _servicoIdentidade = servicoIdentidade;
        }

        [HttpGet]
        [Authorize(Roles = "administrador")]
        [Route("{id:required:guid}/por-dia")]
        public async Task<ActionResult> BatidasUsuarioPorDiaAsync([FromRoute] Guid id, DateTime data)
        {
            var usuario = await _servicoIdentidade.ObterUsuarioPorIdAsync(id);

            if (!usuario.Sucesso)
                return NotFound(usuario.Erros);

            var pis = usuario.Valor.Claims.ObterPisFuncionario();

            if (string.IsNullOrEmpty(pis))
                return BadRequest(Resultado.DeErros<UsuarioApp>(new Erro("O usuário informado não possui cadastro no sistema de ponto.", nameof(id))));

            var resultado = await _servico.ObterBatidasPorDiaAsync(pis, DateOnly.FromDateTime(data));

            if (!resultado.Sucesso)
                return BadRequest(resultado.Erros);

            var batidas = resultado.Valor;

            batidas.OcultarBatidas();

            return Ok(batidas);
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
        [Authorize(Roles = "administrador")]
        [Route("{id:required:guid}/por-mes")]
        public async Task<ActionResult> BatidasUsuarioPorMesAsync([FromRoute] Guid id, int mes, int ano)
        {
            var usuario = await _servicoIdentidade.ObterUsuarioPorIdAsync(id);

            if (!usuario.Sucesso)
                return NotFound(usuario.Erros);

            var pis = usuario.Valor.Claims.ObterPisFuncionario();

            if (string.IsNullOrEmpty(pis))
                return BadRequest(Resultado.DeErros<UsuarioApp>(new Erro("O usuário informado não possui cadastro no sistema de ponto.", nameof(id))));

            var resultado = await _servico.ObterBatidasPorMesAsync(pis, mes, ano);

            if(!resultado.Sucesso)
                return BadRequest(resultado.Erros);

            var batidas = resultado.Valor;

            batidas.OcultarBatidas();

            return Ok(batidas);
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
