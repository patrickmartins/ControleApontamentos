namespace CA.Repositorios.Tfs.Models
{
    internal class ColunaRegistroTfs
    {
        public int Indice { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }

        public ColunaRegistroTfs(int indice, string nome, string tipo)
        {
            Nome = nome ?? throw new ArgumentNullException(nameof(nome));
            Tipo = tipo ?? throw new ArgumentNullException(nameof(tipo));

            Indice = indice;
        }
    }
}
