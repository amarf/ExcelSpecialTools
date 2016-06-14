using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExcelAnalysisTools.Model
{
    [ImplementPropertyChanged, Serializable, XmlRoot("regexes")]
    public class RegexExpressionList
    {
        [XmlElement("replaceExpression", typeof(RegexReplaceExpression))]
        public ObservableCollection<RegexReplaceExpression> Items { get; set; }


        public static RegexExpressionList Create()
        {
            return new RegexExpressionList
            {
                Items = new ObservableCollection<RegexReplaceExpression>()
            };
        }
    }
}
