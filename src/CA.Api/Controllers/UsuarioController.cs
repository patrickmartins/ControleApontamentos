using CA.Core.Valores;
using CA.Identity.Interfaces;
using CA.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CA.Api.Controllers
{
    [Authorize(Roles = "administrador")]
    [ApiController]
    [Route("api/usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly IServicoIdentidade _servicoIdentidade;

        public UsuarioController(IServicoIdentidade servicoIdentidade)
        {
            _servicoIdentidade = servicoIdentidade;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> ObterTodosUsuariosAsync()
        {
            var usuarios = await _servicoIdentidade.ObterTodosUsuariosAsync();
            
            return Ok(usuarios);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> AtualizarUsuarioAsync(AtualizarUsuarioApp model)
        {
            var resultado = await _servicoIdentidade.AtualizarUsuario(model);

            if (!resultado.Sucesso)
                return BadRequest(resultado.Erros);

            return Ok();
        }
    }
}
