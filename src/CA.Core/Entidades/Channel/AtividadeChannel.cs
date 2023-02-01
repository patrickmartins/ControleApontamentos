using CA.Core.Valores;

namespace CA.Core.Entidades.Channel
{
    public class AtividadeChannel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Codigo { get; set; }
        public ProjetoChannel Projeto { get; set; }

        public IEnumerable<ApontamentoChannel> Apontamentos { get; set; }

        public Resultado Validar()
        {
            var erros = new List<Erro>();

            if (Projeto is null)
                erros.Add(new Erro("O projeto não foi informado.", nameof(Projeto)));

            if (string.IsNullOrEmpty(Nome))
                erros.Add(new Erro("O nome da atividade não foi informado.", nameof(Nome)));

            if (Nome.Length > 200)
                erros.Add(new Erro("O nome da atividade contém mais de 200 caracteres.", nameof(Nome)));

            if (string.IsNullOrEmpty(Codigo))
                erros.Add(new Erro("O código da atividade não foi informado.", nameof(Codigo)));

            if (Codigo.Length > 30)
                erros.Add(new Erro("O código da atividade contém mais de 30 caracteres.", nameof(Codigo)));

            return Resultado.DeErros(erros);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null || GetType() != obj.GetType())
                return false;

            var atividade = (AtividadeChannel)obj;

            return atividade.Id == Id &&
                    atividade.Codigo == Codigo &&
                    atividade.Nome == Nome;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Id, Codigo, Nome).GetHashCode();
        }

        public static bool operator ==(AtividadeChannel atividade1, AtividadeChannel atividade2)
        {
            return atividade1.Equals(atividade2);
        }

        public static bool operator !=(AtividadeChannel atividade1, AtividadeChannel atividade2)
        {
            return !atividade1.Equals(atividade2);
        }
    }
}
