namespace CA.Aplicacao.Models
{
    public record BatidasPontoDiaModel
    {
        public TimeSpan Entrada1 { get; set; }
        public TimeSpan Saida1 { get; set; }
        public TimeSpan Entrada2 { get; set; }
        public TimeSpan Saida2 { get; set; }
        public TimeSpan Entrada3 { get; set; }
        public TimeSpan Saida3 { get; set; }
        public TimeSpan Entrada4 { get; set; }
        public TimeSpan Saida4 { get; set; }
        public TimeSpan Entrada5 { get; set; }
        public TimeSpan Saida5 { get; set; }
        public TimeSpan Entrada6 { get; set; }
        public TimeSpan Saida6 { get; set; }
        public DateTime DataReferencia { get; set; }
        public TimeSpan TempoTotalTrabalhadoNoDia { get; set; }

    }
}