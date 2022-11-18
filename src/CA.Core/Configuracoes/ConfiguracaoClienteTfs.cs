using CA.Core.Entidades.Tfs;

namespace CA.Core.Configuracoes
{
    public class ConfiguracaoClienteTfs
    {
        public string UrlTfs { get; set; } = string.Empty;
        public string UrlServicoColecao { get; set; } = string.Empty;
        public string UrlServicoItemTrabalho { get; set; } = string.Empty;
        public string UrlServicoIdentidade { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string Dominio { get; set; } = string.Empty;
        public int TempoCacheColecoes { get; set; }
        public int TempoCacheMetadados { get; set; }

        public void Validar()
        {
            if (string.IsNullOrEmpty(UrlServicoColecao))
                throw new ArgumentException("O endpoint do serviço de coleção do TFS não foi informado.");

            if (string.IsNullOrEmpty(UrlServicoItemTrabalho))
                throw new ArgumentException("O endpoint do serviço de itens de trabalho do TFS não foi informado.");

            if (string.IsNullOrEmpty(UrlServicoIdentidade))
                throw new ArgumentException("O endpoint do serviço de identidade do TFS não foi informado.");

            if (string.IsNullOrEmpty(Usuario))
                throw new ArgumentException("O usuário não foi configurado.");

            if (string.IsNullOrEmpty(Senha))
                throw new ArgumentException("A senha não foi configurada.");

            if (string.IsNullOrEmpty(Dominio))
                throw new ArgumentException("O dominio não foi configurado.");

            if (TempoCacheColecoes <= 0)
                throw new ArgumentException("O tempo de cache das coleções do TFS não foi configurado.");

            if (TempoCacheMetadados <= 0)
                throw new ArgumentException("O tempo de cache de metadados do TFS não foi configurado.");
        }
    }
}
