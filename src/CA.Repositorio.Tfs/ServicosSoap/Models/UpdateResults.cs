using System.Xml.Serialization;

namespace CA.Repositorios.Tfs.ServicosSoap.Models
{
    public class UpdateResults
    {
        [XmlElement]
        public UpdateWorkItem UpdateWorkItem { get; set; }        

        public UpdateResults() { }
    }
}
