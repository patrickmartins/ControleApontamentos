using CA.Aplicacao.Models;
using CA.Core.Entidades.Channel;
using CA.Core.Entidades.Ponto;
using CA.Core.Entidades.Tfs;
using System.Text.Json.Serialization;

namespace CA.Identity.Models
{
    public class UsuarioIdentityModel
    {
        public Guid Id { get; set; }
        
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public string? NomeUsuarioTfs { get; set; }
        public string? IdUsuarioTfs { get; set; }
        public int? IdUsuarioChannel { get; set; }
        public int? IdFuncionarioPonto { get; set; }
        public bool PossuiContaPonto { get; set; }
        public bool PossuiContaTfs { get; set; }
        public bool PossuiContaChannel { get; set; }
        public bool EhAdministrador { get; set; }
        public string[] Colecoes { get; set; }
        public string[] Roles { get; set; }

        [JsonIgnore]
        public string? NomeUsuarioChannel { get; set; }

        [JsonIgnore]
        public string? DominioTfs { get; set; }

        [JsonIgnore]
        public string? TipoIdUsuarioTfs { get; set; }

        [JsonIgnore]
        public string? PisFuncionarioPonto { get; set; }

        public Guid? IdUnidade { get; set; }
        public UnidadeModel? Unidade { get; set; }

        public Guid? IdGerente { get; set; }
        public GerenteModel? Gerente { get; set; }

        public UsuarioTfs? ExtrairUsuarioTfs()
        {
            if (string.IsNullOrEmpty(IdUsuarioTfs))
                return null;

            return new UsuarioTfs
            {
                NomeUsuario = NomeUsuarioTfs,
                Dominio = DominioTfs,
                Colecoes = Colecoes,
                Identidade = new IdentidadeTfs
                {
                    Id = IdUsuarioTfs,
                    Tipo = TipoIdUsuarioTfs
                }
            };
        }

        public UsuarioChannel? ExtrairUsuarioChannel()
        {
            if (!IdUsuarioChannel.HasValue)
                return null;

            return new UsuarioChannel
            {
                Id = IdUsuarioChannel.Value,
                NomeUsuario = NomeUsuarioChannel
            };
        }

        public Funcionario? ExtrairFuncionarioPonto()
        {
            if (!IdFuncionarioPonto.HasValue)
                return null;

            return new Funcionario
            {
                Id = IdFuncionarioPonto.Value,
                NumeroPis = PisFuncionarioPonto
            };
        }
    }
}
