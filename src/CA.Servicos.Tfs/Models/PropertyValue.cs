using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CA.Servicos.Tfs.Models
{
    public class PropertyValue
    {

        private object? valField;

        private string pnameField;

        [XmlElement(IsNullable = true, Order = 0)]
        public object? val
        {
            get
            {
                return valField;
            }
            set
            {
                valField = value;
            }
        }

        [XmlAttribute()]
        public string pname
        {
            get
            {
                return pnameField;
            }
            set
            {
                pnameField = value;
            }
        }
    }
}
