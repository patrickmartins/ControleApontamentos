using CA.Core.Valores;
using System.Text.RegularExpressions;

namespace CA.Core.Entidades.CA
{
    public class UsuarioCA
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string NomeCompleto { get; set; }

        public string? IdUnidade { get; set; }
        public Unidade? Unidade { get; set; }

        public string? IdGerente { get; set; }
        public UsuarioCA? Gerente { get; set; }

        public ParametrosIntegracao ParametrosIntegracoes { get; set; }

        public UsuarioCA()
        {
            Id = Guid.NewGuid().ToString();    
        }

        public Resultado Validar()
        {
            var erros = new List<Erro>();

            if (string.IsNullOrEmpty(Id))
                erros.Add(new Erro("O id não foi informado.", nameof(NomeCompleto)));

            if (NomeCompleto.Length > 200)
                erros.Add(new Erro("O nome completo não pode conter mais de 200 caracteres.", nameof(NomeCompleto)));

            if (string.IsNullOrEmpty(Email))
                erros.Add(new Erro("O email não pode conter mais de 200 caracteres.", nameof(Email)));

            if (Email.Length > 60)
                erros.Add(new Erro("O email não pode conter mais de 200 caracteres.", nameof(Email)));

            if (!Regex.IsMatch(Email, "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$"))
                erros.Add(new Erro("O email está em um formato inválido.", nameof(Email)));

            return Resultado.DeErros(erros);
        }

        public bool PossuiIntegracaoChannel()
        {
            return ParametrosIntegracoes.IdUsuarioChannel.HasValue;
        }

        public bool PossuiIntegracaoTfs()
        {
            return !string.IsNullOrEmpty(ParametrosIntegracoes.IdUsuarioTfs);
        }

        public bool PossuiIntegracaoPonto()
        {
            return ParametrosIntegracoes.IdFuncionarioPonto.HasValue;
        }
    }
}
