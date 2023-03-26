using CA.Identity.Extensions;
using CA.Identity.Interfaces;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CA.Api.Controllers
{
    [ApiController]
    [Route("api/conta")]
    [Authorize(AuthenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme)]
    public class ContaController : ControllerBase
    {
        private readonly IServicoIdentidade _servicoIdentidade;

        public ContaController(IServicoIdentidade servicoIdentidade)
        {
            _servicoIdentidade = servicoIdentidade;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> LoginAsync()
        {
            var resultadoUsuario = await _servicoIdentidade.ObterUsuarioPorEmailAsync(User.ObterEmailUsuario());

            if(!resultadoUsuario.Sucesso)
            {
                resultadoUsuario = await _servicoIdentidade.ImportarUsuarioAsync(User);

                if (!resultadoUsuario.Sucesso)
                    return BadRequest(resultadoUsuario.Erros);
            }
            
            var resultadoLogin = _servicoIdentidade.Login(resultadoUsuario.Valor);

            if (!resultadoLogin.Sucesso)
                return Unauthorized(resultadoUsuario.Erros);

            return Ok(new
            {
                Token = resultadoLogin.Valor,
                Usuario = resultadoUsuario.Valor
            });
        }
    }
}
