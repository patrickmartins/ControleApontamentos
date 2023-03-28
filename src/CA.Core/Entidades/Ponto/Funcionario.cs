namespace CA.Core.Entidades.Ponto
{
    public class Funcionario
    {
        public int Id { get; set; }
        public string NumeroPis { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public DateTime Admissao { get; set; }
        public DateTime? Demissao { get; set; }

        public bool FoiDemitido()
        {
            return Demissao.HasValue;
        }
    }
}