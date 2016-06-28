using Core.Model;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace InlineSearch.Model
{
    [Serializable, ImplementPropertyChanged, XmlRoot("config")]
    public class RootModel
    {
        [XmlArray("profiles"), XmlArrayItem("profile")]
        public ObservableCollection<Profile> Profiles { get; set; }


        [XmlArray("regexlist"), XmlArrayItem("regex")]
        public ObservableCollection<RegexItem> RegexList { get; set; }


        [XmlArray("addresses"), XmlArrayItem("address")]
        public ObservableCollection<Address> Addresses { get; set; }

    }
}
