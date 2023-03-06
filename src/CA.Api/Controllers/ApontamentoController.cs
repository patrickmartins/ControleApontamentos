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
        private readonly IServicoTfsApp _servicoTfs;
        private readonly IServicoChannelApp _servicoChannel;

        public ApontamentoController(IServicoTfsApp servicoTfs, IServicoChannelApp servicoChannel)
        {
            _servicoTfs = servicoTfs;
            _servicoChannel = servicoChannel;
        }

        [HttpGet]
        [Route("tfs/por-dia")]
        public async Task<ActionResult> ObterApontamentosTfsPorDiaAsync(DateTime data)
        {
            var usuario = User.ObterUsuarioTfs();

            if (usuario is null)
                return Unauthorized();

            var resultado = await _servicoTfs.ObterApontamentosPorDiaAsync(usuario, DateOnly.FromDateTime(data));

            if(!resultado.Sucesso)
                return BadRequest(resultado.Erros);
            
            return Ok(resultado.Valor);
        }

        [HttpGet]
        [Route("tfs/por-mes")]
        public async Task<ActionResult> ObterApontamentosTfsPorMesAsync(int mes, int ano)
        {
            var usuario = User.ObterUsuarioTfs();

            if (usuario is null)
                return Unauthorized();

            var resultado = await _servicoTfs.ObterApontamentosPorMesAsync(usuario, mes, ano);

            if (!resultado.Sucesso)
                return BadRequest(resultado.Erros);

            return Ok(resultado.Valor);
        }

        [HttpGet]
        [Route("channel/por-dia")]
        public async Task<ActionResult> ObterApontamentosChannelPorDiaAsync(DateTime data)
        {
            var usuario = User.ObterUsuarioChannel();

            if (usuario is null)
                return Unauthorized();

            var resultado = await _servicoChannel.ObterApontamentosPorDiaAsync(usuario.Id, DateOnly.FromDateTime(data));

            if (!resultado.Sucesso)
                return BadRequest(resultado.Erros);

            return Ok(resultado.Valor);
        }

        [HttpGet]
        [Route("channel/por-mes")]
        public async Task<ActionResult> ObterApontamentosChannelPorMesAsync(int mes, int ano)
        {
            var usuario = User.ObterUsuarioChannel();

            if (usuario is null)
                return Unauthorized();

            var resultado = await _servicoChannel.ObterApontamentosPorMesAsync(usuario.Id, mes, ano);

            if (!resultado.Sucesso)
                return BadRequest(resultado.Erros);

            return Ok(resultado.Valor);
        }
    }
}