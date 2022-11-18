using CA.Core.Entidades.Tfs;

namespace CA.Aplicacao.Models
{
    public record ApontamentosDiaModel
    {
        public DateOnly DataReferencia { get; set; }
        public TimeSpan TempoTotalApontadoNoDia { get; set; }
        public TimeSpan TempoTotalApontadoSincronizadoChannel { get; set; }
        public TimeSpan TempoTotalApontadoNaoSincronizadoChannel { get; set; }
        public ICollection<TarefaModel> Tarefas { get; set; }

        public ApontamentosDiaModel()
        {
            Tarefas = new HashSet<TarefaModel>();
        }
    }
}