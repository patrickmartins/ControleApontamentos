using System.Xml.Serialization;

namespace CA.Servicos.Tfs.Models
{
    public class TeamFoundationIdentity
    {

        private IdentityDescriptor descriptorField;

        private KeyValueOfStringString[] attributesField;

        private PropertyValue[] propertiesField;

        private PropertyValue[] localPropertiesField;

        private IdentityDescriptor[] membersField;

        private IdentityDescriptor[] memberOfField;

        private string providerDisplayNameField;

        private string customDisplayNameField;

        private string displayNameField;

        private bool isContainerField;

        private bool isActiveField;

        private Guid teamFoundationIdField;

        private string uniqueNameField;

        private int uniqueUserIdField;

        [XmlElement(Order = 0)]
        public IdentityDescriptor Descriptor
        {
            get
            {
                return descriptorField;
            }
            set
            {
                descriptorField = value;
            }
        }

        [XmlArray(Order = 1)]
        public KeyValueOfStringString[] Attributes
        {
            get
            {
                return attributesField;
            }
            set
            {
                attributesField = value;
            }
        }

        [XmlArray(Order = 2)]
        public PropertyValue[] Properties
        {
            get
            {
                return propertiesField;
            }
            set
            {
                propertiesField = value;
            }
        }

        [XmlArray(Order = 3)]
        public PropertyValue[] LocalProperties
        {
            get
            {
                return localPropertiesField;
            }
            set
            {
                localPropertiesField = value;
            }
        }

        [XmlArray(Order = 4)]
        public IdentityDescriptor[] Members
        {
            get
            {
                return membersField;
            }
            set
            {
                membersField = value;
            }
        }

        [XmlArray(Order = 5)]
        public IdentityDescriptor[] MemberOf
        {
            get
            {
                return memberOfField;
            }
            set
            {
                memberOfField = value;
            }
        }

        [XmlElement(Order = 6)]
        public string ProviderDisplayName
        {
            get
            {
                return providerDisplayNameField;
            }
            set
            {
                providerDisplayNameField = value;
            }
        }

        [XmlElement(Order = 7)]
        public string CustomDisplayName
        {
            get
            {
                return customDisplayNameField;
            }
            set
            {
                customDisplayNameField = value;
            }
        }

        [XmlAttribute()]
        public string DisplayName
        {
            get
            {
                return displayNameField;
            }
            set
            {
                displayNameField = value;
            }
        }

        [XmlAttribute()]
        public bool IsContainer
        {
            get
            {
                return isContainerField;
            }
            set
            {
                isContainerField = value;
            }
        }

        [XmlAttribute()]
        public bool IsActive
        {
            get
            {
                return isActiveField;
            }
            set
            {
                isActiveField = value;
            }
        }

        [XmlAttribute()]
        public Guid TeamFoundationId
        {
            get
            {
                return teamFoundationIdField;
            }
            set
            {
                teamFoundationIdField = value;
            }
        }

        [XmlAttribute()]
        public string UniqueName
        {
            get
            {
                return uniqueNameField;
            }
            set
            {
                uniqueNameField = value;
            }
        }

        [XmlAttribute()]
        public int UniqueUserId
        {
            get
            {
                return uniqueUserIdField;
            }
            set
            {
                uniqueUserIdField = value;
            }
        }
    }

}
