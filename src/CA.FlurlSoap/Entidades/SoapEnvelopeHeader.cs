using CA.Util.Helpers;
using System.Xml;
using System.Xml.Serialization;

namespace CA.FlurlSoap.Entidades
{
    public class SoapEnvelopeHeader
    {
        [XmlAnyElement]
        public XmlElement[] Valores { get; set; }

        public SoapEnvelopeHeader()
        {
            Valores = new XmlElement[0];
        }

        public void AdicionarValor(object valor)
        {
            Valores = Valores.Concat(new XmlElement[] { XmlHelper.SerializarParaXmlElement(valor) }).ToArray();
        }
    }
}
