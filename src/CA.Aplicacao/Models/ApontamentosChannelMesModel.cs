namespace CA.Aplicacao.Models
{
    public record ApontamentosChannelMesModel
    {
        public int MesReferencia { get; set; }
        public int AnoReferencia { get; set; }
        public TimeSpan TempoTotalApontadoNoMes { get; set; }
        public IEnumerable<ApontamentosChannelDiaModel> ApontamentosDiarios { get; set; }

        public ApontamentosChannelMesModel()
        {
            ApontamentosDiarios = new HashSet<ApontamentosChannelDiaModel>();
        }
    }
}