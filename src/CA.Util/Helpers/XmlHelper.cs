using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CA.Util.Helpers
{
    public static class XmlHelper
    {
        public static T DesserializarDeString<T>(string valor) where T : new()
        {
            if (string.IsNullOrEmpty(valor))
                return new T();

            var xmlserializer = new XmlSerializer(typeof(T));

            using (var stringWriter = new StringReader(valor))
            {
                return (T)xmlserializer.Deserialize(stringWriter);
            }
        }

        public static T DesserializarDeXmlElement<T>(XmlElement element) where T : new()
        {
            return DesserializarDeString<T>(element.OuterXml);
        }

        public static string SerializarParaString<T>(T objeto)
        {
            if (objeto == null)
            {
                return string.Empty;
            }
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    using (var writer = XmlWriter.Create(stringWriter, new XmlWriterSettings
                    {
                        OmitXmlDeclaration = true,
                        Indent = false,
                        NamespaceHandling = NamespaceHandling.OmitDuplicates
                    }))
                    {
                        var xmlSerializerNamespaces = new XmlSerializerNamespaces();
                        xmlSerializerNamespaces.Add("", "");

                        new XmlSerializer(typeof(T)).Serialize(writer, objeto, xmlSerializerNamespaces);

                        return stringWriter.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }

        public static XmlElement? SerializarParaXmlElement<T>(T objeto)
        {
            if (objeto == null)
                return null;

            XmlDocument doc = new XmlDocument();

            using (XmlWriter writer = doc.CreateNavigator().AppendChild())
            {
                new XmlSerializer(objeto.GetType()).Serialize(writer, objeto);
            }

            return doc.DocumentElement;
        }
    }
}