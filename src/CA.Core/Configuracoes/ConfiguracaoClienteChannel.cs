namespace CA.Core.Configuracoes
{
    public class ConfiguracaoClienteChannel
    {
        public string UrlCloud { get; set; } = string.Empty;
        public string UrlBase { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;

        public int TempoCacheAtividades { get; set; }
        public int TempoCacheProjetos { get; set; }
        public int TempoCacheApontamentos { get; set; }
        public int TempoCacheUsuarios{ get; set; }

        public void Validar()
        {
            if (string.IsNullOrEmpty(UrlCloud))
                throw new ArgumentException("A url de autenticação do Channel não foi configurada.");

            if (string.IsNullOrEmpty(Usuario))
                throw new ArgumentException("O usuário não foi configurado.");

            if (string.IsNullOrEmpty(Senha))
                throw new ArgumentException("A senha não foi configurada.");

            if (string.IsNullOrEmpty(UrlBase))
                throw new ArgumentException("A base do Channel não foi configurada.");

            if (TempoCacheAtividades <= 0)
                throw new ArgumentException("O tempo de cache de atividades não foi informado.");

            if (TempoCacheProjetos <= 0)
                throw new ArgumentException("O tempo de cache de projetos não foi informado.");

            if (TempoCacheApontamentos <= 0)
                throw new ArgumentException("O tempo de cache de apontamentos não foi informado.");

            if (TempoCacheUsuarios <= 0)
                throw new ArgumentException("O tempo de cache de usuários não foi informado.");
        }
    }
}
