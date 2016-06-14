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
    public class RegexReplaceExpression: RegexExpression
    {
        [XmlAttribute("replaceValue")]
        public string ReplceExpression { get; set; }
    }
}
