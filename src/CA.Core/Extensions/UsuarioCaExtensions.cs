using CA.Core.Entidades.CA;
using CA.Core.Entidades.Channel;
using CA.Core.Entidades.Ponto;
using CA.Core.Entidades.Tfs;

namespace CA.Core.Extensions
{
    public static class UsuarioCaExtensions
    {
        public static UsuarioTfs? ExtrairUsuarioTfs(this UsuarioCA usuarioCa)
        {
            if(string.IsNullOrEmpty(usuarioCa.ParametrosIntegracoes.IdUsuarioTfs))
                return null;

            return new UsuarioTfs
            {
                NomeUsuario = usuarioCa.ParametrosIntegracoes.NomeUsuarioTfs,
                Dominio = usuarioCa.ParametrosIntegracoes.DominioTfs,
                Identidade = new IdentidadeTfs
                {
                    Id = usuarioCa.ParametrosIntegracoes.IdUsuarioTfs,
                    Tipo = usuarioCa.ParametrosIntegracoes.TipoIdUsuarioTfs
                }
            };
        }

        public static UsuarioChannel? ExtrairUsuarioChannel(this UsuarioCA usuarioCa)
        {
            if (!usuarioCa.ParametrosIntegracoes.IdUsuarioChannel.HasValue)
                return null;

            return new UsuarioChannel
            {
                Id = usuarioCa.ParametrosIntegracoes.IdUsuarioChannel.Value,
                NomeUsuario = usuarioCa.ParametrosIntegracoes.NomeUsuarioChannel
            };
        }

        public static Funcionario? ExtrairFuncionarioPonto(this UsuarioCA usuarioCa)
        {
            if (!usuarioCa.ParametrosIntegracoes.IdFuncionarioPonto.HasValue)
                return null;

            return new Funcionario
            {
                Id = usuarioCa.ParametrosIntegracoes.IdFuncionarioPonto.Value,
                NumeroPis = usuarioCa.ParametrosIntegracoes.PisFuncionarioPonto
            };
        }
    }
}
