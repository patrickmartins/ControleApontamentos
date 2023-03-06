using System.Xml.Serialization;

namespace CA.Servicos.Tfs.Models.Requests
{
    [XmlRoot("PageWorkitemsByIds", Namespace = "http://schemas.microsoft.com/TeamFoundation/2005/06/WorkItemTracking/ClientServices/03")]

    public class PageWorkitemsByIdsRequest
    {
        public RequestHeader RequestHeader;

        public int[] ids;

        public string[] columns;

        public int[] longTextColumns;

        [XmlElement(IsNullable = true)]
        public DateTime? asOfDate;

        public bool useMaster;

        [XmlArrayItem(IsNullable = false)]
        public MetadataTableHaveEntry[] metadataHave;

        public PageWorkitemsByIdsRequest() { }
    }
}
