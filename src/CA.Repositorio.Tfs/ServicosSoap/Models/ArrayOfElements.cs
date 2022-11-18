using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace CA.Repositorios.Tfs.ServicosSoap.Models
{
    public partial class ArrayOfXElement : object, IXmlSerializable
    {

        private List<XElement> nodesList = new List<XElement>();

        public ArrayOfXElement()
        {
        }

        public virtual List<XElement> Nodes
        {
            get
            {
                return nodesList;
            }
        }

        public virtual XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public virtual void WriteXml(System.Xml.XmlWriter writer)
        {
            IEnumerator<XElement> e = nodesList.GetEnumerator();

            for ( ; e.MoveNext(); )
            {
                ((IXmlSerializable)(e.Current)).WriteXml(writer);
            }
        }

        public virtual void ReadXml(System.Xml.XmlReader reader)
        {
            for ( ; reader.NodeType != System.Xml.XmlNodeType.EndElement; )
            {
                if ((reader.NodeType == System.Xml.XmlNodeType.Element))
                {
                    XElement elem = new XElement("default");
                    ((IXmlSerializable)(elem)).ReadXml(reader);
                    Nodes.Add(elem);
                }
                else
                {
                    reader.Skip();
                }
            }
        }
    }

}
