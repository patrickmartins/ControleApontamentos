using CA.Core.Entidades.Tfs;
using System.Security.Claims;
using System.Security.Principal;

namespace CA.Seguranca.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string ObterNomeUsuario(this ClaimsIdentity identity)
        {
            var nomeUsuario = identity.Claims.FirstOrDefault(c => c.Type.Equals(TiposClaims.Email))?.Value;

            if (!string.IsNullOrEmpty(nomeUsuario))
                return nomeUsuario.Split('@')[0];

            return string.Empty;
        }

        public static string ObterNomeCompleto(this ClaimsIdentity identity)
        {
            return identity.Claims.First(c => c.Type.Equals(TiposClaims.NomeCompleto)).Value;
        }

        public static string ObterEmailUsuario(this ClaimsIdentity identity)
        {
            return identity.Claims.First(c => c.Type.Equals(TiposClaims.Email)).Value;
        }

        public static string ObterDominioUsuario(this ClaimsIdentity identity)
        {
            return identity.Claims.First(c => c.Type.Equals(TiposClaims.DominioTfs)).Value;
        }

        public static string ObterIdentidadeTfs(this ClaimsIdentity identity)
        {
            return identity.Claims.First(c => c.Type.Equals(TiposClaims.IdentidadeTfs)).Value;
        }
        public static string ObterTipoIdentidadeTfs(this ClaimsIdentity identity)
        {
            return identity.Claims.First(c => c.Type.Equals(TiposClaims.TipoIdentidadeTfs)).Value;
        }

        public static string? ObterPisFuncionario(this ClaimsIdentity identity)
        {
            return identity.Claims.FirstOrDefault(c => c.Type.Equals(TiposClaims.PisFuncionario))?.Value;
        }

        public static void AdicionaColecoesTfsClaim(this ClaimsIdentity identity, ICollection<string> colecoesTfs)
        {
            identity.AddClaim(new Claim(TiposClaims.ColecoesTfs, string.Join(",", colecoesTfs)));
        }

        public static ICollection<string> ObterColecoesTfs(this ClaimsIdentity identity)
        {
            var colecoes = identity.Claims.FirstOrDefault(c => c.Type.Equals(TiposClaims.ColecoesTfs))?.Value;

            if (colecoes != null)
                return colecoes.Split(';').ToList();

            return new List<string>();
        }

        public static UsuarioTfs ObterUsuarioTfs(this ClaimsIdentity identity)
        {
            var tipoIdentidadeTfs = identity.ObterTipoIdentidadeTfs();
            var identidadeTfs = identity.ObterIdentidadeTfs();

            return new UsuarioTfs
            {
                Dominio = identity.ObterDominioUsuario(),
                Email = identity.ObterEmailUsuario(),
                NomeCompleto = identity.ObterNomeCompleto(),
                NomeUsuario = identity.ObterNomeUsuario(),
                Colecoes = ObterColecoesTfs(identity),
                Identidade = new IdentidadeTfs
                {
                    Tipo = tipoIdentidadeTfs,
                    Id = identidadeTfs
                }
            };
        }

        public static string ObterNomeUsuario(this IPrincipal principal)
        {
            if(principal.Identity is null || !principal.Identity.IsAuthenticated)
                return string.Empty;

            return ((ClaimsIdentity)principal.Identity).ObterNomeUsuario();
        }

        public static string ObterNomeCompleto(this IPrincipal principal)
        {
            if (principal.Identity is null || !principal.Identity.IsAuthenticated)
                return string.Empty;

            return ((ClaimsIdentity)principal.Identity).ObterNomeCompleto();
        }

        public static string ObterEmailUsuario(this IPrincipal principal)
        {
            if (principal.Identity is null || !principal.Identity.IsAuthenticated)
                return string.Empty;

            return ((ClaimsIdentity)principal.Identity).ObterEmailUsuario();
        }

        public static string ObterDominioUsuario(this IPrincipal principal)
        {
            if (principal.Identity is null || !principal.Identity.IsAuthenticated)
                return string.Empty;

            return ((ClaimsIdentity)principal.Identity).ObterDominioUsuario();
        }

        public static string ObterIdentidadeTfs(this IPrincipal principal)
        {
            if (principal.Identity is null || !principal.Identity.IsAuthenticated)
                return string.Empty;

            return ((ClaimsIdentity)principal.Identity).ObterIdentidadeTfs();
        }

        public static string ObterTipoIdentidadeTfs(this IPrincipal principal)
        {
            if (principal.Identity is null || !principal.Identity.IsAuthenticated)
                return string.Empty;

            return ((ClaimsIdentity)principal.Identity).ObterTipoIdentidadeTfs();
        }

        public static string? ObterPisFuncionario(this IPrincipal principal)
        {
            if (principal.Identity is null || !principal.Identity.IsAuthenticated)
                return string.Empty;

            return ((ClaimsIdentity)principal.Identity).ObterPisFuncionario();
        }

        public static UsuarioTfs? ObterUsuarioTfs(this IPrincipal principal)
        {
            if (principal.Identity is null || !principal.Identity.IsAuthenticated)
                return null;

            return ((ClaimsIdentity)principal.Identity).ObterUsuarioTfs();
        }

        public static ICollection<string> ObterColecoesTfs(this IPrincipal principal)
        {
            if (principal.Identity is null || !principal.Identity.IsAuthenticated)
                return new List<string>();

            return ((ClaimsIdentity)principal.Identity).ObterColecoesTfs();
        }
    }

    public static class TiposClaims
    {
        public const string Email = "preferred_username";
        public const string NomeCompleto = "name";
        public const string DominioTfs = "tfs_dominio";
        public const string IdentidadeTfs = "tfs_identidade";
        public const string TipoIdentidadeTfs = "tfs_tipo_identidade";
        public const string ColecoesTfs = "tfs_colecoes";
        public const string PisFuncionario = "secullum_pis";
    }
}