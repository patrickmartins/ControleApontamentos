using CA.Aplicacao.Interfaces;
using CA.Aplicacao.Models;
using CA.Core.Entidades.Tfs;
using CA.Core.Extensions;
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
    [Route("api/tfs")]
    public class TfsController : ControllerBase
    {
        private readonly IServicoTfsApp _servicoTfs;
        private readonly IServicoIdentidade _servicoIdentidade;

        public TfsController(IServicoTfsApp servicoTfs, IServicoIdentidade servicoIdentidade)
        {
            _servicoTfs = servicoTfs;
            _servicoIdentidade = servicoIdentidade;
        }

        [HttpGet]
        [Authorize(Roles = "administrador")]
        [Route("apontamento/{id:required:guid}/por-dia")]
        public async Task<ActionResult> ObterApontamentosTfsUsuarioPorDiaAsync([FromRoute] Guid id, DateTime data)
        {
            var usuario = await _servicoIdentidade.ObterUsuarioPorIdAsync(id);

            if (!usuario.Sucesso)
                return NotFound(usuario.Erros);

            var usuarioTfs = usuario.Valor.Claims.ObterUsuarioTfs();

            if (usuarioTfs is null)
                return BadRequest(Resultado.DeErros<UsuarioApp>(new Erro("O usuário informado não possui uma conta no TFS.", nameof(id))));

            var resultado = await _servicoTfs.ObterApontamentosPorDiaAsync(usuarioTfs, DateOnly.FromDateTime(data));

            if (!resultado.Sucesso)
                return BadRequest(resultado.Erros);

            return Ok(resultado.Valor);
        }

        [HttpGet]
        [Route("apontamento/por-dia")]
        public async Task<ActionResult> ObterApontamentosTfsPorDiaAsync(DateTime data)
        {
            var usuario = User.ObterUsuarioTfs();

            if (usuario is null)
                return Unauthorized();

            var resultado = await _servicoTfs.ObterApontamentosPorDiaAsync(usuario, DateOnly.FromDateTime(data));

            if (!resultado.Sucesso)
                return BadRequest(resultado.Erros);

            return Ok(resultado.Valor);
        }

        [HttpGet]
        [Authorize(Roles = "administrador")]
        [Route("apontamento/{id:required:guid}/por-mes")]
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
        [Route("apontamento/por-mes")]
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
        [Route("tarefa/ativas")]
        public async Task<ActionResult> ObterTarefasAtivasAsync()
        {
            var usuario = User.ObterUsuarioTfs();

            if (usuario is null)
                return Unauthorized();

            var resultado = await _servicoTfs.ObterTarefasAtivasPorUsuarioAsync(usuario);

            if (!resultado.Sucesso)
                return BadRequest(resultado.Erros.ParaDicionario());

            return Ok(resultado.Valor);
        }

        [HttpGet]
        [Route("tarefa/por-ids")]
        public async Task<ActionResult> ObterTarefasPorIdAsync([FromQuery] string colecao, [FromQuery] int[] ids)
        {
            if (ids.Length >= 10)
                return BadRequest(new Erro("A quantidade máxima de ids consultados é de 10.", nameof(ids)));

            var usuario = User.ObterUsuarioTfs();

            if (usuario is null)
                return Unauthorized();

            if (!usuario.PossuiAcessoColecao(colecao))
                return Ok(Enumerable.Empty<TarefaModel>());

            var resultado = await _servicoTfs.ObterTarefasPorIdAsync(usuario, colecao, ids);

            if (!resultado.Sucesso)
                return BadRequest(resultado.Erros.ParaDicionario());

            return Ok(resultado.Valor);
        }

        [HttpGet]
        [Route("tarefa/buscar")]
        public async Task<ActionResult> BuscarAsync(string palavraChave, string colecao, [FromQuery] StatusItemTrabalho[] status, int pagina = 1, int tamanhoPagina = 10)
        {
            var usuario = User.ObterUsuarioTfs();

            if (usuario is null)
                return Unauthorized();

            if (!usuario.PossuiAcessoColecao(colecao))
                return Unauthorized(new Erro($"Você não possui acesso a coleção '{colecao}'.", "colecao"));

            var resultado = await _servicoTfs.BuscarTarefasAsync(usuario, colecao, palavraChave, status, pagina, tamanhoPagina);

            if (!resultado.Sucesso)
                return BadRequest(resultado.Erros);

            return Ok(resultado.Valor);
        }

        [HttpPost]
        [Route("tarefa/adicionar-apontamento")]
        public async Task<ActionResult> AdicionarApontamentoAsync(ApontamentoTfsNovoModel apontamento)
        {
            var usuario = User.ObterUsuarioTfs();

            if (usuario is null)
                return Unauthorized();

            if (!usuario.PossuiAcessoColecao(apontamento.Colecao))
                return Unauthorized(new Erro($"Você não possui acesso a coleção '{apontamento.Colecao}'.", "colecao"));

            apontamento.Usuario = usuario.NomeUsuario;

            var resultado = await _servicoTfs.AdicionarNovoApontamentoAsync(usuario, apontamento);

            if (!resultado.Sucesso)
                return BadRequest(resultado.Erros);

            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = "administrador")]
        [Route("usuarios")]
        public async Task<ActionResult> ObterTodosUsuarioAsync()
        {
            return Ok(await _servicoTfs.ObterTodosUsuariosAsync());
        }
    }
}