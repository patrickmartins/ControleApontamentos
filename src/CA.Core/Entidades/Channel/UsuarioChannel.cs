using CA.Core.Valores;

namespace CA.Core.Entidades.Channel
{
    public class UsuarioChannel
    {
        public int Id { get; set; }
        public string NomeCompleto { get; set; }
        public string Email { get; set; }

        public IEnumerable<ApontamentoChannel> Apontamentos { get; set; }

        public Resultado Validar()
        {
            var erros = new List<Erro>();

            if (Id <= 0)
                erros.Add(new Erro("O id não foi informado.", nameof(Id)));

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
                    usuario.Email == Email;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Id, NomeCompleto, Email).GetHashCode();
        }

        public static bool operator ==(UsuarioChannel usuario1, UsuarioChannel usuario2)
        {
            return usuario1.Equals(usuario2);
        }

        public static bool operator !=(UsuarioChannel usuario1, UsuarioChannel usuario2)
        {
            return !usuario1.Equals(usuario2);
        }
    }
}
