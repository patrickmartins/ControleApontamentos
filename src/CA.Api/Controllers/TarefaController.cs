using CA.Aplicacao.Interfaces;
using CA.Aplicacao.Models;
using CA.Core.Entidades.Tfs;
using CA.Core.Extensions;
using CA.Core.Valores;
using CA.Seguranca.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CA.Api.Controllers
{
    [Authorize]
    [ApiController]    
    [Route("api/tarefa")]
    public class TarefaController : ControllerBase
    {
        private readonly IServicoTfsApp _servico;

        public TarefaController(IServicoTfsApp servico)
        {
            _servico = servico;
        }

        [HttpGet]
        [Route("ativas")]
        public async Task<ActionResult> ObterTarefasAtivasAsync()
        {
            var usuario = User.ObterUsuarioTfs();

            if (usuario is null)
                return Unauthorized();

            var resultado = await _servico.ObterTarefasAtivasPorUsuarioAsync(usuario);

            if (!resultado.Sucesso)
                return BadRequest(resultado.Erros.ParaDicionario());

            return Ok(resultado.Valor);
        }

        [HttpGet]
        [Route("por-ids")]
        public async Task<ActionResult> ObterTarefasPorIdAsync([FromQuery] string colecao, [FromQuery] int[] ids)
        {
            if (ids.Length >= 10)
                return BadRequest(new Erro("A quantidade máxima de ids consultados é de 10.", nameof(ids)));

            var usuario = User.ObterUsuarioTfs();

            if (usuario is null)
                return Unauthorized();

            if (!usuario.PossuiAcessoColecao(colecao))
                return Ok(new List<TarefaModel>());

            var resultado = await _servico.ObterTarefasPorIdAsync(usuario, colecao, ids);

            if (!resultado.Sucesso)
                return BadRequest(resultado.Erros.ParaDicionario());

            return Ok(resultado.Valor);
        }

        [HttpGet]
        [Route("buscar")]
        public async Task<ActionResult> Buscar(string palavraChave, string colecao, [FromQuery] StatusItemTrabalho[] status, int pagina = 1, int tamanhoPagina = 10)
        {
            var usuario = User.ObterUsuarioTfs();

            if (usuario is null)
                return Unauthorized();

            if (!usuario.PossuiAcessoColecao(colecao))
                return Unauthorized(new Erro($"Você não possui acesso a coleção '{colecao}'.", "colecao"));

            var resultado = await _servico.BuscarTarefasAsync(usuario, colecao, palavraChave, status, pagina, tamanhoPagina);

            if (!resultado.Sucesso)
                return BadRequest(resultado.Erros);

            return Ok(resultado.Valor);
        }


        [HttpPost]
        [Route("adicionar-apontamento")]
        public async Task<ActionResult> AdicionarApontamento(ApontamentoTfsNovoModel apontamento)
        {
            var usuario = User.ObterUsuarioTfs();

            if (usuario is null)
                return Unauthorized();

            if (!usuario.PossuiAcessoColecao(apontamento.Colecao))
                return Unauthorized(new Erro($"Você não possui acesso a coleção '{apontamento.Colecao}'.", "colecao"));

            apontamento.Usuario = usuario.NomeUsuario;

            var resultado = await _servico.AdicionarNovoApontamentoAsync(usuario, apontamento);

            if (!resultado.Sucesso)
                return BadRequest(resultado.Erros);

            return Ok();
        }
    }
}