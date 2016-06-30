using Commander;
using InlineSearch.Model;
using X = Microsoft.Office.Interop.Excel;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelDna.Integration;
using NullGuard;
using System.Runtime.InteropServices;

namespace InlineSearch.ViewModel
{
    [ImplementPropertyChanged]
    public class ProfileEditorViewModel
    {
        private readonly X.Application _excelApplication;

        public Profile Profile { get; set; } = new Profile();


        public ProfileEditorViewModel()
        {
            _excelApplication = (X.Application)ExcelDnaUtil.Application;
        }

        //[ExcelCommand(MenuName = "Test Range Macros - C API", MenuText = "Double the Range")]
        [OnCommand("AddNewKeyCommand")]
        private void AddNewKey()
        {
            Profile
                .With(i => i.Keys)
                .Do(i => i.Add(new KeyItem { }));
        }

        [OnCommand("SelectColumnCommand")]
        private async void SelectColumnCommand(KeyItem key)
        {
            //key.Colunm = await Task.Run(() =>
            //{
            //    int count = 0;
            //    int first = _excelApplication.ActiveCell.Column;
            //    int last = first;
            //    while (count++>100)
            //    {
            //        Task.Delay(200);
            //        last = _excelApplication.ActiveCell.Column;
            //        if (last != first)
            //            break;
            //    }
            //    return last;
            //});
        }
    }
}
