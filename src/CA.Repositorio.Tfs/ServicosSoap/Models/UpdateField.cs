using System.Xml.Serialization;

namespace CA.Repositorios.Tfs.ServicosSoap.Models
{
    public class UpdateField
    {
        [XmlAttribute]
        public string FieldName { get; set; }

        [XmlText]
        public string Value { get; set; }

        public UpdateField() { }

        public UpdateField(string fieldName)
        {
            FieldName = fieldName;
        }
    }
}
