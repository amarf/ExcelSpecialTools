using Commander;
using Core.Interfaces;
using ExcelAnalysisTools.Model;
using ExcelAnalysisTools.Services;
using Microsoft.Practices.ServiceLocation;
using PropertyChanged;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ExcelAnalysisTools.ViewModel
{
    [ImplementPropertyChanged]
    public class AddressListViewModel
    {
        private readonly IServiceLocator _serviceLocator;
        private readonly IUserMsgService _userMsgService;
        private readonly Repository _repository;

        public ICollectionView Items { get; private set; }
        public string FindText { get; set; } = "";
        public string newAddress { get; set; }
        public string newDistrict { get; set; }


        private CollectionViewSource CVS;
        private ObservableCollection<AddressModel> items;

        public AddressListViewModel(IServiceLocator serviceLocator, IUserMsgService userMsgService, Repository repository)
        {
            _serviceLocator = serviceLocator;
            _userMsgService = userMsgService;
            _repository = repository;

            createView();

            (this as INotifyPropertyChanged).PropertyChanged += (obj, args) => { if (args.PropertyName == nameof(FindText)) Items?.Refresh(); };
            (repository as INotifyPropertyChanged).PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "AddressList")
                    createView();
            };
        }
        private void createView()
        {
            CVS = new CollectionViewSource();
            CVS.Source = items = _repository.AddressList.Items;
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

        [OnCommand("SaveListCommand")]
        private void SaveList()=> _repository.Save<AddressList>();


        [OnCommand("AddNewAddressCommand")]
        private void AddNewAddress()
        {
            var maxNumber = _repository.AddressList?.Items?.Max(i => i.Number);
            if(maxNumber!=null)
                _repository.AddressList.Items.Add(new AddressModel
                {
                    Number = (int)maxNumber + 1
                });
        }



        [OnCommand("ReloadAddressListCommand")]
        private void ReloadAddressList()
        {
            _repository.Load<AddressList>(_repository.Options.GetDataPath<AddressList>());
        }

        [OnCommand("RemoveAddressCommand")]
        private void RemoveAddressCommand(AddressModel item)
        {
            items?.Remove(item);
        }


    }
}
