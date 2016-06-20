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

        public string DistrictReplace { get; set; } = " район Санкт-Петербурга";
        public string DistrictKeyWord { get; set; } = " район ";

        public bool IsFirstComplite { get; set; } 

        #endregion

        #region Commands

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

       

        [OnCommand("ShowFirstResultCommand")]
        public void ShowFirstResult() => OpenNotFoundPane();
        [OnCommandCanExecute("ShowFirstResultCommand")]
        public bool ShowFirstResultCanExecute() => IsFirstComplite;


        [OnCommand("FirstMarcosCommand")]
        public void StartDistrictMarcos()
        {
            IsFirstComplite = false;
           

            foreach (var profile in _repository.ProfileList.Items.Where(i => i.IsActive))
                if (!profileCheck(profile))
                {
                    _userMsgService.MsgShow($"В профиле {profile.ProfileName} не заданы начальные и конечные ячейки");
                    return;
                }


            ClearNotFoundPane();
            SetRegexToAddressTable();

            foreach (var profile in _repository.ProfileList.Items.Where(i => i.IsActive))
            {
                var list = new ObservableCollection<AddressModel>();

                //if (!profileCheck(profile))
                //{
                //    _userMsgService.MsgShow($"В профиле {profile.ProfileName} не заданы начальные и конечные ячейки");
                //    continue;
                //}

                Worksheet workSheet = null;
                foreach (Worksheet sheet in _excelApplication.Worksheets)
                    if (sheet.Name == profile.WorksheetName)
                        workSheet = sheet;
                if (workSheet != null)
                {
                    list = anything(workSheet, profile);
                    if (profile.IsPrintResult)
                        printResult(list, profile);

                    var viewModel = OpenNotFoundPane();
                    //viewModel.RemoveAllItems();
                    viewModel.AddItem(list, profile);
                }
                else
                    _userMsgService.MsgShow($"В профиле {profile.ProfileName} не правильно указано имя листа");
                /*сообщение об неправильном имени*/
            }

            IsFirstComplite = true;
        }

        private void printResult(ObservableCollection<AddressModel> list, WorkSheetProfile profile)
        {
            if (list.Count == 0) return;

            var onceRowCount = list.First().GetRowCount();
            var onceColumnCount = list.First().GetColumnCount();

            object[,] data = new object[(onceRowCount * list.Count), onceColumnCount + 1];

            var curRow = 0;
            for (int i = 0; i < list.Count; i++)
            {
                var address = list[i];
                foreach (DictionaryEntry cell in address.GetDataTable())
                {
                    data[curRow, 0] = address.District;
                    data[curRow, 1] = address.Address;
                    data[curRow, 2] = address.Uid;
                    data[curRow, 3] = address.Description;
                    data[curRow, 4] = address.Regex;
                    data[curRow, 5] = address.Number;
                    data[curRow, 6] = cell.Key;

                    double _value;
                    if (double.TryParse(cell.Value + "", System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out _value))
                        data[curRow, 7] = _value;
                    else
                    {
                        data[curRow, 7] = cell.Value;
                        data[curRow, 8] = "не удалось конвертировать стоимость";
                    }


                    curRow++;
                }
            }

            //имя листа отчета

            string resultSheetName = profile.ProfileName.Length > 25 ? profile.ProfileName.Substring(0, 25) + "_res" : profile.ProfileName + "_res";
            int wsIndx = 0;
            while (true)
            {
                var temp = wsIndx;
                foreach (Worksheet ws in _excelApplication.Worksheets)
                    if (ws.Name == resultSheetName)
                    {
                        resultSheetName = profile.ProfileName + "_res" + ++wsIndx;
                        break;
                    }
                if (temp == wsIndx) break;
            }

            
            _excelApplication.Worksheets.Add(After: _excelApplication.Worksheets[_excelApplication.Worksheets.Count]).Name = resultSheetName;
            var sheet = _excelApplication.Worksheets[resultSheetName] as Worksheet;
            WriteArray(data, sheet);
        }


        private void WriteArray(object[,] cells, Worksheet worksheet)
        {
            var rows = cells.GetLongLength(0);
            var columns = cells.GetLongLength(1);

            var startCell = (Range)worksheet.Cells[1, 1];
            var endCell = (Range)worksheet.Cells[rows, columns];
            var writeRange = worksheet.Range[startCell, endCell];

            writeRange.Value2 = cells;
        }

        [OnCommand("SecontMarcosCommand")]
        public void StartDistrictMarcos2()
        {
            #region MyRegion

            var vm = GetViewModelNotFoundPane();
            if (vm == null) return;


            List<WorkSheetProfile> profiles = new List<WorkSheetProfile>();  //все профили
            List<IList<AddressModel>> addresLists = new List<IList<AddressModel>>(); //найденные адреса
            List<int> counters = new List<int>(); //счетчик каждому списку

            foreach (var atoa in vm.TabItems)
            {
                var l = atoa.GetFoundItems();
                if (l.Count != 0)
                {
                    profiles.Add(atoa.Profile);
                    counters.Add(0);
                    addresLists.Add(l);
                }

            }

            //ключи для разворачивания строк
            var keyList = GetProfileKeys(profiles);

            #endregion


            var RESULT = CreateResultTable(profiles);

            //находим первое минимальное значение
            List<AddressModel> adrs = new List<AddressModel>();
            foreach (var list in addresLists)
                adrs.Add(list.First());
            long curentNumber = GetMinNumber(adrs);



            while (true)
            {
                AddressModel[] adrSynhArray = new AddressModel[addresLists.Count];
                adrs.Clear();


                for (int basei = 0; basei < addresLists.Count; basei++)
                {
                    var cur_index = counters[basei];
                    var cur_addressList = addresLists[basei];
                    var cur_profile = addresLists[basei];

                    if (cur_index >= cur_addressList.Count) continue; //прерывание если адресный список кончился

                    var cur_address = cur_addressList[cur_index];

                    if (cur_address.Number > curentNumber)
                    {
                        //adrs.Add(cur_address);
                    }
                    else if (cur_address.Number == curentNumber)
                    {
                        adrSynhArray[basei] = (cur_address);
                        counters[basei]++;
                    }
                    else
                    {
                        throw new ArgumentException("алгоритм накрылся!!!");
                    }
                }


                SetResultData(ref RESULT, adrSynhArray, keyList, profiles);

                if (adrs.Count == 0) //если все итемы попали в итог тонадо снова найти минимальный номер из всех итемов
                    for (int basei = 0; basei < addresLists.Count; basei++)
                        if (counters[basei] < addresLists[basei].Count)
                            adrs.Add(addresLists[basei][counters[basei]]);
                if (adrs.Count == 0) break; //если не осталось адресов с минималкой то мы закончили обработку и надо выходить
                curentNumber = GetMinNumber(adrs);



                bool IsFinish = false;
                for (int basei = 0; basei < addresLists.Count; basei++)
                {
                    var cur_index = counters[basei];
                    var cur_addressList = addresLists[basei];
                    if (cur_index < cur_addressList.Count)
                    {
                        IsFinish = false;
                        break;
                    };
                    IsFinish = true;
                }
                if (IsFinish) break;


            }

            var r = RESULT;

            StringBuilder sb = new StringBuilder();
            foreach (System.Data.DataColumn item in RESULT.Columns)
                sb.Append(item.ColumnName + "\t");
            sb.Append("\r\n");
            foreach (System.Data.DataRow item in RESULT.Rows)
            {
                for (int i = 0; i < RESULT.Columns.Count; i++)
                {
                    sb.Append(item[i] + "\t");
                }
                sb.Append("\r\n");
            }

            System.Windows.Clipboard.SetText(sb.ToString());
        }

        [OnCommandCanExecute("SecontMarcosCommand")]
        public bool StartDistrictMarcos2CanExecute() => GetViewModelNotFoundPane() != null;

        #endregion


        private void SetResultData(ref System.Data.DataTable table, AddressModel[] synhAddresses, List<string> KeyList, List<WorkSheetProfile> synhProfiles)
        {
            foreach (var key in KeyList)
            {
                
                    var anyAddress = synhAddresses.FirstOrDefault(adr => !string.IsNullOrWhiteSpace(adr?.Uid)); //тут могут быть нули
                    var addressFromRepository = _repository.AddressList.Items.First(i => i.Uid == anyAddress.Uid);
                  
                    var row = table.NewRow();
                    table.Rows.Add(row);
                    row[0] = addressFromRepository.District;
                    row[1] = addressFromRepository.Address;
                    row[2] = key;
                    row[synhAddresses.Count() + 3] = ""; //это примечание - номер столбца заранее не известен
                    row[synhAddresses.Count() + 4] = addressFromRepository.Uid; //это индификатор для нужд разработчика 


                    for (int i = 0; i < synhAddresses.Count(); i++) //тут вписывается стоимость
                    {
                        var adr = synhAddresses[i];

                    if (adr != null)
                    {
                        var str_value = adr.GetData(key);
                        double _value;
                        if (double.TryParse(str_value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out _value))
                            row[2 + i + 1] = _value;
                        else
                        {
                            if (string.IsNullOrWhiteSpace(str_value))
                            {
                                row[2 + i + 1] = 0;
                            }
                            else
                            {
                                row[2 + i + 1] = str_value;
                                row[synhAddresses.Count() + 3] = row[synhAddresses.Count() + 3] + "не удалось конвертировать;";
                            }
                        }
                    }
                    else
                        {
                            row[synhAddresses.Count() + 3] = row[synhAddresses.Count() + 3] + "Адрес полностью исключен из " + synhProfiles[i].ProfileName + "; ";
                        }
                    }
            }
        }


        private System.Data.DataTable CreateResultTable(List<WorkSheetProfile> profiles)
        {
            var dt = new System.Data.DataTable();

            dt.Columns.Add(new System.Data.DataColumn { ColumnName = "Район", DataType = typeof(string) });
            dt.Columns.Add(new System.Data.DataColumn { ColumnName = "Адрес", DataType = typeof(string) });
            dt.Columns.Add(new System.Data.DataColumn { ColumnName = "Вид работ", DataType = typeof(string) });

            for (int i = 0; i < profiles.Count; i++)
                dt.Columns.Add(new System.Data.DataColumn { ColumnName = "Стоимость по " + profiles[i].ProfileName, DefaultValue = 0/*, DataType = typeof(string)*/ });

            dt.Columns.Add(new System.Data.DataColumn { ColumnName = "Примечание", DataType = typeof(string) });
            dt.Columns.Add(new System.Data.DataColumn { ColumnName = "Индификатор", DataType = typeof(string)});

            return dt;
        }

        private long GetMinNumber(List<AddressModel> addresses)
        {
            List<long> numbers = new List<long>(addresses.Count);
            foreach (var address in addresses)
                numbers.Add(address.Number);
            return numbers.Min();
        }

        //определим максимальное количество ключей при разворачивании строки
        private List<string> GetProfileKeys(List<WorkSheetProfile> counters)
        {
            var retList = new List<string>();
            Dictionary<string, int> namesCount = new Dictionary<string, int>();
            foreach (var dict in counters)
                foreach (var profileItem in dict.Items.Where(key => key.Column > 0).OrderBy(key => key.Name))
                {
                    var name = profileItem.Name;
                    if (namesCount.ContainsKey(name)) namesCount[name]++;
                    else namesCount.Add(name, 1);
                }

            foreach (var item in namesCount.Where(i => i.Value == counters.Count))
                retList.Add(item.Key);

            return retList;
        }

        public ObservableCollection<AddressModel> anything(Worksheet sheet, WorkSheetProfile profile)
        {

            var regex = _repository.RegexList;
            var addresses = _repository.AddressList;

            ObservableCollection<AddressModel> returnAddressList = new ObservableCollection<AddressModel>();
            string lastDistrict = "";

            for (int i = profile.FirstDistrictCell.Row; i < profile.LastAddressCell.Row + 1; i++)
            {

                var curent_district = (sheet.Cells[i, profile.FirstDistrictCell.Column] as Range).Value + "";
                var curent_address = (sheet.Cells[i, profile.FirstAddressCell.Column] as Range).Value + "";

                if (!string.IsNullOrWhiteSpace(curent_district) && curent_district.Contains(DistrictKeyWord))
                {
                    lastDistrict = (curent_district).Replace(DistrictReplace, "");
                }
                //else
                //{

                //}


                if (!string.IsNullOrWhiteSpace(lastDistrict) && !string.IsNullOrWhiteSpace(curent_address) && !curent_address.ToLower().Contains("итого "))
                {


                    string curent_rx_replace = curent_address;
                    foreach (var rx in regex.Items.OrderBy(it => it.Order))
                    {
                        curent_rx_replace = Regex.Replace(curent_rx_replace, rx.Expression, rx.ReplceExpression);
                    }
                    curent_rx_replace = lastDistrict + curent_rx_replace;


                    /*Надо проверить на уникальность address.Uid ! Если не уникально выкинуть все с данным юидом в нули а также запомнить его*/
                    /*А вообще лучше это выкинуть в постобработку*/
                    var address = new AddressModel();
                    address.District = lastDistrict;
                    address.Address = curent_address;
                    address.Regex = curent_rx_replace;
                    address.Uid = addresses.Items.FirstOrDefault(it => it.Regex == curent_rx_replace)?.Uid;

                    //var maxCol = profile.Items.Max(it => it.Column);
                    //var minCol = 1;//profile.Items.Where(it => it.Column > 0).Min(it => it.Column);

                    //Range c1 = sheet.Cells[i, minCol];
                    //Range c2 = sheet.Cells[i, maxCol];
                    //Range range = sheet.Range[c1, c2]; //ни фига на скорость не влияет все равно около 3 сек

                    //var asd = XlCall.Excel(XlCall.xlfGetCell, 53, range);
                    //XlCall.xlfGetCell

                    foreach (var profileItem in profile.Items)
                    {
                        if (profileItem.Column <= 0) continue;
                        var val = (sheet.Cells[i, profileItem.Column] as Range).Value + "";
                        address.SetData(profileItem.Name, val);
                    }

                    returnAddressList.Add(address);
                }

            }

            /*проверка на повторяющиеся индификаторы*/
            Dictionary<string, int> namesCount = new Dictionary<string, int>();
            foreach (var item in returnAddressList.Where(adr=>!string.IsNullOrWhiteSpace(adr.Uid)).ToList())
            {
                if (namesCount.ContainsKey(item.Uid))
                    namesCount[item.Uid]++;
                else
                    namesCount.Add(item.Uid, 1);
            }

            var correctList = namesCount.Where(i => i.Value > 1).ToList();

            foreach (var item in correctList)
                returnAddressList.First(adr => adr.Uid == item.Key).Uid = null;


            return returnAddressList;
        }

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


        private bool profileCheck(WorkSheetProfile profile)
        {
            return
                profile.FirstAddressCell.Row > 0 &&
                profile.FirstAddressCell.Column > 0 &&
                profile.LastAddressCell.Row > 0 &&
                profile.LastAddressCell.Column > 0 &&
                profile.FirstDistrictCell.Row > 0 &&
                profile.FirstDistrictCell.Column > 0;
        }

        private void SetRegexToAddressTable()
        {
            foreach (var adr in _repository.AddressList.Items)
            {
                adr.Regex = null;
                foreach (var rgx in _repository.RegexList.Items)
                    adr.Regex = Regex.Replace(string.IsNullOrWhiteSpace(adr.Regex) ? adr.Address : adr.Regex + "", rgx.Expression, rgx.ReplceExpression);
                adr.Regex = adr.District + adr.Regex;
            }
        }

        //private void AddColumn(int count = 1)
        //{
        //    var worksheet = _excelApplication.ActiveSheet as Worksheet;
        //    for (int i = 0; i < count; i++)
        //        worksheet.Range["A:A"].Insert(XlInsertShiftDirection.xlShiftToRight, XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
        //}
        //private void StopCalculation()
        //{
        //    _excelApplication.ScreenUpdating = false;
        //    _excelApplication.Calculation = XlCalculation.xlCalculationManual;
        //}
        //private void StartCalculation()
        //{
        //    _excelApplication.ScreenUpdating = true;
        //    _excelApplication.Calculation = XlCalculation.xlCalculationAutomatic;
        //}
        //private int GetRowCount()
        //{
        //    var lasti = 1;
        //    var nullCount = 0;
        //    var worksheet = _excelApplication.ActiveSheet as Worksheet;
        //    for (int i = 1; i < worksheet.Rows.Count; i++)
        //    {
        //        var val_address = (worksheet.Cells[i, Column_AddressNumber] as Range).Value + "";
        //        if (string.IsNullOrWhiteSpace(val_address))
                    
        //            nullCount++;
        //        else
        //            lasti = i;
        //        if (nullCount > 100) return lasti;
        //    }
        //    return 0;
        //}
        //private int GetColumnCount()
        //{
        //    var lasti = 1;
        //    var nullCount = 0;
        //    var worksheet = _excelApplication.ActiveSheet as Worksheet;
        //    for (int i = 1; i < worksheet.Columns.Count; i++)
        //    {
        //        var val_address = (worksheet.Cells[Row_AddressStartNumber, i] as Range).Value + "";
        //        if (string.IsNullOrWhiteSpace(val_address))
        //            nullCount++;
        //        else
        //            lasti = i;
        //        if (nullCount > 100) return lasti;
        //    }
        //    return 0;
        //}

    }
}
