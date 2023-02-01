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

        public IEnumerable<AtividadeChannel> Atividades { get; set; }

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
            return projeto1.Equals(projeto2);
        }

        public static bool operator !=(ProjetoChannel projeto1, ProjetoChannel projeto2)
        {
            return !projeto1.Equals(projeto2);
        }
    }
}
