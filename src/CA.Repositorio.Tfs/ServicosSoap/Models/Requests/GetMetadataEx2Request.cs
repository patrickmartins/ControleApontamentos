using System.Xml.Serialization;

namespace CA.Repositorios.Tfs.ServicosSoap.Models.Requests
{
    [XmlRoot("GetMetadataEx2", Namespace = "http://schemas.microsoft.com/TeamFoundation/2005/06/WorkItemTracking/ClientServices/03")]

    public class GetMetadataEx2Request
    {
        public RequestHeader RequestHeader;

        [XmlArrayItem(IsNullable = false)]
        public MetadataTableHaveEntry[] metadataHave;

        public bool useMaster;

        public GetMetadataEx2Request()
        {
        }
    }
}
