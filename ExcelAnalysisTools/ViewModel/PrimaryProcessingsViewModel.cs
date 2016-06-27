using Commander;
using Core.Interfaces;
using ExcelAnalysisTools.Model;
using ExcelAnalysisTools.Services;
using ExcelAnalysisTools.View;
using ExcelAnalysisTools.WfHosts;
using ExcelDna.Integration;
using ExcelDna.Integration.CustomUI;
using Microsoft.Office.Interop.Excel;
using Microsoft.Practices.ServiceLocation;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections;
using ExcelAnalysisTools.ViewModel.vmServices;


namespace ExcelAnalysisTools.ViewModel
{
    [ImplementPropertyChanged]
    public class PrimaryProcessingsViewModel : IDisposable
    {
        #region ctor

        private readonly Application _excelApplication;
        private readonly Repository _repository;
        private readonly IUserMsgService _userMsgService;
        private readonly IServiceLocator _serviceLocator;

        public PrimaryProcessingsViewModel(Repository repository, IUserMsgService userMsgService, IServiceLocator serviceLocator)
        {
            _repository = repository;
            _userMsgService = userMsgService;
            _serviceLocator = serviceLocator;

            _excelApplication = (Application)ExcelDnaUtil.Application;
            _excelApplication.SheetSelectionChange += _excelApplication_SheetSelectionChange;
        }
        public void Dispose()
        {
            _excelApplication.SheetSelectionChange += _excelApplication_SheetSelectionChange;
        }
        private void _excelApplication_SheetSelectionChange(object Sh, Range Target)
        {
            if (IsSelectDistrictColumn && !IsSelectAddressColumn)
            {
                Column_DistrictNumber = Target.EntireColumn.Column;
                Row_DistrictStartNumber = Target.EntireRow.Row;
            }
            else if (IsSelectAddressColumn && !IsSelectDistrictColumn)
            {
                Column_AddressNumber = Target.EntireColumn.Column;
                Row_AddressStartNumber = Target.EntireRow.Row;
            }
            else if (!IsSelectAddressColumn && !IsSelectDistrictColumn)
            { }
            else throw new ArgumentException();
        }

        #endregion

        #region props

        public bool IsSelectDistrictColumn { get; set; }
        public bool IsSelectAddressColumn { get; set; }
        public int Column_DistrictNumber { get; set; } = 1;
        public int Column_AddressNumber { get; set; } = 2;

        public int Row_DistrictStartNumber { get; set; } = 13;
        public int Row_AddressStartNumber { get; set; } = 14;

        //public string DistrictReplace { get; set; } = " район Санкт-Петербурга";
        //public string DistrictKeyWord { get; set; } = " район ";

        public bool IsFirstComplite { get; set; }

        public bool IsTwoProfileCompare { get; set; } = false;

        #endregion

        #region Commands

        //[OnCommand("SelectDistrictColumnCommand")]
        //public void SelectDistrictColumn(bool isCheked)
        //{
        //    if (isCheked)
        //    {
        //        IsSelectAddressColumn = false;
        //        IsSelectDistrictColumn = true;
        //    }
        //    else
        //        IsSelectDistrictColumn = false;
        //}

        //[OnCommand("SelectAddressColumnCommand")]
        //public void SelectAddressColumn(bool isCheked)
        //{
        //    if (isCheked)
        //    {
        //        IsSelectAddressColumn = true;
        //        IsSelectDistrictColumn = false;
        //    }
        //    else
        //        IsSelectAddressColumn = false;
        //}

       

        [OnCommand("ShowFirstResultCommand")]
        public void ShowFirstResult() => OpenNotFoundPane();
        [OnCommandCanExecute("ShowFirstResultCommand")]
        public bool ShowFirstResultCanExecute() => IsFirstComplite;



        public bool Start_1_Procces { get; set; }
        IList<WorkObject> tempList;
        [OnCommand("FirstMarcosCommand")]
        private async void StartDistrictMarcos()
        {
            IsFirstComplite = false;

            var class1 = _serviceLocator.GetInstance<Class1>();


            Start_1_Procces = true;
            var data = tempList = await Task.Run(() =>
            {
                return class1.CollectData();
            });
            Start_1_Procces = false;

            if (data == null)
            {
                _userMsgService.MsgShow("Нет активных профилей для выполнения анализа");
                return;
            }

            //отчет
            foreach (var item in data)
                if (item.Profile.IsPrintResult)
                {
                    string sheetName = item.Profile.ProfileName + "_res";
                    WriteArray(CreateResultData(item), sheetName, true);
                }




            ClearNotFoundPane();
            var vmOpenPan = OpenNotFoundPane();

            foreach (var wObj in data)
                vmOpenPan.AddItem(wObj);

            IsFirstComplite = true;

        }

