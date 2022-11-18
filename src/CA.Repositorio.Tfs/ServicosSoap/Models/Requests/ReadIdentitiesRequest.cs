using System.Xml.Serialization;

namespace CA.Repositorios.Tfs.ServicosSoap.Models.Requests
{
    [XmlRoot("ReadIdentities", Namespace = "http://microsoft.com/webservices/")]
    public class ReadIdentitiesRequest
    {
        public int searchFactor;

        public string[] factorValues;

        public int queryMembership;

        public int options;

        public int features;

        public string[]? propertyNameFilters;

        public int propertyScope;

        public ReadIdentitiesRequest()
        {
        }

        public ReadIdentitiesRequest(int searchFactor, string[] factorValues, int queryMembership, int options, int features, string[]? propertyNameFilters, int propertyScope)
        {
            this.searchFactor = searchFactor;
            this.factorValues = factorValues;
            this.queryMembership = queryMembership;
            this.options = options;
            this.features = features;
            this.propertyNameFilters = propertyNameFilters;
            this.propertyScope = propertyScope;
        }
    }
}
