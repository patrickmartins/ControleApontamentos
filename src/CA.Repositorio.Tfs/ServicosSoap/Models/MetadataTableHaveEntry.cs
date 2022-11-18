using System.Xml.Serialization;

namespace CA.Repositorios.Tfs.ServicosSoap.Models
{
    public class MetadataTableHaveEntry
    {

        private string tableNameField;

        private long rowVersionField;

        [XmlElement(Order = 0)]
        public string TableName
        {
            get
            {
                return tableNameField;
            }
            set
            {
                tableNameField = value;
            }
        }

        [XmlElement(Order = 1)]
        public long RowVersion
        {
            get
            {
                return rowVersionField;
            }
            set
            {
                rowVersionField = value;
            }
        }
    }
}
