using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace CA.Identity.Models
{
    public class UsuarioApp
    {
        public Guid Id { get; set; }
        public string? IdUsuarioTfs { get; set; }
        public int? IdUsuarioChannel { get; set; }
        public int? IdFuncionarioPonto { get; set; }
        public string NomeCompleto { get; set; }
        public string NomeUsuario { get; set; }
        public string Email { get; set; }
        public bool PossuiContaPonto { get; set; }
        public bool PossuiContaTfs { get; set; }
        public bool PossuiContaChannel { get; set; }

        public bool EhAdministrador { get; set; }

        public IEnumerable<string> Colecoes { get; set; }
        public IEnumerable<string> Roles { get; set; }

        [JsonIgnore]
        public IEnumerable<Claim> Claims { get; set; }

        public UsuarioApp()
        {
            Colecoes = new HashSet<string>();
            Roles = new HashSet<string>();
            Claims = new HashSet<Claim>();
        }
    }

    public class AtualizarUsuarioApp
    {
        [Required(ErrorMessage = "O id do usuário não foi informado!")]
        public Guid IdUsuario { get; set; }
        public string? IdUsuarioTfs { get; set; }
        public int? IdUsuarioChannel { get; set; }
        public int? IdFuncionarioPonto { get; set; }
        public bool EhAdministrador { get; set; }
    }
}
