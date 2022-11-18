using System.Xml.Serialization;

namespace CA.Repositorios.Tfs.ServicosSoap.Models.Responses
{
    [Serializable]
    [XmlRoot("ReadIdentitiesResponse", Namespace = "http://microsoft.com/webservices/")]
    public partial class ReadIdentitiesResponse
    {
        [XmlArrayItem("ArrayOfTeamFoundationIdentity")]
        [XmlArrayItem(NestingLevel = 1)]
        public TeamFoundationIdentity[][] ReadIdentitiesResult;

        public ReadIdentitiesResponse() { }
    }
}
