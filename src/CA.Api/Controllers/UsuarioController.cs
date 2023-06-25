using CA.Aplicacao.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CA.Api.Controllers
{
    [Authorize(Roles = "administrador")]
    [ApiController]
    [Route("api/usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly IServicoUsuariosCaApp _servicoUsuariosCa;

        public UsuarioController(IServicoUsuariosCaApp servicoUsuarios)
        {
            _servicoUsuariosCa = servicoUsuarios;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> ObterTodosUsuariosAsync()
        {
            var usuarios = await _servicoUsuariosCa.ObterTodosUsuariosAsync();
            
            return Ok(usuarios);
        }

        [HttpGet]
        [Route("gerentes")]
        public async Task<ActionResult> ObterTodosGerentesAsync()
        {
            var usuarios = await _servicoUsuariosCa.ObterTodosGerentesAsync();

            return Ok(usuarios);
        }
    }
}
