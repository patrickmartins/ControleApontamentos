namespace CA.Core.Configuracoes
{
    public class ConfiguracaoCA
    {
        public static string SecaoConfig = "ConfiguracoesCA";

        public ConfiguracoesJwt Jwt { get; set; }
        public ConfiguracaoClienteTfs Tfs { get; set; }
        public ConfiguracaoClienteSecullum Secullum { get; set; }
        public ConfiguracoesGerais Geral { get; set; }

    }
}
