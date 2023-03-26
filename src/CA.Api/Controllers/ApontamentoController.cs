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
    [Route("api/apontamento")]
    public class ApontamentoController : ControllerBase
    {
        private readonly IServicoTfsApp _servicoTfs;
        private readonly IServicoChannelApp _servicoChannel;
        private readonly IServicoIdentidade _servicoIdentidade;

        public ApontamentoController(IServicoIdentidade servicoIdentidade, IServicoTfsApp servicoTfs, IServicoChannelApp servicoChannel)
        {
            _servicoTfs = servicoTfs;
            _servicoChannel = servicoChannel;
            _servicoIdentidade = servicoIdentidade;
        }

        [HttpGet]
        [Authorize(Roles = "administrador")]
        [Route("tfs/{id:required:guid}/por-dia")]
        public async Task<ActionResult> ObterApontamentosTfsUsuarioPorDiaAsync([FromRoute] Guid id, DateTime data)
        {
            var usuario = await _servicoIdentidade.ObterUsuarioPorIdAsync(id);

            if (!usuario.Sucesso)
                return NotFound(usuario.Erros);

            var usuarioTfs = usuario.Valor.Claims.ObterUsuarioTfs();

            if(usuarioTfs is null)
                return BadRequest(Resultado.DeErros<UsuarioApp>(new Erro("O usuário informado não possui uma conta no TFS.", nameof(id))));

            var resultado = await _servicoTfs.ObterApontamentosPorDiaAsync(usuarioTfs, DateOnly.FromDateTime(data));

            if (!resultado.Sucesso)
                return BadRequest(resultado.Erros);

            return Ok(resultado.Valor);
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
        [Authorize(Roles = "administrador")]
        [Route("tfs/{id:required:guid}/por-mes")]
        public async Task<ActionResult> ObterApontamentosTfsUsuarioPorMesAsync([FromRoute] Guid id, int mes, int ano)
        {
            var usuario = await _servicoIdentidade.ObterUsuarioPorIdAsync(id);

            if (!usuario.Sucesso)
                return NotFound(usuario.Erros);

            var usuarioTfs = usuario.Valor.Claims.ObterUsuarioTfs();

            if (usuarioTfs is null)
                return BadRequest(Resultado.DeErros<UsuarioApp>(new Erro("O usuário informado não possui uma conta no TFS.", nameof(id))));

            var resultado = await _servicoTfs.ObterApontamentosPorMesAsync(usuarioTfs, mes, ano);

            if (!resultado.Sucesso)
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
        [Authorize(Roles = "administrador")]
        [Route("channel/{id:required:guid}/por-dia")]
        public async Task<ActionResult> ObterApontamentosChannelUsuarioPorDiaAsync([FromRoute] Guid id, DateTime data)
        {
            var usuario = await _servicoIdentidade.ObterUsuarioPorIdAsync(id);

            if (!usuario.Sucesso)
                return NotFound(usuario.Erros);

            var usuarioChannel = usuario.Valor.Claims.ObterUsuarioChannel();

            if (usuarioChannel is null)
                return BadRequest(Resultado.DeErros<UsuarioApp>(new Erro("O usuário informado não possui uma conta no Channel.", nameof(id))));

            var resultado = await _servicoChannel.ObterApontamentosPorDiaAsync(usuarioChannel.Id, DateOnly.FromDateTime(data));

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
        [Authorize(Roles = "administrador")]
        [Route("channel/{id:required:guid}/por-mes")]
        public async Task<ActionResult> ObterApontamentosChannelUsuarioPorMesAsync([FromRoute] Guid id, int mes, int ano)
        {
            var usuario = await _servicoIdentidade.ObterUsuarioPorIdAsync(id);

            if (!usuario.Sucesso)
                return NotFound(usuario.Erros);

            var usuarioChannel = usuario.Valor.Claims.ObterUsuarioChannel();

            if (usuarioChannel is null)
                return BadRequest(Resultado.DeErros<UsuarioApp>(new Erro("O usuário informado não possui uma conta no Channel.", nameof(id))));

            var resultado = await _servicoChannel.ObterApontamentosPorMesAsync(usuarioChannel.Id, mes, ano);

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