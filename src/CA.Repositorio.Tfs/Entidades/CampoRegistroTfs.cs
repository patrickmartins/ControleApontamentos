using CA.Repositorios.Tfs.Extensions;

namespace CA.Repositorios.Tfs.Entidades
{
    internal class CampoRegistroTfs
    {
        public object? Valor { get; set; }
        public int IndiceColuna { get; set; }

        public CampoRegistroTfs(object? valor, int indiceColuna)
        {           
            Valor = valor;
            IndiceColuna = indiceColuna;
        }
    }
}
