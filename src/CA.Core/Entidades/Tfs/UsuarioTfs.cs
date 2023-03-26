namespace CA.Core.Entidades.Tfs
{
    public class UsuarioTfs
    {
        public bool Ativo { get; set; }
        public string NomeCompleto { get; set; } = string.Empty;
        public string NomeUsuario { get; set; } = string.Empty;
        public string Dominio { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;        
        public IdentidadeTfs Identidade { get; set; }
        public DateTime UltimoAcesso { get; set; }

        public IEnumerable<string> Colecoes { get; set; }

        public UsuarioTfs()
        {
            Colecoes = new List<string>();
        }

        public bool PossuiAcessoColecao(string nome)
        {
            return Colecoes.Contains(nome);
        }
    }
}