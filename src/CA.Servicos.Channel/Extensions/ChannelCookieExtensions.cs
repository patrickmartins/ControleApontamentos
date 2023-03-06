using CA.Servicos.Channel.Models;
using Flurl.Http;

namespace CA.Servicos.Channel.Extensions
{
    internal static class ChannelCookieExtensions
    {
        public static CookieJar ParaCookieJar(this IEnumerable<ChannelCookie> cookie)
        {
            var cookieJar = new CookieJar();

            foreach (var c in cookie)
            {
                cookieJar.AddOrReplace(c.Nome, c.Valor, c.UrlOrigem);
            }

            return cookieJar;
        }
    }
}
