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
using System.ComponentModel;

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

            (_repository as INotifyPropertyChanged).PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(Repository.ProfileList))
                    Data = _repository.ProfileList;
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

        [OnCommand("RemoveProfileCommand")]
        private void RemoveProfile(WorkSheetProfile profile)
        {
            Data?.Items?.Remove(profile);
            _repository.Save<ProfileList>();
        }


        [OnCommand("BackCommand")]
        private void Back(WorkSheetProfile profile)
        {
            EditData = null;
            SelectedItem = null;
            IsAutoCommandBegin = false;
            _excelApplication.SheetSelectionChange -= _excelApplication_SheetChange;
            _repository.Save<ProfileList>();
        }


        [OnCommand("ResetProfileItemCommand")]
        private void ResetProfileItem(WorkSheetProfileItem item)
        {
            item.Column = 0;
        }


        #region AutoCommand

        public bool IsAutoCommandBegin { get; set; }
        public WorkSheetProfileItem SelectedItem { get; set; }

        [OnCommand("AutoCommand")]
        private void Auto()
        {
            if (IsAutoCommandBegin)
                _excelApplication.SheetSelectionChange += _excelApplication_SheetChange;
            else
                _excelApplication.SheetSelectionChange -= _excelApplication_SheetChange;
        }

        private void _excelApplication_SheetChange(object Sh, Range Target)
        {
            if (SelectedItem != null)
            {
                SelectedItem.Column = Target.Column;
                var nextSelected = EditData.Items.IndexOf(SelectedItem) + 1;
                if (nextSelected < EditData.Items.Count)
                    SelectedItem = EditData.Items[nextSelected];
                else
                {
                    IsAutoCommandBegin = false;
                    Auto();
                }
            }
            else
            {
                SelectedItem = EditData.Items.FirstOrDefault();
                _excelApplication_SheetChange(Sh, Target);
            }
        }

        #endregion


        bool _isFirstDistrictCell;
        public bool IsFirstDistrictCell
        {
            get
            {
                return _isFirstDistrictCell;
            }
            set
            {
                _isFirstDistrictCell = value;
                if (value)
                    _excelApplication.SheetSelectionChange += _excelApplication_SheetSelectionChange;
                else
                    _excelApplication.SheetSelectionChange -= _excelApplication_SheetSelectionChange;
            }
        }

        private void _excelApplication_SheetSelectionChange(object Sh, Range Target)
        {
            if (EditData.FirstDistrictCell == null) EditData.FirstDistrictCell = new Cell();
            EditData.FirstDistrictCell.Address = Target.Address;
            EditData.FirstDistrictCell.Column = Target.Column;
            EditData.FirstDistrictCell.Row = Target.Row;
            IsFirstDistrictCell = false;
        }

        bool _isFirstAddressCell;
        public bool IsFirstAddressCell
        {
            get
            {
                return _isFirstAddressCell;
            }
            set
            {
                _isFirstAddressCell = value;
                if (value)
                    _excelApplication.SheetSelectionChange += _excelApplication_SheetSelectionChange2;
                else
                    _excelApplication.SheetSelectionChange -= _excelApplication_SheetSelectionChange2;
            }
        }

        private void _excelApplication_SheetSelectionChange2(object Sh, Range Target)
        {
            if (EditData.FirstAddressCell == null) EditData.FirstAddressCell = new Cell();

            EditData.FirstAddressCell.Address = Target.Address;
            EditData.FirstAddressCell.Column = Target.Column;
            EditData.FirstAddressCell.Row = Target.Row;
            IsFirstAddressCell = false;
        }

        bool _isLastAddressCell;
        public bool IsLastAddressCell
        {
            get
            {
                return _isLastAddressCell;
            }
            set
            {
                _isLastAddressCell = value;
                if (value)
                    _excelApplication.SheetSelectionChange += _excelApplication_SheetSelectionChange3;
                else
                    _excelApplication.SheetSelectionChange -= _excelApplication_SheetSelectionChange3;
            }
        }

        private void _excelApplication_SheetSelectionChange3(object Sh, Range Target)
        {
            if (EditData.LastAddressCell == null) EditData.LastAddressCell = new Cell();
            EditData.LastAddressCell.Address = Target.Address;
            EditData.LastAddressCell.Column = Target.Column;
            EditData.LastAddressCell.Row = Target.Row;
            IsLastAddressCell = false;
        }
    }
}
