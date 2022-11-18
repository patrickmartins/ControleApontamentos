using CA.Repositorios.Tfs.ServicosSoap.Models.Requests;
using System.Xml.Serialization;

namespace CA.Repositorios.Tfs.ServicosSoap.Models
{
    [XmlRoot(ElementName = "Package", Namespace = "http://schemas.microsoft.com/TeamFoundation/2005/06/WorkItemTracking/ClientServices/03")]
    public class PackageUpdateWorkItem
    {
        [XmlAttribute]
        public string AttachmentUrl
        {
            get
            {
                return !string.IsNullOrEmpty(Colecao) && !string.IsNullOrEmpty(ServidorTfs) ? $"{ServidorTfs}{Colecao}/WorkItemTracking/v1.0/AttachFileHandler.ashx" : string.Empty;
            }
            set
            {

            }
        }

        [XmlElement(Namespace = "")]
        public UpdateWorkItem UpdateWorkItem { get; set; }

        [XmlIgnore]
        public string Colecao { get; set; }

        [XmlIgnore]
        public string ServidorTfs { get; set; }

        public PackageUpdateWorkItem() { }

        public PackageUpdateWorkItem(string colecao, string servidorTfs)
        {
            Colecao = colecao;
            ServidorTfs = servidorTfs;
        }
    }

}
