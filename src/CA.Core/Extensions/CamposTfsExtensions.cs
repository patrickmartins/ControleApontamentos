using CA.Core.Valores;

namespace CA.Core.Extensions
{
    public static class CamposTfsExtensions
    {
        public static bool CampoSuportado(this IEnumerable<CampoTfs> campos, string nomeCampo)
        {
            return campos.Any(c => c.NomeReferencia == nomeCampo);
        }
    }
}
