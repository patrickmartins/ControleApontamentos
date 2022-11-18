using CA.FlurlSoap.Entidades;
using CA.Util.Helpers;
using Flurl;
using Flurl.Http;
using System.Net;

namespace CA.FlurlSoap
{
    public static class Entensions
    {
        public static IFlurlRequest WithWindowsAuth(this IFlurlRequest flurlRequest, string userName, string password, string? domain = "")
        {
            var client = (HttpClientHandler)flurlRequest.Client.HttpMessageHandler;

            if (client.Credentials == null)
                client.Credentials = new NetworkCredential(userName, password, domain);

            return flurlRequest;
        }

        public static IFlurlRequest WithWindowsAuth(this Url url, string userName, string password, string? domain = "")
        {
            return new FlurlRequest(url).WithWindowsAuth(userName, password, domain);
        }

        public static IFlurlRequest WithWindowsAuth(this string url, string userName, string password, string? domain = "")
        {
            return new FlurlRequest(url).WithWindowsAuth(userName, password, domain);
        }

        public static async Task<IFlurlResponse> SendXmlSoapAsync(this IFlurlRequest flurlRequest, string action, object body, object? header = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var envelope = new SoapEnvelope();

            envelope.Body.AdicionarValor(body);

            if(header != null)
                envelope.Header.AdicionarValor(header);

            var content = XmlHelper.SerializarParaString(envelope);

            return await flurlRequest
                            .WithHeader("Content-Type", "application/soap+xml; charset=utf-8;")
                            .WithHeader("SOAPAction", action)
                            .SendAsync(HttpMethod.Post, new StringContent(content), cancellationToken);
        }

        public static async Task<TResponse> ReceiveXmlSoapAsync<TResponse>(this Task<IFlurlResponse> flurlResponse) where TResponse : new()
        {
            using (var response = await flurlResponse.ConfigureAwait(false))
            {
                var stringResponse = await response.GetStringAsync();

                var envelope = XmlHelper.DesserializarDeString<SoapEnvelope>(stringResponse);

                envelope.Body.Valor.SetAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
                envelope.Body.Valor.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");

                return XmlHelper.DesserializarDeXmlElement<TResponse>(envelope.Body.Valor);
            }
        }
    }
}
