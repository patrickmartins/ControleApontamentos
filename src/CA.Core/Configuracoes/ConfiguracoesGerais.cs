namespace CA.Core.Configuracoes
{
    public class ConfiguracoesGerais
    {
        public string DominioEmailPermitido { get; set; }

        public void Validar()
        {
            if (string.IsNullOrEmpty(DominioEmailPermitido))
                throw new ArgumentException("O dominio permitido para importação de usuários não foi configurada.");
        }
    }
}
