using System.Xml.Serialization;

namespace CA.Servicos.Tfs.Models
{
    public class IdentityDescriptor
    {

        private string identityTypeField;

        private string identifierField;

        [XmlAttribute()]
        public string identityType
        {
            get
            {
                return identityTypeField;
            }
            set
            {
                identityTypeField = value;
            }
        }

        [XmlAttribute()]
        public string identifier
        {
            get
            {
                return identifierField;
            }
            set
            {
                identifierField = value;
            }
        }
    }
}