        [OnCommand("SecontMarcosCommand")]
        public void StartDistrictMarcos2()
        {
            var class2 = _serviceLocator.GetInstance<Class2>();
            var table = class2.GoWork(tempList, IsTwoProfileCompare);
            object[,] result = new object[table.Rows.Count + 1, table.Columns.Count];


            for (int i = 0; i < table.Rows.Count; i++)
                for (int j = 0; j < table.Columns.Count; j++)
                    if (i == 0)
                        result[i, j] = table.Columns[j].ColumnName;
                    else
                        result[i, j] = table.Rows[i][j];

            WriteArray(result, $"ОТЧЕТ");
        }

        [OnCommandCanExecute("SecontMarcosCommand")]
        public bool StartDistrictMarcos2CanExecute() => GetViewModelNotFoundPane() != null && IsFirstComplite;


        /// <summary>
        /// Конвертирует объект в массив данных
        /// </summary>
        private object[,] CreateResultData(WorkObject wObj)
        {
            object[,] result = null;

               var count = wObj?.Profile?.Items?.Where(i => i.Column > 0).Count();
            if (count != null && count > 0 && wObj?.Addresses != null)
            {
                var columns = (int)count + 4;
                var rows = wObj.Addresses.Count * (int)count;
                result = new object[rows, columns];
                var realRow = 0;

                for (int i = 0; i < wObj.Addresses.Count; i++)
                {
                    var adr = wObj.Addresses[i];
                    var tbl = adr.GetDataTable();
                    foreach (DictionaryEntry dk in tbl)
                    {
                        
                        result[realRow, 0] = adr.District;
                        result[realRow, 1] = adr.Address;
                        result[realRow, 2] = adr.Uid;
                        result[realRow, 3] = adr.Description;
                        result[realRow, 4] = adr.Regex;
                        result[realRow, 5] = adr.Number;
                        result[realRow, 5] = dk.Key + "";
                        result[realRow, 6] = dk.Value + "";
                        realRow++;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Записывает объект данных на новый созданный лист начиная с ячейки А1, с проверкой имени листа
        /// </summary>
        private void WriteArray(object[,] cells, string newWorksheetName, bool IsOnlyString = false)
        {
            var newName = newWorksheetName;
            var newNameIndex = 0;
            if (newName.Length > 25) newName = newName.Substring(0, 25);
            while (true)
            {
                back:

                newName = $"{newName}" + (newNameIndex > 0 ? $"({newNameIndex})" : "");
                foreach (Worksheet sheet in _excelApplication.Worksheets)
                    if (sheet.Name == newName)
                    {
                        newNameIndex++;
                        goto back;
                    }
                break;
            }

            _excelApplication.Worksheets.Add(After: _excelApplication.Worksheets[_excelApplication.Worksheets.Count]).Name = newName;
            var worksheet = _excelApplication.Worksheets[newName] as Worksheet;

            var rows = cells.GetLongLength(0);
            var columns = cells.GetLongLength(1);

            var startCell = (Range)worksheet.Cells[1, 1];
            var endCell = (Range)worksheet.Cells[rows, columns];
            var writeRange = worksheet.Range[startCell, endCell];
            if(IsOnlyString) writeRange.NumberFormat = "@";//текстовый
            writeRange.Value2 = cells;
        }



        #endregion


        CustomTaskPane notFoundPane;
        private NotFoundViewModel OpenNotFoundPane()
        {
            if (notFoundPane != null)
            {
                notFoundPane.Visible = true;
            }
            else
            {
                var paneManager = _serviceLocator.GetInstance<IPaneManager<CustomTaskPane>>();
                notFoundPane = paneManager.CreateCustomTaskPane<NotFoundView, NotFoundViewModel>("Панель поиска");
                notFoundPane.DockPosition = MsoCTPDockPosition.msoCTPDockPositionLeft;
                notFoundPane.Width = 800;
                notFoundPane.Visible = true;
            }

            return GetViewModelNotFoundPane();
        }
        private void ClearNotFoundPane()
        {
            var vm = GetViewModelNotFoundPane();
            if(vm!=null)
                vm.RemoveAllItems();
        }
        private NotFoundViewModel GetViewModelNotFoundPane()
        {
            if (notFoundPane == null) return null;
            return ((notFoundPane.ContentControl as HostToolsPane).Host.Child as System.Windows.FrameworkElement).DataContext as NotFoundViewModel;
        }



       

    }
}
