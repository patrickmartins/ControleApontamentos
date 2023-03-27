namespace CA.Aplicacao.Models
{
    public record ApontamentosTfsMesModel
    {
        public string UsuarioReferencia { get; set; }
        public int MesReferencia { get; set; }
        public int AnoReferencia { get; set; }
        public int DiasApontados { get; set; }
        public TimeSpan TempoTotalApontadoNoMes { get; set; }
        public TimeSpan TempoTotalApontadoSincronizadoChannel { get; set; }
        public TimeSpan TempoTotalApontadoNaoSincronizadoChannel { get; set; }
        public IEnumerable<ApontamentosTfsDiaModel> ApontamentosDiarios { get; set; }

        public ApontamentosTfsMesModel()
        {
            ApontamentosDiarios = new HashSet<ApontamentosTfsDiaModel>();
        }
    }
}