using System.Xml.Linq;
using System.Xml.Serialization;

namespace CA.Servicos.Tfs.Models.Responses
{
    [XmlRoot("GetMetadataEx2Response", Namespace = "http://schemas.microsoft.com/TeamFoundation/2005/06/WorkItemTracking/ClientServices/03")]
    public class GetMetadataEx2Response
    {
        [XmlAnyElement]
        public XElement metadata;

        public string dbStamp;

        public int locale;

        public int comparisonStyle;

        public int mode;

        public GetMetadataEx2Response() { }
    }
}
