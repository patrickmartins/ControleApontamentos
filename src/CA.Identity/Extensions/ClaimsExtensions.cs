﻿using CA.Core.Entidades.Channel;
using CA.Core.Entidades.Ponto;
using CA.Core.Entidades.Tfs;
using System.Security.Claims;
using System.Security.Principal;

namespace CA.Identity.Extensions
{
    public static class ClaimsExtensions
    {
        public static string ObterNomeUsuario(this IEnumerable<Claim> claims)
        {
            var nomeUsuario = claims.FirstOrDefault(c => c.Type.Equals(TiposClaims.Email))?.Value;

            if (!string.IsNullOrEmpty(nomeUsuario))
                return nomeUsuario.Split('@')[0];

            return string.Empty;
        }

        public static string? ObterNomeUsuarioTfs(this IEnumerable<Claim> claims)
        {
            return claims.FirstOrDefault(c => c.Type.Equals(TiposClaims.NomeUsuarioTfs))?.Value;

        }

        public static string? ObterNomeCompleto(this IEnumerable<Claim> claims)
        {
            return claims.FirstOrDefault(c => c.Type.Equals(TiposClaims.NomeCompleto))?.Value;
        }

        public static string? ObterEmailUsuario(this IEnumerable<Claim> claims)
        {
            return claims.FirstOrDefault(c => c.Type.Equals(TiposClaims.Email))?.Value;
        }

        public static string? ObterDominioUsuario(this IEnumerable<Claim> claims)
        {
            return claims.FirstOrDefault(c => c.Type.Equals(TiposClaims.DominioTfs))?.Value;
        }

        public static string? ObterIdentidadeTfs(this IEnumerable<Claim> claims)
        {
            return claims.FirstOrDefault(c => c.Type.Equals(TiposClaims.IdentidadeTfs))?.Value;
        }
        public static string? ObterTipoIdentidadeTfs(this IEnumerable<Claim> claims)
        {
            return claims.FirstOrDefault(c => c.Type.Equals(TiposClaims.TipoIdentidadeTfs))?.Value;
        }

        public static string? ObterPisFuncionario(this IEnumerable<Claim> claims)
        {
            return claims.FirstOrDefault(c => c.Type.Equals(TiposClaims.PisFuncionario))?.Value;
        }

        public static int? ObterIdFuncionarioPonto(this IEnumerable<Claim> claims)
        {
            var valor = claims.FirstOrDefault(c => c.Type.Equals(TiposClaims.IdFuncionario))?.Value;

            if (valor is null)
                return null;

            if (int.TryParse(valor, out var id))
                return id;

            return null;
        }

        public static string? ObterEmailUsuarioChannel(this IEnumerable<Claim> claims)
        {
            return claims.FirstOrDefault(c => c.Type.Equals(TiposClaims.EmailUsuarioChannel))?.Value;
        }

        public static int? ObterIdUsuarioChannel(this IEnumerable<Claim> claims)
        {
            var valor = claims.FirstOrDefault(c => c.Type.Equals(TiposClaims.IdUsuarioChannel))?.Value;

            if (valor is null)
                return null;

            if (int.TryParse(valor, out var id))
                return id;

            return null;
        }

        public static IEnumerable<string> ObterColecoesTfs(this IEnumerable<Claim> claims)
        {
            var colecoes = claims.FirstOrDefault(c => c.Type.Equals(TiposClaims.ColecoesTfs))?.Value;

            if (colecoes != null)
                return colecoes.Split(';').ToList();

            return Enumerable.Empty<string>();
        }

        public static UsuarioTfs? ObterUsuarioTfs(this IEnumerable<Claim> claims)
        {
            var nomeUsuarioTfs = claims.ObterNomeUsuarioTfs();

            if (nomeUsuarioTfs is null)
                return null;

            var tipoIdentidadeTfs = claims.ObterTipoIdentidadeTfs();
            var identidadeTfs = claims.ObterIdentidadeTfs();

            return new UsuarioTfs
            {
                NomeUsuario = nomeUsuarioTfs,
                Dominio = claims.ObterDominioUsuario(),
                Email = claims.ObterEmailUsuario(),
                NomeCompleto = claims.ObterNomeCompleto(),
                Colecoes = claims.ObterColecoesTfs(),
                Identidade = new IdentidadeTfs
                {
                    Tipo = tipoIdentidadeTfs,
                    Id = identidadeTfs
                }
            };
        }

        public static UsuarioChannel? ObterUsuarioChannel(this IEnumerable<Claim> claims)
        {
            var idUsuario = claims.ObterIdUsuarioChannel();

            if (idUsuario is null)
                return null;

            var emailUsuario = claims.ObterEmailUsuarioChannel();

            return new UsuarioChannel
            {
                Id = idUsuario.Value,
                Email = emailUsuario
            };
        }

        public static Funcionario? ObterFuncionarioPonto(this IEnumerable<Claim> claims)
        {
            var idFuncionario = claims.ObterIdFuncionarioPonto();

            if (idFuncionario is null)
                return null;

            var pisFuncionario = claims.ObterPisFuncionario();

            return new Funcionario
            {
                Id = idFuncionario.Value,                
                NumeroPis = pisFuncionario
            };
        }

        public static string ObterNomeUsuario(this ClaimsIdentity identity)
        {
            return identity.Claims.ObterNomeUsuario();
        }

        public static string? ObterNomeCompleto(this ClaimsIdentity identity)
        {
            return identity.Claims.ObterNomeCompleto();
        }

        public static string? ObterEmailUsuario(this ClaimsIdentity identity)
        {
            return identity.Claims.ObterEmailUsuario();
        }

