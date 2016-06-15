using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExcelAnalysisTools.Model
{
    [ImplementPropertyChanged, Serializable, XmlRoot("profileList")]
    public class ProfileList
    {
        [XmlArray("profiles"), XmlArrayItem("profile")]
        public ObservableCollection<WorkSheetProfile> Items { get; set; } = new ObservableCollection<WorkSheetProfile>();


        public static ProfileList Create()
        {
            return new ProfileList
            {
                Items = new ObservableCollection<WorkSheetProfile>()
            };
        }
    }
}
