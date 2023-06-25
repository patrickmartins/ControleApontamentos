using CA.Aplicacao.Models;
using CA.Identity.Entidades;
using CA.Identity.Models;

namespace CA.Identity.Extensions
{
    internal static class UsuarioIdentityExtensions
    {
        public static UsuarioIdentityModel UsuarioIdentityParaUsuarioIdentityModel(this UsuarioIdentity usuarioIdentity, string[] colecoes, string[] roles, bool ehAdministrador)
        {
            return new UsuarioIdentityModel
            {
                Id = new Guid(usuarioIdentity.Id),
                Email = usuarioIdentity.Email,
                NomeCompleto = usuarioIdentity.Usuario.NomeCompleto,
                IdUsuarioTfs = usuarioIdentity.Usuario.ParametrosIntegracoes.IdUsuarioTfs,
                DominioTfs = usuarioIdentity.Usuario.ParametrosIntegracoes.DominioTfs,
                NomeUsuarioTfs = usuarioIdentity.Usuario.ParametrosIntegracoes.NomeUsuarioTfs,
                TipoIdUsuarioTfs = usuarioIdentity.Usuario.ParametrosIntegracoes.TipoIdUsuarioTfs,
                IdUsuarioChannel = usuarioIdentity.Usuario.ParametrosIntegracoes.IdUsuarioChannel,
                NomeUsuarioChannel = usuarioIdentity.Usuario.ParametrosIntegracoes.NomeUsuarioChannel,
                IdFuncionarioPonto = usuarioIdentity.Usuario.ParametrosIntegracoes.IdFuncionarioPonto,
                PisFuncionarioPonto = usuarioIdentity.Usuario.ParametrosIntegracoes.PisFuncionarioPonto,
                PossuiContaTfs = !string.IsNullOrEmpty(usuarioIdentity.Usuario.ParametrosIntegracoes.IdUsuarioTfs),
                PossuiContaPonto = usuarioIdentity.Usuario.ParametrosIntegracoes.IdFuncionarioPonto.HasValue,
                PossuiContaChannel = usuarioIdentity.Usuario.ParametrosIntegracoes.IdUsuarioChannel.HasValue,                               
                EhAdministrador = ehAdministrador,                
                Colecoes = colecoes,
                Roles = roles,
                IdUnidade = !string.IsNullOrEmpty(usuarioIdentity.Usuario.IdUnidade) ? new Guid(usuarioIdentity.Usuario.IdUnidade) : null,
                Unidade = usuarioIdentity.Usuario.Unidade is not null ? new UnidadeModel
                {
                    Id = new Guid(usuarioIdentity.Usuario.Unidade.Id),
                    Nome = usuarioIdentity.Usuario.Unidade.Nome
                }
                : null,
                IdGerente = !string.IsNullOrEmpty(usuarioIdentity.Usuario.IdGerente) ? new Guid(usuarioIdentity.Usuario.IdGerente) : null,
                Gerente = usuarioIdentity.Usuario.Unidade is not null ? new GerenteModel
                {
                    Id = new Guid(usuarioIdentity.Usuario.Gerente.Id),
                    NomeCompleto = usuarioIdentity.Usuario.Gerente.NomeCompleto
                }
                : null
            };
        }

        public static IEnumerable<UsuarioIdentityModel> UsuarioIdentityParaUsuarioIdentityModel(this IEnumerable<UsuarioIdentity> usuarios, string[] colecoes, string[] roles, bool ehAdministrador)
        {
            return usuarios.Select(c => c.UsuarioIdentityParaUsuarioIdentityModel(colecoes, roles, ehAdministrador)).ToList();
        }
    }
}
