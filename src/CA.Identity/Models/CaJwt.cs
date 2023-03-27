namespace CA.Identity.Models
{
    public class CaJwt
    {
        public string TokenAcesso { get; set; }
        public string TokenAtualizacao { get; set; }
        public int Validade { get; set; }
    }
}