namespace CA.Core.Configuracoes
{
    public class ConfiguracaoCA
    {
        public static string SecaoConfig = "ConfiguracoesCA";

        public ConfiguracaoJwt Jwt { get; set; }
        public ConfiguracaoClienteTfs Tfs { get; set; }
        public ConfiguracaoClienteSecullum Secullum { get; set; }
        public ConfiguracaoClienteChannel Channel { get; set; }
        public ConfiguracaoMicrosoftGraph Graph { get; set; }
        public ConfiguracaoGerais Geral { get; set; }

    }
}
