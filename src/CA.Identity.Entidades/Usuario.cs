using Microsoft.AspNetCore.Identity;

namespace CA.Identity.Entidades
{
    public class Usuario : IdentityUser<string> 
    {
        public IEnumerable<IdentityUserRole<string>> UserRoles { get; set; }
        public IEnumerable<IdentityUserClaim<string>> UserClaims { get; set; }

        public Usuario()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
