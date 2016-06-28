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
    public class RegexItem
    {
        [XmlAttribute("uid")]
        public string Uid { get; set; } = Guid.NewGuid().ToString();
        [XmlAttribute("pattern")]
        public string Pattern { get; set; }
        [XmlAttribute("evaluator")]
        public string Evaluator { get; set; }
        [XmlAttribute("isenable")]
        public bool IsEnable { get; set; } = true;
        [XmlAttribute("isreplace")]
        public bool IsReplace { get; set; } = false;

   
    }
}
