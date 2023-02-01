using CA.Servicos.Tfs.Entidades;

namespace CA.Repositorios.Tfs.Models
{
    internal class LinhaRegistrosTfs
    {
        public ICollection<CampoRegistroTfs> Campos { get; set; }

        public LinhaRegistrosTfs()
        {
            Campos = new HashSet<CampoRegistroTfs>();
        }

        public void AdicionarCampo(CampoRegistroTfs campo)
        {
            Campos.Add(campo);
        }

        public CampoRegistroTfs? ObterCampo(int indice)
        {
            return Campos.FirstOrDefault(c => c.IndiceColuna == indice);
        }

        public object? ObterValorCampo(int indice)
        {
            var campo = ObterCampo(indice);

            return campo is not null ? campo.Valor : null;
        }
    }
}
