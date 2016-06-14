using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExcelAnalysisTools.Model
{
    [ImplementPropertyChanged, Serializable]
    public class AddressModel
    {
        [XmlAttribute("name")]
        public string Address { get; set; }
        [XmlAttribute("district")]
        public string District { get; set; }
        [XmlAttribute("uid")]
        public string Uid { get; set; } = Guid.NewGuid().ToString();

        [XmlIgnore]
        public string Regex { get; set; } 
    }
}
