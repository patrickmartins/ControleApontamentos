namespace CA.Aplicacao.Models
{
    public record RelatorioApontamentosUsuarioPorMes
    {
        public int MesReferencia { get; set; }
        public int AnoReferencia { get; set; }
        public TimeSpan DiferencaTempoTrabalhadoApontado { get; set; }
        public TimeSpan TempoTotalTrabalhadoNoMes { get; set; }
        public TimeSpan TempoTotalApontadoNoChannelNoMes { get; set; }
        public TimeSpan TempoTotalApontadoNaoSincronizadoNoTfsNoMes { get; set; }

        public UsuarioModel Usuario { get; set; }
    }
}
