using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExcelAnalysisTools.Model
{
    [ImplementPropertyChanged, Serializable, XmlRoot("addresses")]
    public class AddressList
    {
        [XmlElement("address", typeof(AddressModel))]
        public ObservableCollection<AddressModel> Items { get; set; }


        public static AddressList Create()
        {
            return new AddressList
            {
                Items = new ObservableCollection<AddressModel>()
            };
        }
    }
}
