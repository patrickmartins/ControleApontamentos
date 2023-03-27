using CA.Identity.Interfaces;
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
        public async Task<ActionResult> ObterTodosUsuarios()
        {
            var usuarios = await _servicoIdentidade.ObterTodosUsuariosAsync();

            return Ok(usuarios);
        }
    }
}
