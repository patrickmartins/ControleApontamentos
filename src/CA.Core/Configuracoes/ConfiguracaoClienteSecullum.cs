namespace CA.Core.Configuracoes
{
    public class ConfiguracaoClienteSecullum
    {
        public string UrlAutenticador { get; set; } = string.Empty;
        public string UrlIntegracao { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string IdCliente { get; set; } = string.Empty;
        public int TempoCacheFuncionarios { get; set; }

        public void Validar()
        {
            if (string.IsNullOrEmpty(UrlAutenticador))
                throw new ArgumentException("A url do autenticador do Secullum não foi configurada.");

            if (string.IsNullOrEmpty(UrlIntegracao))
                throw new ArgumentException("A url da integração externa do Secullum não foi configurada.");

            if (string.IsNullOrEmpty(Usuario))
                throw new ArgumentException("O usuário não foi configurado.");

            if (string.IsNullOrEmpty(Senha))
                throw new ArgumentException("A senha não foi configurada.");

            if (string.IsNullOrEmpty(IdCliente))
                throw new ArgumentException("O id do cliente não foi configurado.");

            if (TempoCacheFuncionarios <= 0)
                throw new ArgumentException("O tempo de cache de funcionários não foi informado.");
        }
    }
}
