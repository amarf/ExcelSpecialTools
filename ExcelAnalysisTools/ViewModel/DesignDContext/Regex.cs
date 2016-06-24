using ExcelAnalysisTools.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelAnalysisTools.ViewModel.DesignDContext
{
    public class Regex
    {
        public ObservableCollection<RegexReplaceExpression> Items { get; set; }


        public Regex()
        {
            Items = new ObservableCollection<RegexReplaceExpression>
            {

                new RegexReplaceExpression {Expression = "asdsad ", ReplceExpression = "cool" , Order = 2 },
                new RegexReplaceExpression {Expression = "asdsad ", ReplceExpression = "cool" , Order = 2 },
                new RegexReplaceExpression {Expression = "asdsad ", ReplceExpression = "cool" , Order = 2 },
                new RegexReplaceExpression {Expression = "asdsad ", ReplceExpression = "cool" , Order = 2 },
                new RegexReplaceExpression {Expression = "asdsad ", ReplceExpression = "cool" , Order = 2 },
                new RegexReplaceExpression {Expression = "asdsad ", ReplceExpression = "cool" , Order = 2 },

            };
        }
    }
}
