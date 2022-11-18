﻿using System.Xml.Serialization;

namespace CA.Repositorios.Tfs.ServicosSoap.Models.Responses
{
    [XmlRoot("PageWorkitemsByIdsResponse", Namespace = "http://schemas.microsoft.com/TeamFoundation/2005/06/WorkItemTracking/ClientServices/03")]
    public class PageWorkitemsByIdsResponse
    {
        public ArrayOfXElement items;

        public ArrayOfXElement metadata;

        public PageWorkitemsByIdsResponse()
        {
        }
    }
}
