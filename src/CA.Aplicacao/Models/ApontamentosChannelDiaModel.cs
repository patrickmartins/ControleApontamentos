namespace CA.Aplicacao.Models
{
    public record ApontamentosChannelDiaModel
    {
        public DateOnly DataReferencia { get; set; }
        public TimeSpan TempoTotalApontadoNoDia { get; set; }
        public IEnumerable<AtividadeModel> Atividades { get; set; }

        public ApontamentosChannelDiaModel()
        {
            Atividades = new HashSet<AtividadeModel>();
        }
    }
}