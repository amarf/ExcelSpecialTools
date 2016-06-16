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
        private readonly IFileBrowserDialog _fileBrowserDialog;



        public OptionsViewModel(IServiceLocator serviceLocator, Repository repository, IUserMsgService userMsgService, IFileBrowserDialog fileBrowserDialog)
        {
            _serviceLocator = serviceLocator;
            _repository = repository;
            _userMsgService = userMsgService;
            _fileBrowserDialog = fileBrowserDialog;

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
            var path = GetFilePath();
            if (!string.IsNullOrWhiteSpace(path))
            {
                _repository.Options.AddressListPath = path;
                _repository.Save<Options>();
            }
        }
        [OnCommand("OpenRegexListCommand")]
        private void OpenRegexList()
        {
            var path = GetFilePath();
            if (!string.IsNullOrWhiteSpace(path))
            {
                _repository.Options.RegexListPath = path;
                _repository.Save<Options>();
            }
        }


        CustomTaskPane addressEditorPane;
        [OnCommand("EditAddressListCommand")]
        private void EditAddressList()
        {
            if (addressEditorPane == null)
            {
                var paneManager = _serviceLocator.GetInstance<IPaneManager<CustomTaskPane>>();
                addressEditorPane = paneManager.CreateCustomTaskPane<AddressListView, AddressListViewModel>("Адрессный список");
                addressEditorPane.DockPosition = MsoCTPDockPosition.msoCTPDockPositionFloating;
                addressEditorPane.Width = 600;
                addressEditorPane.Height = 450;
                addressEditorPane.Visible = true;
            }
            else
            {
                addressEditorPane.Visible = true;
            }
        }

        CustomTaskPane regexEditorPane;
        [OnCommand("EditRegexListCommand")]
        private void EditRegexList()
        {
            if (regexEditorPane == null)
            {
                var paneManager = _serviceLocator.GetInstance<IPaneManager<CustomTaskPane>>();
                regexEditorPane = paneManager.CreateCustomTaskPane<RegexListView, RegexListViewModel>("Регулярные выражения");
                regexEditorPane.DockPosition = MsoCTPDockPosition.msoCTPDockPositionFloating;
                regexEditorPane.Width = 600;
                regexEditorPane.Height = 450;
                regexEditorPane.Visible = true;
            }
            else
            {
                regexEditorPane.Visible = true;
            }
        }
        [OnCommand("CreateAddressListCommand")]
        private void CreateAddressList()
        {
            _fileBrowserDialog.IsSaveFileDialog = true;
            _fileBrowserDialog.Reset();
            if (_fileBrowserDialog.ShowDialog())
                _repository.Create<AddressList>(_fileBrowserDialog.SelectedPath);
        }
        [OnCommand("CreateRegexListCommand")]
        private void CreateRegexList()
        {
            _fileBrowserDialog.IsSaveFileDialog = true;
            _fileBrowserDialog.Reset();
            if (_fileBrowserDialog.ShowDialog())
                _repository.Create<RegexExpressionList>(_fileBrowserDialog.SelectedPath);
        }
    }
}
