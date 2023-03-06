using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace CA.Util.Extensions
{
    public static class StringExtensions
    {
        public static string Truncar(this string str, int max)
        {
            if(string.IsNullOrEmpty(str) || str.Length <= max) 
                return str;

            return str.Substring(0, max);
        }

        public static string CodificarHtml(this string str)
        {
            return WebUtility.HtmlEncode(str);
        }

        public static string DecodificarHtml(this string str)
        {
            return WebUtility.HtmlDecode(str);
        }

        public static string ParaUTF8(this string str)
        {
            var bytesOrigem = Encoding.Unicode.GetBytes(str);

            var bytesDestino = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, bytesOrigem);

            return Encoding.UTF8.GetString(bytesDestino);
        }

        public static string RemoverCaracteresNaoReconhecidos(this string str)
        {
            return Regex.Replace(str, "[^\x00-\x7F áàãâäéèêëíìîïóòõôöúùûüçÁÀÃÂÄÉÈÊËÍÌÎÏÓÒÕÖÔÚÙÛÜÇ´–‘’“”]+", string.Empty);
        }

        public static string RemoverQuebrasDeLinha(this string str)
        {
            return Regex.Replace(str, @"\n|\r|\r\n", " ");
        }

        public static string RemoverTabulacoes(this string str)
        {
            return Regex.Replace(str, @"\t", " ");
        }

        public static string RemoverAcentos(this string texto)
        {
            return new string(texto
                            .Normalize(NormalizationForm.FormD)
                            .Where(ch => char.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                            .ToArray());
        }

        public static string RemoverEspacosDuplicados(this string texto)
        {
            return texto.Replace("  ", " ");
        }
    }
}
