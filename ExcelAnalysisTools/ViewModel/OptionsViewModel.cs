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
        public Options Data { get; private set; }

        private readonly Repository _repository;
        private readonly IServiceLocator _serviceLocator;
        private readonly IUserMsgService _userMsgService;


        public OptionsViewModel(IServiceLocator serviceLocator, Repository repository, IUserMsgService userMsgService)
        {
            _serviceLocator = serviceLocator;
            _repository = repository;
            _userMsgService = userMsgService;

            Data = _repository.Options;
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
            _repository.Options.AddressListPath = GetFilePath();
            _repository.Save<Options>();
        }
        [OnCommand("OpenRegexListCommand")]
        private void OpenRegexList()
        {
            _repository.Options.RegexListPath = GetFilePath();
            _repository.Save<Options>();
        }
        [OnCommand("EditAddressListCommand")]
        private void EditAddressList()
        {
            var paneManager = _serviceLocator.GetInstance<IPaneManager<CustomTaskPane>>();
            var ctPane = paneManager.CreateCustomTaskPane<AddressListView, AddressListViewModel>("Адрессный список");
            ctPane.Width = 600;
            ctPane.Height = 450;
            ctPane.Visible = true;
        }
        [OnCommand("EditRegexListCommand")]
        private void EditRegexList()
        {
        }
        [OnCommand("CreateAddressListCommand")]
        private void CreateAddressList()
        {
            //var filePath = GetFilePath(true);
            //if (string.IsNullOrWhiteSpace(filePath)) return;

            //var newList = AddressList.Create();
            //try
            //{
            //    _dataService.SerializeObject(newList, filePath);
            //    OptionsService.AddressListPath = filePath;
            //    SaveOptionFile(OptionsService.OptionsFileFullPath);
            //}
            //catch (Exception e)
            //{
            //    _userMsgService.MsgShow("Не удалось сохранить файл адресов");
            //}
        }
        [OnCommand("CreateRegexListCommand")]
        private void CreateRegexList()
        {
            //var filePath = GetFilePath(true);
            //if (string.IsNullOrWhiteSpace(filePath)) return;
            //var newList = RegexExpressionList.Create();
            //try
            //{
            //    _dataService.SerializeObject(newList, filePath);
            //    OptionsService.RegexListPath = filePath;
            //    SaveOptionFile(OptionsService.OptionsFileFullPath);
            //}
            //catch (Exception e)
            //{
            //    _userMsgService.MsgShow("Не удалось сохранить файл выражений");
            //}


        }
    }
}
