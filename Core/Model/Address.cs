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
    public class Address
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("district")]
        public string District { get; set; }

        [XmlAttribute("kgiopStatus")]
        public string KgiopStatus { get; set; }

        [XmlAttribute("uid")]
        public string Uid { get; set; }

        [XmlAttribute("description")]
        public string Description { get; set; }
    }
}
