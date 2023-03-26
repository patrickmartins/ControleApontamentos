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

        public void OcultarBatidas()
        {
            Entrada1 = TimeSpan.Zero;            
            Saida1 = TimeSpan.Zero;
            Entrada2 = TimeSpan.Zero;
            Saida2 = TimeSpan.Zero;
            Entrada3 = TimeSpan.Zero;
            Saida3 = TimeSpan.Zero;
            Entrada4 = TimeSpan.Zero;
            Saida4 = TimeSpan.Zero;
            Entrada5 = TimeSpan.Zero;
            Saida5 = TimeSpan.Zero;
            Entrada6 = TimeSpan.Zero;
            Saida6 = TimeSpan.Zero;
        }

    }
}