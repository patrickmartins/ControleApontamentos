using System.Xml.Serialization;

namespace CA.Servicos.Tfs.Models
{
    public class UpdateResults
    {
        [XmlElement]
        public UpdateWorkItem UpdateWorkItem { get; set; }

        public UpdateResults() { }
    }
}
