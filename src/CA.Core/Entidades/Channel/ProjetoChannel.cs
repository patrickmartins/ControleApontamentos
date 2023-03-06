using CA.Core.Valores;

namespace CA.Core.Entidades.Channel
{
    public enum StatusProjeto
    {
        EmIniciacao = 0,
        EmExecução = 1,
        Finalizado = 2,
        Cancelado = 3,
        EmAprovacao = 4,
        Suspenso = 5,
        EmPlanejamento = 6,
        EmEncerramento = 8
    }

    public class ProjetoChannel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public StatusProjeto Status { get; set; }

        public ICollection<AtividadeChannel> Atividades { get; set; }
        public ICollection<ApontamentoChannel> Apontamentos { get; set; }

        public void AdicionarAtividade(AtividadeChannel atividade)
        {
            Atividades.Add(atividade);
        }

        public Resultado Validar()
        {
            var erros = new List<Erro>();

            if (string.IsNullOrEmpty(Nome))
                erros.Add(new Erro("O nome do projeto não foi informado.", nameof(Nome)));

            if (Nome.Length > 200)
                erros.Add(new Erro("O nome do projeto contém mais de 200 caracteres.", nameof(Nome)));

            return Resultado.DeErros(erros);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null || GetType() != obj.GetType())
                return false;

            var projeto = (ProjetoChannel)obj;

            if(ReferenceEquals(this, projeto)) 
                return true;

            return projeto.Id == Id &&
                    projeto.Nome == Nome &&
                    projeto.Status == Status;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Id, Nome, Status).GetHashCode();
        }

        public static bool operator ==(ProjetoChannel projeto1, ProjetoChannel projeto2)
        {
            if (projeto1 is null && projeto2 is null)
                return true;

            if ((projeto1 is null && projeto2 is not null) || (projeto1 is not null && projeto2 is null))
                return false;

            return projeto1.Equals(projeto2);
        }

        public static bool operator !=(ProjetoChannel projeto1, ProjetoChannel projeto2)
        {
            if (projeto1 is null && projeto2 is null)
                return false;

            if ((projeto1 is null && projeto2 is not null) || (projeto1 is not null && projeto2 is null))
                return true;

            return !projeto1.Equals(projeto2);
        }
    }
}
