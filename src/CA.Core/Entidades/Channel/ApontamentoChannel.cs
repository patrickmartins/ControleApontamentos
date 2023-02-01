using CA.Core.Valores;
using System.Numerics;

namespace CA.Core.Entidades.Channel
{
    public class ApontamentoChannel
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string Comentario { get; set; }
        public TimeSpan TempoApontado { get; set; }
        public UsuarioChannel Usuario { get; set; }
        public AtividadeChannel Atividade { get; set; }

        public Resultado Validar()
        {
            var erros = new List<Erro>();

            if (Id <= 0)
                erros.Add(new Erro("O id não foi informado.", nameof(Id)));

            if (Usuario == null)
                erros.Add(new Erro("O usuário não foi informado.", nameof(Usuario)));

            if (Atividade is null)
                erros.Add(new Erro("Atividade não foi informada.", nameof(Atividade)));

            if (Comentario.Length > 1000)
                erros.Add(new Erro("O comentário contém mais de 1000 caracteres.", nameof(Comentario)));

            if (TempoApontado.Ticks == 0)
                erros.Add(new Erro("O tempo apontado não foi informado.", nameof(TempoApontado)));

            return Resultado.DeErros(erros);
        }

        public override bool Equals(object? obj)
        {            
            if (obj is null || GetType() != obj.GetType()) 
                return false;

            var apontamento = (ApontamentoChannel)obj;

            return apontamento.Id == Id &&
                    apontamento.Data == Data &&
                    apontamento.Comentario == Comentario &&
                    apontamento.TempoApontado.Ticks == TempoApontado.Ticks &&
                    apontamento.Atividade.Id == Atividade.Id &&
                    apontamento.Usuario.Id == Usuario.Id;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Id, Data, Comentario, TempoApontado, Atividade.Id, Usuario.Id).GetHashCode();
        }

        public static bool operator ==(ApontamentoChannel apontamento1, ApontamentoChannel apontamento2)
        {
            return apontamento1.Equals(apontamento2);
        }

        public static bool operator !=(ApontamentoChannel apontamento1, ApontamentoChannel apontamento2)
        {
            return !apontamento1.Equals(apontamento2);
        }
    }
}