using System.Xml.Serialization;

namespace CA.Servicos.Tfs.Models.Responses
{
    public class UpdateResponse
    {
        [XmlElement(ElementName = "result")]
        public ResultUpdateResponse Result { get; set; }

        public ArrayOfXElement metadata;
        public string dbStamp;
    }

    public class ResultUpdateResponse
    {
        [XmlElement(Namespace = "")]
        public UpdateResults UpdateResults { get; set; }
    }
}