        public static string? ObterDominioUsuario(this ClaimsIdentity identity)
        {
            return identity.Claims.ObterDominioUsuario();
        }

        public static string? ObterIdentidadeTfs(this ClaimsIdentity identity)
        {
            return identity.Claims.ObterIdentidadeTfs();
        }
        public static string? ObterTipoIdentidadeTfs(this ClaimsIdentity identity)
        {
            return identity.Claims.ObterTipoIdentidadeTfs();
        }

        public static string? ObterPisFuncionario(this ClaimsIdentity identity)
        {
            return identity.Claims.ObterPisFuncionario();
        }

        public static string? ObterEmailUsuarioChannel(this ClaimsIdentity identity)
        {
            return identity.Claims.ObterEmailUsuarioChannel();
        }

        public static int? ObterIdUsuarioChannel(this ClaimsIdentity identity)
        {
            return identity.Claims.ObterIdUsuarioChannel();
        }

        public static UsuarioChannel? ObterUsuarioChannel(this ClaimsIdentity identity)
        {
            return identity.Claims.ObterUsuarioChannel();
        }

        public static IEnumerable<string> ObterColecoesTfs(this ClaimsIdentity identity)
        {
            return identity.Claims.ObterColecoesTfs();
        }

        public static UsuarioTfs? ObterUsuarioTfs(this ClaimsIdentity identity)
        {
            return identity.Claims.ObterUsuarioTfs();
        }

        public static Funcionario? ObterFuncionarioPonto(this ClaimsIdentity identity)
        {
            return identity.Claims.ObterFuncionarioPonto();
        }

        public static string? ObterNomeUsuario(this IPrincipal principal)
        {
            if (principal.Identity is null || !principal.Identity.IsAuthenticated)
                return null;

            return ((ClaimsIdentity)principal.Identity).ObterNomeUsuario();
        }

        public static string? ObterNomeCompleto(this IPrincipal principal)
        {
            if (principal.Identity is null || !principal.Identity.IsAuthenticated)
                return null;

            return ((ClaimsIdentity)principal.Identity).ObterNomeCompleto();
        }

        public static string? ObterEmailUsuario(this IPrincipal principal)
        {
            if (principal.Identity is null || !principal.Identity.IsAuthenticated)
                return null;

            return ((ClaimsIdentity)principal.Identity).ObterEmailUsuario();
        }

        public static string? ObterDominioUsuario(this IPrincipal principal)
        {
            if (principal.Identity is null || !principal.Identity.IsAuthenticated)
                return null;

            return ((ClaimsIdentity)principal.Identity).ObterDominioUsuario();
        }

        public static string? ObterIdentidadeTfs(this IPrincipal principal)
        {
            if (principal.Identity is null || !principal.Identity.IsAuthenticated)
                return null;

            return ((ClaimsIdentity)principal.Identity).ObterIdentidadeTfs();
        }

        public static string? ObterTipoIdentidadeTfs(this IPrincipal principal)
        {
            if (principal.Identity is null || !principal.Identity.IsAuthenticated)
                return null;

            return ((ClaimsIdentity)principal.Identity).ObterTipoIdentidadeTfs();
        }

        public static string? ObterPisFuncionario(this IPrincipal principal)
        {
            if (principal.Identity is null || !principal.Identity.IsAuthenticated)
                return null;

            return ((ClaimsIdentity)principal.Identity).ObterPisFuncionario();
        }

        public static UsuarioTfs? ObterUsuarioTfs(this IPrincipal principal)
        {
            if (principal.Identity is null || !principal.Identity.IsAuthenticated)
                return null;

            return ((ClaimsIdentity)principal.Identity).ObterUsuarioTfs();
        }

        public static Funcionario? ObterFuncionarioPonto(this IPrincipal principal)
        {
            if (principal.Identity is null || !principal.Identity.IsAuthenticated)
                return null;

            return ((ClaimsIdentity)principal.Identity).ObterFuncionarioPonto();
        }

        public static IEnumerable<string> ObterColecoesTfs(this IPrincipal principal)
        {
            if (principal.Identity is null || !principal.Identity.IsAuthenticated)
                return Enumerable.Empty<string>();

            return ((ClaimsIdentity)principal.Identity).ObterColecoesTfs();
        }

        public static string? ObterEmailUsuarioChannel(this IPrincipal principal)
        {
            if (principal.Identity is null || !principal.Identity.IsAuthenticated)
                return null;

            return ((ClaimsIdentity)principal.Identity).ObterEmailUsuarioChannel();
        }

        public static int? ObterIdUsuarioChannel(this IPrincipal principal)
        {
            if (principal.Identity is null || !principal.Identity.IsAuthenticated)
                return null;

            return ((ClaimsIdentity)principal.Identity).ObterIdUsuarioChannel();
        }

        public static UsuarioChannel? ObterUsuarioChannel(this IPrincipal principal)
        {
            if (principal.Identity is null || !principal.Identity.IsAuthenticated)
                return null;

            return ((ClaimsIdentity)principal.Identity).ObterUsuarioChannel();
        }
    }

    public static class TiposClaims
    {
        public const string Email = "preferred_username";
        public const string NomeCompleto = "name";
        public const string NomeUsuarioTfs = "tfs_nome_usuario";
        public const string DominioTfs = "tfs_dominio";
        public const string IdentidadeTfs = "tfs_identidade";
        public const string TipoIdentidadeTfs = "tfs_tipo_identidade";
        public const string ColecoesTfs = "tfs_colecoes";
        public const string IdFuncionario = "secullum_funcionario_id";
        public const string PisFuncionario = "secullum_funcionario_pis";
        public const string EmailUsuarioChannel = "channel_email_usuario";
        public const string IdUsuarioChannel = "channel_id_usuario";
    }
}