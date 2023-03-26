using System.Security.Claims;
using System.Text.Json.Serialization;

namespace CA.Identity.Models
{
    public class UsuarioApp
    {
        public Guid Id { get; set; }
        public string NomeCompleto { get; set; }
        public string NomeUsuario { get; set; }
        public string Email { get; set; }
        public bool PossuiContaPonto { get; set; }
        public bool PossuiContaTfs { get; set; }
        public bool PossuiContaChannel { get; set; }

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
}
