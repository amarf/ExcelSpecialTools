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
    public class Cell
    {
        [XmlAttribute("address")]
        public string Address { get; set; }
        [XmlAttribute("r")]
        public int Row { get; set; }
        [XmlAttribute("c")]
        public int Column { get; set; }
    }
}
