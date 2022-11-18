using System.Xml;
using System.Xml.Serialization;

namespace CA.Repositorios.Tfs.ServicosSoap.Models.Requests
{
    [XmlRoot("QueryWorkitems", Namespace = "http://schemas.microsoft.com/TeamFoundation/2005/06/WorkItemTracking/ClientServices/03")]
    public class QueryWorkitemsRequest
    {
        public RequestHeader RequestHeader;

        public XmlElement? psQuery;

        public MetadataTableHaveEntry[]? metadataHave;

        public bool useMaster;

        public QueryWorkitemsRequest() { }
    }
}
