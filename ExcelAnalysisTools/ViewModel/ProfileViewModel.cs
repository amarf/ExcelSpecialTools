using Commander;
using ExcelAnalysisTools.Services;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelAnalysisTools.Model;
using PropertyChanged;
using ExcelDna.Integration;
using Microsoft.Office.Interop.Excel;

namespace ExcelAnalysisTools.ViewModel
{
    [ImplementPropertyChanged]
    public class ProfileViewModel
    {
        public ProfileList Data { get; private set; }
        public WorkSheetProfile EditData { get; private set; }
        public string ProfileListPath { get { return _repository.Options.ProfileListPath; } }

        private readonly Repository _repository;
        private readonly IFileBrowserDialog _fileBrowserDialog;
        private readonly Application _excelApplication;

        public ProfileViewModel(Repository repository, IFileBrowserDialog fileBrowserDialog)
        {
            _repository = repository;
            _fileBrowserDialog = fileBrowserDialog;
            _excelApplication = (Application)ExcelDnaUtil.Application;
            Data = _repository.ProfileList;
        }

        [OnCommand("CreateProfileListCommand")]
        private void CreateProfileList()
        {
            _fileBrowserDialog.IsSaveFileDialog = true;
            _fileBrowserDialog.Reset();
            if (_fileBrowserDialog.ShowDialog())
            {
                Data = _repository.Create<ProfileList>(_fileBrowserDialog.SelectedPath);
            };
        }

        [OnCommand("LoadProfileListCommand")]
        private void LoadProfileList()
        {
            _fileBrowserDialog.IsSaveFileDialog = false;
            _fileBrowserDialog.Reset();
            if (_fileBrowserDialog.ShowDialog())
            {
                Data = _repository.Load<ProfileList>(_fileBrowserDialog.SelectedPath);
            };
        }
        [OnCommand("NewProfileCommand")]
        private void NewProfile()
        {
            Data.Items.Add(WorkSheetProfile.Create(_excelApplication.ActiveSheet.Name));
            _repository.Save<ProfileList>();
        }
        [OnCommandCanExecute("NewProfileCommand")]
        private bool NewProfileCanExecute()
        {
            return Data != null;
        }


        [OnCommand("EditProfileCommand")]
        private void EditProfile(WorkSheetProfile profile)
        {
            EditData = profile;
        }
    }
}
