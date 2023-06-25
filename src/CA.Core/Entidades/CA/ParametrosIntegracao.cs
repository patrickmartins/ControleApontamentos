namespace CA.Core.Entidades.CA
{
    public class ParametrosIntegracao
    {
        public string? NomeUsuarioTfs { get; set; }
        public string? DominioTfs { get; set; }
        public string? IdUsuarioTfs { get; set; }
        public string? TipoIdUsuarioTfs { get; set; }
        public int? IdFuncionarioPonto { get; set; }
        public string? PisFuncionarioPonto { get; set; }
        public int? IdUsuarioChannel { get; set; }
        public string? NomeUsuarioChannel { get; set; }

        public string IdUsuario { get; set; }
        public UsuarioCA Usuario { get; set; }
    }
}