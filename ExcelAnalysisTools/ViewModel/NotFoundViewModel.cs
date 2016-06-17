using ExcelAnalysisTools.Model;
using Microsoft.Practices.ServiceLocation;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelAnalysisTools.ViewModel
{
    [ImplementPropertyChanged]
    public class NotFoundViewModel
    {
        private readonly IServiceLocator _serviceLocator;

        public NotFoundViewModel(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }


        public ObservableCollection<AddressToAddressViewModel> TabItems { get; private set; } = new ObservableCollection<AddressToAddressViewModel>();

        public void AddItem(ObservableCollection<AddressModel> list, WorkSheetProfile profile)
        {
            var mtm = _serviceLocator.GetInstance<AddressToAddressViewModel>();
            mtm.AddFoundItems(list, profile);
            TabItems.Add(mtm);


            if (TabItems.Count == 1) SelectedItem = mtm;
        }

        public void RemoveItem(AddressToAddressViewModel item) =>TabItems.Remove(item);
        public void RemoveAllItems() => TabItems.Clear();


        public object SelectedItem { get; set; }
    }

}
