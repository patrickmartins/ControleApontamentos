using System.Xml.Serialization;

namespace CA.Servicos.Tfs.Models
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
