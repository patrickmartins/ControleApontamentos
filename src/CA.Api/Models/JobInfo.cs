namespace CA.Api.Models
{
    public record JobInfo
    {
        public string Id { get; set; }
        public DateTime? UltimaExecucao { get; set; }
        public DateTime? ProximaExecucao { get; set; }
        public int IntervaloExecucao { get; set; }
    }
}
