namespace CA.Core.Configuracoes
{
    public class ConfiguracaoMicrosoftGraph
    {
        public string ClientId { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
        public string Dominio { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;

        public void Validar()
        {
            if (string.IsNullOrEmpty(ClientId))
                throw new ArgumentException("O id do cliente do Microsoft Graph não foi configurado.");

            if (string.IsNullOrEmpty(TenantId))
                throw new ArgumentException("O tenant id do Microsoft Graph não foi configurado.");

            if (string.IsNullOrEmpty(Dominio))
                throw new ArgumentException("O domínio do Microsoft Graph não foi configurado.");

            if (string.IsNullOrEmpty(Usuario))
                throw new ArgumentException("O usuário do Microsoft Graph não foi configurado.");

            if (string.IsNullOrEmpty(Senha))
                throw new ArgumentException("A senha do Microsoft Graph não foi configurada.");
        }
    }
}
