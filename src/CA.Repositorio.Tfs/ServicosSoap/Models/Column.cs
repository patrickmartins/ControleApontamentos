using System.Xml.Serialization;

namespace CA.Repositorios.Tfs.ServicosSoap.Models
{
    public class Column
    {
        [XmlAttribute(AttributeName = "Column")]
        public string ColumnName { get; set; }

        [XmlAttribute]
        public string Type { get; set; }

        [XmlElement]
        public object Value { get; set; }

        public Column() { }

        public Column(string columnName, string type, object value)
        {
            ColumnName = columnName;
            Type = type;
            Value = value;  
        }
    }

}
