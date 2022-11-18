using System.ComponentModel;

namespace CA.Core.Entidades.Tfs
{
    public enum StatusItemTrabalho
    {
        Desconhecido = 0,

        [Description("Proposed")]
        Proposto,

        [Description("Active")]
        Ativo,

        [Description("Resolved")]
        Resolvido,

        [Description("Closed")]
        Fechado     
    }

    public enum TipoItemTrabalho
    {
        NaoReconhecido = 0,
        Ad,
        Bug,
        Tarefa,
        Chamado,
        Requisito,
        HistoriaUsuario        
    }

    public class ItemTrabalho
    {
        public int IdItemTrabalho { get; set; }
        public int? IdItemTrabalhoPai { get; set; }
        public int Revisao { get; set; }
        public bool ApontamentoHabilitado { get; set; }
        public StatusItemTrabalho Status { get; set; }
        public TipoItemTrabalho Tipo { get; set; }
        public double EstimativaOriginal { get; set; }
        public double TempoRestante { get; set; }
        public double TempoConcluido { get; set; }
        public string Colecao { get; set; } = "";
        public string Designado { get; set; } = "";
        public string Titulo { get; set; } = "";
        public string TituloItemTrabalhoPai { get; set; } = "";
        public string Tags { get; set; } = "";
        public string IterationPath { get; set; } = "";
        public string Projeto { get; set; } = "";
        public DateTime DataCriacao { get; set; }

        public ListaApontamentos ListaApontamentos { get; set; }

        public ItemTrabalho()
        {
            ListaApontamentos = new ListaApontamentos();
        }
    }
}