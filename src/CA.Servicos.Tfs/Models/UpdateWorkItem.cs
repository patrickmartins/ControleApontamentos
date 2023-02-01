using System.Xml.Serialization;

namespace CA.Servicos.Tfs.Models
{
    public class UpdateWorkItem
    {
        [XmlAttribute]
        public string ObjectType { get; set; }

        [XmlAttribute]
        public int ClientCapabilities { get; set; }

        [XmlAttribute]
        public int WorkItemID { get; set; }

        [XmlAttribute]
        public int Revision { get; set; }

        [XmlElement(ElementName = "InsertText")]
        public UpdateField InsertText { get; set; }

        [XmlArray(ElementName = "Columns")]
        public List<Column> Columns { get; set; }

        [XmlArray(ElementName = "ComputedColumns")]
        public List<ComputedColumn> ComputedColumns { get; set; }

        public UpdateWorkItem()
        {
            Columns = new List<Column>();
            ComputedColumns = new List<ComputedColumn>();
        }

        public UpdateWorkItem(string objectType, int clientCapabilities, int workItemID, int revision) : base()
        {
            ObjectType = objectType;
            ClientCapabilities = clientCapabilities;
            WorkItemID = workItemID;
            Revision = revision;
        }
    }
}
