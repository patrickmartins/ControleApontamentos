using System.Globalization;
using System.Text;

namespace CA.Seguranca.Extensions
{
    internal static class StringExtensions
    {
        public static string RemoverAcentos(this string texto)
        {
            return new string(texto
                            .Normalize(NormalizationForm.FormD)
                            .Where(ch => char.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                            .ToArray());
        }
    }
}