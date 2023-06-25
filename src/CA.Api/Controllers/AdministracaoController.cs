using CA.Aplicacao.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CA.Api.Controllers
{
    [Authorize(Roles = "administrador")]
    [Route("api/administracao")]
    [ApiController]
    public class AdministracaoController : ControllerBase
    {
        private readonly IServicoAdministracaoApp _servicoAdministracao;

        public AdministracaoController(IServicoAdministracaoApp servicoAdministracao)
        {
            _servicoAdministracao = servicoAdministracao;
        }

        [HttpGet]
        [Route("unidade")]
        public async Task<ActionResult> ObterTodasUnidadesAsync()
        {
            var unidades = await _servicoAdministracao.ObterTodasUnidadesAsync();

            return Ok(unidades);
        }
    }
}
