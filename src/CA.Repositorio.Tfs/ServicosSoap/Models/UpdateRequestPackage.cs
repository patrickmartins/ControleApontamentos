using System.Xml.Serialization;

namespace CA.Repositorios.Tfs.ServicosSoap.Models
{
    public class UpdateRequestPackage
    {
        [XmlElement(Namespace = "")]
        public PackageUpdateWorkItem Package { get; set; }
    }
}
