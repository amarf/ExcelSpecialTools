using ExcelAnalysisTools.Model;
using ExcelAnalysisTools.ViewModel.vmServices;
using Microsoft.Practices.ServiceLocation;
using Prism.Events;
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
        private readonly IEventAggregator _eventAggregator;

        public NotFoundViewModel(IServiceLocator serviceLocator, IEventAggregator eventAggregator)
        {
            _serviceLocator = serviceLocator;
            _eventAggregator = eventAggregator;
        }


        public ObservableCollection<AddressToAddressViewModel> TabItems { get; private set; } = new ObservableCollection<AddressToAddressViewModel>();

        public void AddItem(WorkObject workObject)
        {
            var mtm = _serviceLocator.GetInstance<AddressToAddressViewModel>();
            mtm.AddFoundItems(workObject.Addresses, workObject.Profile);
            TabItems.Add(mtm);


            if (TabItems.Count == 1) SelectedItem = mtm;
        }

        public void RemoveItem(AddressToAddressViewModel item) =>TabItems.Remove(item);
        public void RemoveAllItems() => TabItems.Clear();


        public object SelectedItem { get; set; }
    }

}
