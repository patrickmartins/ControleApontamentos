using System.Xml;
using System.Xml.Serialization;

namespace CA.Servicos.Tfs.Models.Requests
{
    [XmlRoot(ElementName = "Update", Namespace = "http://schemas.microsoft.com/TeamFoundation/2005/06/WorkItemTracking/ClientServices/03")]
    public class UpdateRequest
    {
        public RequestHeader RequestHeader;

        [XmlElement(ElementName = "package")]
        public UpdateRequestPackage Package { get; set; }

        public UpdateRequest() { }
    }
}
