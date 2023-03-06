using System.Xml.Serialization;

namespace CA.Servicos.Tfs.Models
{
    public partial class RequestHeader
    {

        private string idField;

        private int useDisambiguatedIdentityStringField;


        [XmlElement(Order = 0)]
        public string Id
        {
            get
            {
                return idField;
            }
            set
            {
                idField = value;
            }
        }

        [XmlElement(Order = 1)]
        public int UseDisambiguatedIdentityString
        {
            get
            {
                return useDisambiguatedIdentityStringField;
            }
            set
            {
                useDisambiguatedIdentityStringField = value;
            }
        }
    }
}
