using System.Xml;
using System.Xml.Serialization;

namespace CA.Servicos.Tfs.Models
{
    public class ComputedColumn
    {
        [XmlAttribute(AttributeName = "Column")]
        public string ColumnName { get; set; }

        public ComputedColumn() { }

        public ComputedColumn(string columnName)
        {
            ColumnName = columnName;
        }
    }
}
