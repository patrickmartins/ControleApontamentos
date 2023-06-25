using CA.Aplicacao.Interfaces;
using CA.Aplicacao.Models;
using CA.Core.Extensions;
using CA.Core.Valores;
using CA.Identity.Extensions;
using CA.Identity.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CA.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/channel")]
    public class ChannelController : ControllerBase
    {
        private readonly IServicoChannelApp _servicoChannel;
        private readonly IServicoIdentidade _servicoIdentidade;

        public ChannelController(IServicoChannelApp servicoChannel, IServicoIdentidade servicoIdentidade)
        {
            _servicoChannel = servicoChannel;
            _servicoIdentidade = servicoIdentidade;            
        }

        [HttpGet]
        [Authorize(Roles = "administrador")]
        [Route("apontamento/{id:required:guid}/por-dia")]
        public async Task<ActionResult> ObterApontamentosChannelUsuarioPorDiaAsync([FromRoute] Guid id, DateTime data)
        {
            var usuario = await _servicoIdentidade.ObterContaUsuarioPorIdAsync(id);

            if (!usuario.Sucesso)
                return NotFound(usuario.Erros);

            var usuarioChannel = usuario.Valor.ExtrairUsuarioChannel();

            if (usuarioChannel is null)
                return BadRequest(Resultado.DeErros<UsuarioModel>(new Erro("O usuário informado não possui uma conta no Channel.", nameof(id))));

            var resultado = await _servicoChannel.ObterApontamentosPorDiaAsync(usuarioChannel.Id, DateOnly.FromDateTime(data));

            if (!resultado.Sucesso)
                return BadRequest(resultado.Erros);

            return Ok(resultado.Valor);
        }

        [HttpGet]
        [Route("apontamento/por-dia")]
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
        [Authorize(Roles = "administrador")]
        [Route("apontamento/{id:required:guid}/por-mes")]
        public async Task<ActionResult> ObterApontamentosChannelUsuarioPorMesAsync([FromRoute] Guid id, int mes, int ano)
        {
            var usuario = await _servicoIdentidade.ObterContaUsuarioPorIdAsync(id);

            if (!usuario.Sucesso)
                return NotFound(usuario.Erros);

            var usuarioChannel = usuario.Valor.ExtrairUsuarioChannel();

            if (usuarioChannel is null)
                return BadRequest(Resultado.DeErros<UsuarioModel>(new Erro("O usuário informado não possui uma conta no Channel.", nameof(id))));

            var resultado = await _servicoChannel.ObterApontamentosPorMesAsync(usuarioChannel.Id, mes, ano);

            if (!resultado.Sucesso)
                return BadRequest(resultado.Erros);

            return Ok(resultado.Valor);
        }

        [HttpGet]
        [Route("apontamento/por-mes")]
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

        [HttpGet]
        [Authorize(Roles = "administrador")]
        [Route("usuarios")]
        public async Task<ActionResult> ObterTodosUsuariosAsync()
        {
            return Ok(await _servicoChannel.ObterTodosUsuariosAsync());
        }
    }
}
