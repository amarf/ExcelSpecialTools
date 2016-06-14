using Commander;
using Core.Interfaces;
using ExcelAnalysisTools.Model;
using ExcelAnalysisTools.Services;
using ExcelAnalysisTools.View;
using ExcelDna.Integration.CustomUI;
using Microsoft.Practices.ServiceLocation;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelAnalysisTools.ViewModel
{
    [ImplementPropertyChanged]
    public class OptionsViewModel
    {
        
        private readonly IDataService _dataService;
        private readonly IServiceLocator _serviceLocator;
        private readonly IUserMsgService _userMsgService;

        public IOptionsService OptionsService { get; set; }

        public OptionsViewModel(IServiceLocator serviceLocator, IOptionsService optionsService, IDataService dataService, IUserMsgService userMsgService)
        {
            _serviceLocator = serviceLocator;
            _dataService = dataService;
            _userMsgService = userMsgService;
            OptionsService = optionsService;

            GetOptionsFile();
        
        }

        private void GetOptionsFile()
        {
            if (File.Exists(OptionsService.FullPath))
                LoadOptionFile(OptionsService.FullPath);
        }
        private void SaveOptionFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) return;
            try
            {
                _dataService.SerializeObject((OptionsService)OptionsService, OptionsService.FullPath);
            }
            catch (Exception e)
            {
                _userMsgService.MsgShow("Не удалось сохранить файл настроек");
            }
        }
        private void LoadOptionFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) return;
            try
            {
                var temp_OptionsService = _dataService.DeserializeObject<OptionsService>(filePath);
                OptionsService.AddressListPath = temp_OptionsService.AddressListPath;
                OptionsService.RegexListPath = temp_OptionsService.RegexListPath;
            }
            catch (Exception e)
            {
                _userMsgService.MsgShow("Не удалось загрузить файл настроек");
            }
        }
        private string GetFilePath(bool isSave = false)
        {
            var fd = _serviceLocator.GetInstance<IFileBrowserDialog>();
            fd.IsSaveFileDialog = isSave;

            if (fd.ShowDialog() && !string.IsNullOrWhiteSpace(fd.SelectedPath))
                return fd.SelectedPath;
            else
                return null;
        }


        [OnCommand("OpenAddressListCommand")]
        private void OpenAddressList()
        {
            OptionsService.AddressListPath = GetFilePath();
            SaveOptionFile(OptionsService.FullPath);
        }
        [OnCommand("OpenRegexListCommand")]
        private void OpenRegexList()
        {
            OptionsService.RegexListPath = GetFilePath();
            SaveOptionFile(OptionsService.FullPath);
        }
        [OnCommand("EditAddressListCommand")]
        private void EditAddressList()
        {
            var paneManager = _serviceLocator.GetInstance<IPaneManager<CustomTaskPane>>();
            var ctPane = paneManager.CreateCustomTaskPane<AddressListView, AddressListViewModel>("Адрессный список");
            ctPane.Width = 600;
            ctPane.Height = 450;
            ctPane.Visible = true;

            //var view = _serviceLocator.GetInstance<AddressListWindowView>();
            //var viewmodel = _serviceLocator.GetInstance<AddressListWindowViewModel>();
            //view.DataContext = viewmodel;
            //view.Show();
        }
        [OnCommand("EditRegexListCommand")]
        private void EditRegexList()
        {
        }
        [OnCommand("CreateAddressListCommand")]
        private void CreateAddressList()
        {
            var filePath = GetFilePath(true);
            if (string.IsNullOrWhiteSpace(filePath)) return;

            var newList = AddressList.Create();
            try
            {
                _dataService.SerializeObject(newList, filePath);
                OptionsService.AddressListPath = filePath;
                SaveOptionFile(OptionsService.FullPath);
            }
            catch (Exception e)
            {
                _userMsgService.MsgShow("Не удалось сохранить файл адресов");
            }
        }
        [OnCommand("CreateRegexListCommand")]
        private void CreateRegexList()
        {
            var filePath = GetFilePath(true);
            if (string.IsNullOrWhiteSpace(filePath)) return;
            var newList = RegexExpressionList.Create();
            try
            {
                _dataService.SerializeObject(newList, filePath);
                OptionsService.RegexListPath = filePath;
                SaveOptionFile(OptionsService.FullPath);
            }
            catch (Exception e)
            {
                _userMsgService.MsgShow("Не удалось сохранить файл выражений");
            }


        }
    }
}
