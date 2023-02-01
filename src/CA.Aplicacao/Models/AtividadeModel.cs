namespace CA.Aplicacao.Models
{
    public record AtividadeModel
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public int IdProjeto { get; set; }
        public string NomeProjeto { get; set; }
        public TimeSpan TempoTotalApontado { get; set; }

        public IEnumerable<ApontamentoModel> Apontamentos { get; set; }

        public AtividadeModel()
        {
            Apontamentos = new HashSet<ApontamentoModel>();
        }
    }
}