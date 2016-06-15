using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExcelAnalysisTools.Model
{
    [ImplementPropertyChanged, Serializable, XmlRoot("profileItem")]
    public class WorkSheetProfileItem
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("columnNumber")]
        public int Column { get; set; }
    }
}
