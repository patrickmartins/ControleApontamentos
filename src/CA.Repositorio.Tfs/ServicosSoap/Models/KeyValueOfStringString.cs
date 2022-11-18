using System.Xml.Serialization;

namespace CA.Repositorios.Tfs.ServicosSoap.Models
{
    public class KeyValueOfStringString
    {

        private string keyField;

        private string valueField;

        [XmlElement(Order = 0)]
        public string Key
        {
            get
            {
                return keyField;
            }
            set
            {
                keyField = value;
            }
        }

        [XmlElement(Order = 1)]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }
}
