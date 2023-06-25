using System.ComponentModel.DataAnnotations;

namespace CA.Aplicacao.Models
{
    public record UsuarioModel
    {
        public Guid Id { get; set; }
        public string? IdUsuarioTfs { get; set; }
        public int? IdUsuarioChannel { get; set; }
        public int? IdFuncionarioPonto { get; set; }
        public string? IdUnidade { get; set; }
        public string? IdGerente { get; set; }
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public bool PossuiContaPonto { get; set; }
        public bool PossuiContaTfs { get; set; }
        public bool PossuiContaChannel { get; set; }

        public UnidadeModel? Unidade { get; set; }
        public GerenteModel? Gerente { get; set; }
    }

    public record AtualizarUsuarioModel
    {
        [Required(ErrorMessage = "O id do usuário não foi informado!")]
        public Guid IdUsuario { get; set; }
        public string? IdUnidade { get; set; }
        public string? IdGerente { get; set; }
        public string? IdUsuarioTfs { get; set; }
        public int? IdUsuarioChannel { get; set; }
        public int? IdFuncionarioPonto { get; set; }
    }
}
