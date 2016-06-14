using Commander;
using Core.Interfaces;
using ExcelAnalysisTools.Model;
using Microsoft.Practices.ServiceLocation;
using PropertyChanged;
using System;
using System.Collections;
using System.Collections.Generic;
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
        private readonly IOptionsService _optionsService;
        private readonly IDataService _dataService;
        private readonly IServiceLocator _serviceLocator;
        private readonly IUserMsgService _userMsgService;

        public AddressList Data { get; private set; }
        public ICollectionView Items { get; private set; }
        public string FindText { get; set; } = "";
        private CollectionViewSource CVS;

        public AddressListViewModel(IServiceLocator serviceLocator, IOptionsService optionsService, IDataService dataService, IUserMsgService userMsgService)
        {
            _serviceLocator = serviceLocator;
            _optionsService = optionsService;
            _dataService = dataService;
            _userMsgService = userMsgService;

            LoadData();


            (this as INotifyPropertyChanged).PropertyChanged += (obj, args) => { if (args.PropertyName == nameof(FindText)) Items?.Refresh(); };
        }

        private void LoadData()
        {
            try
            {
                Data = _dataService.DeserializeObject<AddressList>(_optionsService.AddressListPath);
                CVS = new CollectionViewSource();
                CVS.Source = Data.Items;
                CVS.View.Filter = FilterMethod;
                Items = CVS.View;
            }
            catch (Exception e)
            {
                var errorMsg = string.IsNullOrWhiteSpace(e.InnerException?.Message) ? e.Message : e.InnerException?.Message;
                _userMsgService.MsgShow("Ошибка загрузки списка: " + errorMsg);
            }
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
        private void SaveList()
        {
            try
            {
                _dataService.SerializeObject(Data, _optionsService.AddressListPath);
            }
            catch (Exception e)
            {
                var errorMsg = string.IsNullOrWhiteSpace(e.InnerException?.Message) ? e.Message : e.InnerException?.Message;
                _userMsgService.MsgShow("Ошибка не удалось сохранить список: " + errorMsg);
            }
        }
    }
}
