using CA.Util.Helpers;
using System.Xml;
using System.Xml.Serialization;

namespace CA.FlurlSoap.Entidades
{
    [Serializable]
    public class SoapEnvelopeBody
    {
        [XmlAnyElement]
        public XmlElement Valor { get; set; }

        public void AdicionarValor(object valor)
        {
            Valor = XmlHelper.SerializarParaXmlElement(valor);
        }
    }
}
