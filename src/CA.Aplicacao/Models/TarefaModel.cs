using CA.Core.Entidades.Tfs;

namespace CA.Aplicacao.Models
{
    public record TarefaModel
    {
        public int Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public TipoItemTrabalho Tipo { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string TituloPai { get; set; }
        public StatusItemTrabalho Status { get; set; }
        public string Colecao { get; set; }
        public string Designado { get; set; } = string.Empty;
        public string Projeto { get; set; } = string.Empty;
        public string[] Tags { get; set; } = new string[0];
        public bool ApontamentoHabilitado { get; set; }        
        public TimeSpan TempoTotalApontadoSincronizadoChannel { get; set; }
        public TimeSpan TempoTotalApontadoNaoSincronizadoChannel { get; set; }

        public ICollection<ApontamentoModel> Apontamentos { get; set; }

        public TarefaModel()
        {
            Apontamentos = new HashSet<ApontamentoModel>();
        }
    }
}