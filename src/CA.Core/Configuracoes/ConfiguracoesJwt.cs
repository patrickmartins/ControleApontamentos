namespace CA.Core.Configuracoes
{
    public class ConfiguracoesJwt
    {
        public string Chave { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int TempoVidaToken { get; set; }

        public void Validar()
        {
            if (string.IsNullOrEmpty(Chave))
                throw new ArgumentException("A chave de criptografia do Token JWT não foi configurado.");

            if (string.IsNullOrEmpty(Issuer))
                throw new ArgumentException("A Issuer do Token JWT não foi configurado.");

            if (string.IsNullOrEmpty(Audience))
                throw new ArgumentException("A Audience do Token JWT não foi configurado.");

            if (TempoVidaToken <= 0)
                throw new ArgumentException("O tempo de vida do Token JWT deve ser maior que 0.");
        }
    }
}
