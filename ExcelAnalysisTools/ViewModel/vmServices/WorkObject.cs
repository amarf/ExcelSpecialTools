using Core.Interfaces;
using ExcelAnalysisTools.Model;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelAnalysisTools.ViewModel.vmServices
{
    public class WorkObject
    {

        WorkSheetProfile _profile;
        Worksheet _worksheet;

        public object[,] ActiveRange { get; private set; }
        public int Counter { get; set; } = 0;

        public WorkSheetProfile Profile
        {
            get
            {
                return _profile;
            }
            set
            {
                _profile = value;
                SetAviveRange();
            }
        }
        public Worksheet Worksheet
        {
            get
            {
                return _worksheet;
            }
            set
            {
                _worksheet = value;
                SetAviveRange();
            }
        }

        public ObservableCollection<AddressModel> Addresses { get; set; } = new ObservableCollection<AddressModel>();


        private void SetAviveRange()
        {
            if (_profile == null || _worksheet == null) { ActiveRange = null; return; }

            var lastRow = _profile.GetLastRow();
            var lastColumn = _profile.GetLastColumn();

            if (lastRow == null || lastColumn == null) { ActiveRange = null; return; }

            var firstCell = Worksheet.Cells[1, 1];
            var lastCell = Worksheet.Cells[lastRow, lastColumn];
            Range excelRange = Worksheet.Range[firstCell, lastCell];

            try
            {
                ActiveRange = (object[,])excelRange.get_Value(XlRangeValueDataType.xlRangeValueDefault);
            }
            catch (Exception e)
            {
                ActiveRange = null;
                var error = string.IsNullOrWhiteSpace(e.InnerException.Message) ? e.Message : e.InnerException.Message;
                Debug.Print("***Размер таблицы слишком велик для обработки: " + error);
            }
        }
    }
}
