using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace CA.Servicos.Channel.Helpers
{
    internal static class ChannelStringResponseHelper
    { 
        public static IEnumerable<TType> DesserializarResultado<TType>(string resultado) where TType : class
        {
            var lista = ExtrairLista(resultado);
            var json = SanitizarResultado(lista);

            return Desserializar<TType>(json);
        }

        private static IEnumerable<TType> Desserializar<TType>(string json) where TType : class
        {
            return JsonConvert.DeserializeObject<IEnumerable<TType>>(json);
        }

        private static string ExtrairLista(string resultado)
        {
            var match = Regex.Match(resultado, "(?<=\\[{)(.*)(?=\\}])");

            return match.Success ? $"[{{{match.Value}}}]" : "[]";
        }

        private static string SanitizarResultado(string resultado)
        {
            var resultadoSanitizado = Regex.Replace(resultado, ":s\\d+,", ":null,");

            resultadoSanitizado = Regex.Replace(resultadoSanitizado, ":new Date\\(\\d+\\),", ":null,");
            resultadoSanitizado = Regex.Replace(resultadoSanitizado, ",s\\d+,", ",");

            return resultadoSanitizado;
        }
    }
}
