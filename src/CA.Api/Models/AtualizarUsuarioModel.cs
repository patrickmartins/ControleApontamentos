namespace CA.Api.Models
{
    public record AtualizarUsuarioModel : Aplicacao.Models.AtualizarUsuarioModel
    {
        public bool EhAdministrador { get; set; }
    }
}
