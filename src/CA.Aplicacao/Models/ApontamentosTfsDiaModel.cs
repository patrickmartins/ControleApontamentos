using CA.Core.Entidades.Tfs;

namespace CA.Aplicacao.Models
{
    public record ApontamentosTfsDiaModel
    {
        public string UsuarioReferencia { get; set; }
        public DateOnly DataReferencia { get; set; }        
        public TimeSpan TempoTotalApontadoNoDia { get; set; }
        public TimeSpan TempoTotalApontadoSincronizadoChannel { get; set; }
        public TimeSpan TempoTotalApontadoNaoSincronizadoChannel { get; set; }
        public IEnumerable<TarefaModel> Tarefas { get; set; }

        public ApontamentosTfsDiaModel()
        {
            Tarefas = new HashSet<TarefaModel>();
        }
    }
}