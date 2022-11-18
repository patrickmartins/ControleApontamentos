using System.Xml;
using System.Xml.Serialization;

namespace CA.Repositorios.Tfs.ServicosSoap.Models
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
