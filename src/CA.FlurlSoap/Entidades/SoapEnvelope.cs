using System.Xml.Serialization;

namespace CA.FlurlSoap.Entidades
{
    [Serializable]
    [XmlRoot("Envelope", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
    public class SoapEnvelope
    {
        [XmlElement("Header")]
        public SoapEnvelopeHeader Header { get; set; }

        [XmlElement("Body")]
        public SoapEnvelopeBody Body { get; set; }

        public SoapEnvelope()
        {
            Header = new SoapEnvelopeHeader();
            Body = new SoapEnvelopeBody();
        }
    }
}
