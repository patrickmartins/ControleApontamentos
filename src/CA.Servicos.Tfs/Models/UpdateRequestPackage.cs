using System.Xml.Serialization;

namespace CA.Servicos.Tfs.Models
{
    public class UpdateRequestPackage
    {
        [XmlElement(Namespace = "")]
        public PackageUpdateWorkItem Package { get; set; }
    }
}
