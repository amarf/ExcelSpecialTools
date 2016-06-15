using Commander;
using Core.Interfaces;
using ExcelAnalysisTools.Model;
using ExcelAnalysisTools.Services;
using ExcelDna.Integration;
using Microsoft.Office.Interop.Excel;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExcelAnalysisTools.ViewModel
{
    [ImplementPropertyChanged]
    public class PrimaryProcessingsViewModel : IDisposable
    {
        private readonly Application _excelApplication;
        private readonly Repository _repository;

        public PrimaryProcessingsViewModel(Repository repository)
        {
            _repository = repository;

            _excelApplication = (Application)ExcelDnaUtil.Application;
            _excelApplication.SheetSelectionChange += _excelApplication_SheetSelectionChange;
        }
        public void Dispose()
        {
            _excelApplication.SheetSelectionChange += _excelApplication_SheetSelectionChange;
        }


        public bool IsSelectDistrictColumn { get; set; }
        public bool IsSelectAddressColumn { get; set; }
        public int Column_DistrictNumber { get; set; } = 1;
        public int Column_AddressNumber { get; set; } = 2;

        public int Row_DistrictStartNumber { get; set; } = 13;
        public int Row_AddressStartNumber { get; set; } = 14;

        public string DistrictReplace { get; set; } = " район Санкт-Петербурга";


        

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



        [OnCommand("SelectDistrictColumnCommand")]
        public void SelectDistrictColumn(bool isCheked)
        {
            if (isCheked)
            {
                IsSelectAddressColumn = false;
                IsSelectDistrictColumn = true;
            }
            else
                IsSelectDistrictColumn = false;
        }

        [OnCommand("SelectAddressColumnCommand")]
        public void SelectAddressColumn(bool isCheked)
        {
            if (isCheked)
            {
                IsSelectAddressColumn = true;
                IsSelectDistrictColumn = false;
            }
            else
                IsSelectAddressColumn = false;
        }
        [OnCommand("StartDistrictMarcosCommand")]
        public void StartDistrictMarcos()
        {
            StopCalculation();
            AddColumn(4);
            Column_DistrictNumber = Column_DistrictNumber + 4;
            Column_AddressNumber = Column_AddressNumber + 4;

            var rowCount = GetRowCount();
            var lastDistrict = "";
            var worksheet = _excelApplication.ActiveSheet as Worksheet;

            //REGEX !!!
            var addresses = _repository.AddressList;
            var regex = _repository.RegexList;
            foreach (var address in addresses.Items)
            {
                var rx_replace =  address.Address;
                foreach (var rx in regex.Items.OrderBy(it => it.Order))
                {
                    rx_replace = Regex.Replace(rx_replace, rx.Expression, rx.ReplceExpression);
                }
                address.Regex = address.District + rx_replace;
            }

            //var rowCount = worksheet.Rows.End[XlDirection.xlUp].Row;  район Санкт-Петербурга
            for (int i = 1; i < rowCount + 1; i++)
            {

                var val_district = (worksheet.Cells[i, Column_DistrictNumber] as Range).Value + "";
                var val_address = (worksheet.Cells[i, Column_AddressNumber] as Range).Value + "";


                if (!string.IsNullOrWhiteSpace(val_district) && val_district.Contains("район "))
                {
                    lastDistrict = (val_district).Replace(DistrictReplace, "");
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(lastDistrict) && !string.IsNullOrWhiteSpace(val_address) && !val_address.ToLower().Contains("итого "))
                    {
                        string curent_rx_replace = val_address;
                        foreach (var rx in regex.Items.OrderBy(it=>it.Order))
                        {
                            curent_rx_replace = Regex.Replace(curent_rx_replace, rx.Expression, rx.ReplceExpression);
                        }
                        curent_rx_replace = lastDistrict + curent_rx_replace;

                        var firstItem = addresses.Items.FirstOrDefault(it => it.Regex == curent_rx_replace); ;
                        worksheet.Cells[i, 2] = firstItem?.Regex;


                        worksheet.Cells[i, 1].FormulaR1C1 = $"=RC4&RegexReplacePlus(RC{Column_AddressNumber},РП!R2C7:R36C7,РП!R2C8:R36C8)";
                        //worksheet.Cells[i, 2].FormulaR1C1 = $"=MATCH(RC[-1], РП!C5, 0)";
                        worksheet.Cells[i, 3].FormulaR1C1 = $"=COUNTIF(C[-1],RC[-1])";
                        worksheet.Cells[i, 4] = lastDistrict;
                    }
                }

            }

            StartCalculation();

            //Selection.AutoFilter
            //var filterRange = $"R{Row_DistrictStartNumber}C1:R{rowCount}C{GetColumnCount()}";
            //worksheet.Range[filterRange].AutoFilter();
            //worksheet.Range[filterRange].AutoFilter();
        }


        private void AddColumn(int count = 1)
        {
            var worksheet = _excelApplication.ActiveSheet as Worksheet;
            for (int i = 0; i < count; i++)
                worksheet.Range["A:A"].Insert(XlInsertShiftDirection.xlShiftToRight, XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
        }
        private void StopCalculation()
        {
            _excelApplication.ScreenUpdating = false;
            _excelApplication.Calculation = XlCalculation.xlCalculationManual;
        }
        private void StartCalculation()
        {
            _excelApplication.ScreenUpdating = true;
            _excelApplication.Calculation = XlCalculation.xlCalculationAutomatic;
        }
        private int GetRowCount()
        {
            var lasti = 1;
            var nullCount = 0;
            var worksheet = _excelApplication.ActiveSheet as Worksheet;
            for (int i = 1; i < worksheet.Rows.Count; i++)
            {
                var val_address = (worksheet.Cells[i, Column_AddressNumber] as Range).Value + "";
                if (string.IsNullOrWhiteSpace(val_address))
                    
                    nullCount++;
                else
                    lasti = i;
                if (nullCount > 100) return lasti;
            }
            return 0;
        }
        private int GetColumnCount()
        {
            var lasti = 1;
            var nullCount = 0;
            var worksheet = _excelApplication.ActiveSheet as Worksheet;
            for (int i = 1; i < worksheet.Columns.Count; i++)
            {
                var val_address = (worksheet.Cells[Row_AddressStartNumber, i] as Range).Value + "";
                if (string.IsNullOrWhiteSpace(val_address))
                    nullCount++;
                else
                    lasti = i;
                if (nullCount > 100) return lasti;
            }
            return 0;
        }

    }
}
