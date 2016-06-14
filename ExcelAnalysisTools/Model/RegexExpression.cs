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
    public class RegexExpression
    {
        [XmlAttribute("value")]
        public string Expression { get; set; }
        [XmlAttribute("order")]
        public int Order { get; set; }
    }
}
