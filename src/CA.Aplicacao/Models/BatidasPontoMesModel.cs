namespace CA.Aplicacao.Models
{
    public record BatidasPontoMesModel
    {
        public int MesReferencia { get; set; }
        public int AnoReferencia { get; set; }
        public TimeSpan TempoTotalTrabalhadoNoMes { get; set; }
        public IEnumerable<BatidasPontoDiaModel> BatidasDiarias { get; set; }

        public BatidasPontoMesModel()
        {
            BatidasDiarias = new HashSet<BatidasPontoDiaModel>();
        }
    }
}