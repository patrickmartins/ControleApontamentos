using CA.Core.Valores;

namespace CA.Core.Entidades.Channel
{
    public class UsuarioChannel
    {
        public int Id { get; set; }
        public string NomeUsuario { get; set; }
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public bool Ativo { get; set; }

        public IEnumerable<ApontamentoChannel> Apontamentos { get; set; }

        public Resultado Validar()
        {
            var erros = new List<Erro>();

            if (Id <= 0)
                erros.Add(new Erro("O id não foi informado.", nameof(Id)));

            if (string.IsNullOrEmpty(NomeUsuario))
                erros.Add(new Erro("O nome de usuário não foi informado.", nameof(NomeUsuario)));

            if (string.IsNullOrEmpty(NomeCompleto))
                erros.Add(new Erro("O nome completo do usuário não foi informado.", nameof(NomeCompleto)));

            if (NomeCompleto.Length > 200)
                erros.Add(new Erro("O nome completo do usuário contém mais de 200 caracteres.", nameof(NomeCompleto)));

            if (string.IsNullOrEmpty(Email))
                erros.Add(new Erro("O e-mail do usuário não foi informado.", nameof(Email)));

            if (Email.Length > 100)
                erros.Add(new Erro("O e-mail do usuário contém mais de 100 caracteres.", nameof(Email)));

            return Resultado.DeErros(erros);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null || GetType() != obj.GetType())
                return false;

            var usuario = (UsuarioChannel)obj;

            return usuario.Id == Id &&
                    usuario.NomeCompleto == NomeCompleto &&
                    usuario.NomeUsuario == NomeUsuario &&
                    usuario.Email == Email &&
                    usuario.Ativo == Ativo;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Id, NomeUsuario, NomeCompleto, Email, Ativo).GetHashCode();
        }

        public static bool operator ==(UsuarioChannel usuario1, UsuarioChannel usuario2)
        {
            if (usuario1 is null && usuario2 is null)
                return true;

            if ((usuario1 is null && usuario2 is not null) || (usuario1 is not null && usuario2 is null))
                return false;

            return usuario1.Equals(usuario2);
        }

        public static bool operator !=(UsuarioChannel usuario1, UsuarioChannel usuario2)
        {
            if (usuario1 is null && usuario2 is null)
                return true;

            if ((usuario1 is null && usuario2 is not null) || (usuario1 is not null && usuario2 is null))
                return false;

            return !usuario1.Equals(usuario2);
        }
    }
}
