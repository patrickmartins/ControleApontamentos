using Microsoft.AspNetCore.Identity;

namespace CA.Identity.Entidades
{
    public class Usuario : IdentityUser<string> 
    {
        public IEnumerable<Role> Roles { get; set; }
        public IEnumerable<IdentityUserClaim<string>> Claims { get; set; }

        public Usuario()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
