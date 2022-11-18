namespace CA.Core.Entidades.Tfs
{
    public class Pagina<T>
    {
        public int TotalResultados { get; set; }
        public int NumeroPagina { get; set; }
        public int TamanhoPagina { get; set; }
        public IEnumerable<T> Resultados { get; set; }
        public string PalavraChave { get; set; } = string.Empty;

        public Pagina()
        {
            Resultados = new List<T>();
        }
    }
}