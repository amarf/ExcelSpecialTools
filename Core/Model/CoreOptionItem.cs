using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Core.Model
{
    [ImplementPropertyChanged, Serializable]
    public class CoreOptionItem
    {
        [XmlAttribute("key")]
        public string Key { get; set; }
        [XmlAttribute("value")]
        public string Value { get; set; }
        [XmlAttribute("associativeType")]
        public string AssociativeType { get; set; }

        //[XmlElement("associativeType")]
        //public Type AssociativeType { get; set; }
    }
}
