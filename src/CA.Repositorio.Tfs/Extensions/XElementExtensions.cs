using System.Xml.Linq;

namespace CA.Repositorios.Tfs.Extensions
{
    internal static class XElementExtensions
    {
        internal static IEnumerable<XElement> BuscarElementosDescedentesPorTag(this XElement elemento, string tag)
        {
            return elemento.Descendants().Where(c => c.Name.LocalName.Equals(tag)).ToList();
        }
    }
}
