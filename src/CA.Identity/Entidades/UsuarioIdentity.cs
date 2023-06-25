using CA.Core.Entidades.CA;
using Microsoft.AspNetCore.Identity;

namespace CA.Identity.Entidades
{
    public class UsuarioIdentity : IdentityUser<string>
    {
        public IEnumerable<IdentityUserClaim<string>> UserClaims { get; set; }

        public UsuarioCA Usuario { get; set; }
    }
}
