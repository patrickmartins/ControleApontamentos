namespace CA.Aplicacao.Models
{
    public record ApontamentosMesModel
    {
        public int MesReferencia { get; set; }
        public int AnoReferencia { get; set; }
        public TimeSpan TempoTotalApontadoNoMes { get; set; }
        public TimeSpan TempoTotalApontadoSincronizadoChannel { get; set; }
        public TimeSpan TempoTotalApontadoNaoSincronizadoChannel { get; set; }
        public ICollection<ApontamentosDiaModel> ApontamentosDiarios { get; set; }

        public ApontamentosMesModel()
        {
            ApontamentosDiarios = new HashSet<ApontamentosDiaModel>();
        }
    }
}