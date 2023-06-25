using CA.Api.Models;
using CA.Aplicacao.Interfaces;
using CA.Identity.Extensions;
using CA.Identity.Interfaces;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CA.Api.Controllers
{
    [ApiController]
    [Route("api/conta")]    
    public class ContaController : ControllerBase
    {
        private readonly IServicoIdentidade _servicoIdentidade;
        private readonly IServicoUsuariosCaApp _servicoUsuariosCa;

        public ContaController(IServicoIdentidade servicoIdentidade, IServicoUsuariosCaApp servicoUsuariosCa)
        {
            _servicoIdentidade = servicoIdentidade;
            _servicoUsuariosCa = servicoUsuariosCa;
        }

        [HttpPost]
        [Route("login")]
        [Authorize(AuthenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme)]
        public async Task<ActionResult> LoginAsync()
        {
            var resultadoConta = await _servicoIdentidade.ObterContaUsuarioPorEmailAsync(User.ObterEmailUsuario());

            if(!resultadoConta.Sucesso)
            {
                var resultadoImportacao = await _servicoUsuariosCa.ImportarUsuarioAsync(User.ObterEmailUsuario(), User.ObterNomeCompleto());

                if (!resultadoImportacao.Sucesso)
                    return BadRequest(resultadoImportacao.Erros);

                resultadoConta = await _servicoIdentidade.CriarContaUsuarioAsync(User.ObterEmailUsuario());

                if (!resultadoConta.Sucesso)
                    return BadRequest(resultadoConta.Erros);
            }
            
            var resultadoLogin = await _servicoIdentidade.LoginAsync(User.ObterEmailUsuario());

            if (!resultadoLogin.Sucesso)
                return Unauthorized(resultadoConta.Erros);

            return Ok(new
            {
                Token = resultadoLogin.Valor,
                Usuario = resultadoConta.Valor
            });
        }

        [Authorize(Roles = "administrador")]
        [HttpGet]
        [Route("")]
        public async Task<ActionResult> ObterTodasContasUsuariosAsync()
        {
            var usuarios = await _servicoIdentidade.ObterTodasContaUsuarioAsync();

            return Ok(usuarios);
        }

        [Authorize(Roles = "administrador")]
        [HttpPost]
        [Route("")]
        public async Task<ActionResult> AtualizarContaUsuarioAsync(AtualizarUsuarioModel model)
        {
            var resultadoAtualizacao = await _servicoUsuariosCa.AtualizarUsuarioAsync(model);

            if (!resultadoAtualizacao.Sucesso)
                return BadRequest(resultadoAtualizacao.Erros);

            if (model.EhAdministrador)
            {
                var resultadoAdministrador = await _servicoIdentidade.ConcederAcessoDeAdministradorAsync(resultadoAtualizacao.Valor.Email);

                if (!resultadoAdministrador.Sucesso)
                    return BadRequest(resultadoAdministrador.Erros);
            }
            else
            {
                var resultadoAdministrador = await _servicoIdentidade.RemoverAcessoDeAdministradorAsync(resultadoAtualizacao.Valor.Email);

                if (!resultadoAdministrador.Sucesso)
                    return BadRequest(resultadoAdministrador.Erros);
            }

            return Ok();
        }
    }
}
