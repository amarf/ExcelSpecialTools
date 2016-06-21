using Core.Interfaces;
using ExcelAnalysisTools.Model;
using ExcelAnalysisTools.Services;
using ExcelDna.Integration;
using Microsoft.Office.Interop.Excel;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Commander;
using System.Text.RegularExpressions;

namespace ExcelAnalysisTools.ViewModel
{
    [ImplementPropertyChanged]
    public class AddressToAddressViewModel
    {
       
        private CollectionViewSource CVS;
        private CollectionViewSource CVS_found;

        private readonly Repository _repository;
        private readonly Application _excelApplication;

        public AddressToAddressViewModel(Repository repository)
        {
            _repository = repository;
            _excelApplication = (Application)ExcelDnaUtil.Application;

            createGlobalAddressesView();

            (this as INotifyPropertyChanged).PropertyChanged += (obj, args) => 
            {
                if (args.PropertyName == nameof(FindText))
                    Items?.Refresh();
                else if (args.PropertyName == nameof(SelectedNotFoundItem) && SelectedNotFoundItem != null)
                    FindText = getFindText(SelectedNotFoundItem.Address);
            };

            (repository as INotifyPropertyChanged).PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "AddressList")
                    createGlobalAddressesView();
            };
        }

        private string getFindText(string address)
        {
            var ret_val = "";

            var liter = Regex.Replace(address, @"(.*?)(литер[а]{0,1})\s+([0-1А-Яа-яA-Za-z,]+)", @"литер $3");
            var Befor_liter = Regex.Match(address, @".*(?=литер[а]{0,1}\s+)").Value;

            var mathes = Regex.Matches(Befor_liter, @"(\d+)|(\w{4,})");
            foreach (Match math in mathes)
                ret_val += math.Value + " ";
            ret_val = ret_val.Trim() + " " + liter.Trim();


            return ret_val;
        }

        private void createGlobalAddressesView()
        {
            CVS = new CollectionViewSource();
            CVS.Source = _repository.AddressList.Items;
            CVS.View.Filter = FilterMethod;
            Items = CVS.View;
        }
        private bool FilterMethod(object obj)
        {
            if (string.IsNullOrWhiteSpace(FindText)) return true;

            var findLine = (obj as AddressModel)?.Address?.ToLower();
            var mass = FindText.ToLower().Split(' ', ',', '.');
            foreach (var substring in mass)
                if (!findLine.Contains(substring)) return false;
            return true;
        }
        private bool FilterMethod_found(object obj)
        {
            return string.IsNullOrWhiteSpace((obj as AddressModel).Uid);
        }

        public AddressModel NotFoundSelectedItem { get; set; }
        public ICollectionView Items { get; private set; }
        public ICollectionView NotFoundItems { get; private set; }
        public string FindText { get; set; } = "";
        public WorkSheetProfile Profile { get; private set; }

        public AddressModel SelectedNotFoundItem { get; set; }

        public void AddFoundItems(IEnumerable<AddressModel> items, WorkSheetProfile profile)
        {
            Profile = profile;
            CVS_found = new CollectionViewSource();
            CVS_found.Source = items;
            CVS_found.View.Filter = FilterMethod_found;
            NotFoundItems = CVS_found.View;
        }
        public IList<AddressModel> GetFoundItems()
        {

            var list = (CVS_found.Source as IList<AddressModel>);
            foreach (var item in list.Where(i => !string.IsNullOrWhiteSpace(i.Uid)).ToList())
            {
                Guid guid;
                if(Guid.TryParse(item.Uid, out guid))
                    item.Number = BitConverter.ToInt32(guid.ToByteArray(), 0);
            }

            return list
                .Where(i => i.Number != 0 && !string.IsNullOrWhiteSpace(i.Uid))
                .OrderBy(i => i.Number)
                .ToList();
        }

        [OnCommand("SetUidToNotFoundItemCommand")]
        private void SetUidToNotFoundItem(AddressModel globalAddress)
        {
            SelectedNotFoundItem.Uid = globalAddress.Uid;
            NotFoundItems.Refresh();
        }

        [OnCommandCanExecute("SetUidToNotFoundItemCommand")]
        private bool SetUidToNotFoundItemCanExecute(AddressModel globalAddress) => SelectedNotFoundItem != null;
    }
}
