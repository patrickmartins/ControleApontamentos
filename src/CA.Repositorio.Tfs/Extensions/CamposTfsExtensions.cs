using CA.Repositorios.Tfs.Entidades;

namespace CA.Repositorios.Tfs.Extensions
{
    internal static class CamposTfsExtensions
    {
        public static CampoTfs? ObterCampoPorId(this IEnumerable<CampoTfs> campos, int id)
        {
            return campos.FirstOrDefault(c => c.Id == id);
        }

        public static CampoTfs? ObterCampoPorNomeReferencia(this IEnumerable<CampoTfs> campos, string nomeReferencia)
        {
            return campos.FirstOrDefault(c => c.NomeReferencia.Equals(nomeReferencia));
        }
    }
}
