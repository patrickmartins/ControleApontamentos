using System.Xml;
using System.Xml.Serialization;

namespace CA.Servicos.Tfs.Models.Responses
{
    [XmlRoot("QueryWorkitemsResponse", Namespace = "http://schemas.microsoft.com/TeamFoundation/2005/06/WorkItemTracking/ClientServices/03")]
    public class QueryWorkitemsResponse
    {
        [XmlAnyElement]
        public XmlElement resultIds;

        public DateTime asOfDate;

        public string dbStamp;

        public ArrayOfXElement metadata;

        public QueryWorkitemsResponse()
        {
        }
    }
}
