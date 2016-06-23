using Core.Interfaces;
using ExcelAnalysisTools.Services;
using ExcelAnalysisTools.Model;
using ExcelDna.Integration;
using Microsoft.Office.Interop.Excel;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections;

namespace ExcelAnalysisTools.ViewModel.vmServices
{
    //Итогом работы класса являются полностью подготовленные для анализа данные 
    //и сам список этих данных List<WorkObject>
    public class Class1
    {
        string lastDistrict = "";

        private readonly Application _excelApplication;
        private readonly Repository _repository;
        private readonly IUserMsgService _userMsgService;
        private readonly IServiceLocator _serviceLocator;

        public Class1(Repository repository, IUserMsgService userMsgService, IServiceLocator serviceLocator)
        {
            _repository = repository;
            _userMsgService = userMsgService;
            _serviceLocator = serviceLocator;
            _excelApplication = (Application)ExcelDnaUtil.Application;
        }


        public IList<WorkObject> CollectData() //мы должны сдесь создать адрессные списки
        {
            var workObjList = GetActiveProfiles(_repository.ProfileList.Items);
            if (workObjList.Count == 0)
                return null;

            if (!ApplyRegexToAddresses())
            {
                _userMsgService.MsgShow($"Не удалось применить шаблоны Regex к адресному списку. Операция прервана.");
                return null;
            }

            

            foreach (var workObj in workObjList)
            {
                if(!profileCheck(workObj.Profile))
                {
                    _userMsgService.MsgShow($"Профиль [{workObj.Profile.ProfileName}] исключен из обработки из-за ошибок воода (debug: Class1 profileCheck)");
                    continue;
                }
                if(workObj.ActiveRange == null)
                {
                    _userMsgService.MsgShow($"При обработке профиля [{workObj.Profile.ProfileName}] произошли ошибки (debug: WorkObject class - ActiveRange == null)");
                    continue;
                }


                CollectDataFromProfile(workObj);
            }

            return workObjList;
        }

        private bool ApplyRegexToAddresses()
        {
            if (_repository.AddressList?.Items == null || _repository.AddressList?.Items.Count == 0) return false;

            //проверяем сами регулярные выражения - т.к. они могут быть не корректны
            try 
            {
                var first = _repository.AddressList.Items.FirstOrDefault();
                foreach (var rgx in _repository.RegexList.Items)
                    first.Regex = string.IsNullOrWhiteSpace(first.Regex)
                        ? Regex.Replace(first.Address, rgx.Expression, rgx.ReplceExpression)
                        : Regex.Replace(first.Regex, rgx.Expression, rgx.ReplceExpression);

                first.Regex = null;
            }
            catch 
            {
                return false;
            }
            

            Dictionary<string, int> regValuesCount = new Dictionary<string, int>(); //проверка на уникальность
            int lastNumber = 0; //теперь индификатор аддреса это просто справочная информация
            foreach (var adr in _repository.AddressList.Items)
            {
                adr.Regex = null; //обнуляем если вдруг до этого были записаны значения
                adr.Number = ++lastNumber;
                foreach (var rgx in _repository.RegexList.Items)
                    adr.Regex = string.IsNullOrWhiteSpace(adr.Regex)
                        ? Regex.Replace(adr.Address, rgx.Expression, rgx.ReplceExpression)
                        : Regex.Replace(adr.Regex, rgx.Expression, rgx.ReplceExpression);

                adr.Regex = adr.District + adr.Regex;

                if (regValuesCount.ContainsKey(adr.Regex))
                    regValuesCount[adr.Regex]++;
                else
                    regValuesCount.Add(adr.Regex, 1);

            }

            //проверка на уникальность
            var negativeList = regValuesCount.Where(i => i.Value > 1);
            StringBuilder sb = new StringBuilder();
            foreach (var hashValue in negativeList)
            {
                var list = _repository.AddressList.Items.Where(i => i.Regex == hashValue.Key).ToList();
                foreach (var negativeAdr in list)
                {
                    sb.AppendLine($"{negativeAdr.District}\t{negativeAdr.Address}\t{negativeAdr.Uid}\t{negativeAdr.Number}\t{negativeAdr.Regex}\t");
                    negativeAdr.Regex = null;
                }
            }

            if (negativeList.Count() > 0)
            {
                _userMsgService.MsgShow($"При обработке общего списка адресов обнаружились не уникальные значения в количестве {negativeList.Count()}\r\nСписок скопирован в буфер обмена.");
                System.Windows.Clipboard.Clear();
                System.Windows.Clipboard.SetText(sb.ToString());
            }

            return true;
        }
        private void CollectDataFromProfile(WorkObject workObj)
        {
            workObj.Addresses.Clear();
           

            var distColumn = workObj.Profile.FirstDistrictCell.Column;
            var adrColumn = workObj.Profile.FirstAddressCell.Column;
            var districtKeyWord = workObj.Profile.DistrictKeyWord;
            var districtWordReplace = workObj.Profile.DistrictWordReplace;
            var addressNotKeyWord = workObj.Profile.AddressNotKeyWord;

            Dictionary<string, int> regValuesCount = new Dictionary<string, int>(); //проверка на уникальность

            for (int row = workObj.Profile.FirstDistrictCell.Row; row < workObj.Profile.GetLastRow() + 1; row++)
            {
                AddressModel adr = null;

                var district = workObj.ActiveRange[row, distColumn] + "";
                var address = workObj.ActiveRange[row, adrColumn] + "";

                var distrcitStrings = districtKeyWord.Split('|'); //эти слова используются для поиска района
                var distrReplStrings = districtWordReplace.Split('&');
                var adrNotKeyStrings = addressNotKeyWord.Split('|');

                foreach (var str in distrcitStrings)
                {
                    if (str.Length > 0 && district.Contains(str))
                    {
                        lastDistrict = district;
                        foreach (var str_repl in distrReplStrings)
                            lastDistrict = lastDistrict.Replace(str_repl, "");
                        break;
                    }
                    else if (districtKeyWord.Length == 0)
                        lastDistrict = district;
                }



                if (!string.IsNullOrWhiteSpace(address))
                {
                    bool flag = true;
                    if (addressNotKeyWord.Length > 0)
                        foreach (var str in adrNotKeyStrings)
                            if (address.ToLower().Contains(str.ToLower()))
                            {
                                flag = false;
                                break;
                            }


                    if (flag) 
                    {
                        adr = GetNewAddress(lastDistrict, address, row, workObj);
                        workObj.Addresses.Add(adr);
                    }
                }

                //проверить на уникальность
                if (adr != null && regValuesCount.ContainsKey(adr.Regex))
                    regValuesCount[adr.Regex]++;
                else if (adr != null && !regValuesCount.ContainsKey(adr.Regex))
                    regValuesCount.Add(adr.Regex, 1);
            }

            //проверка на уникальность
            var negativeList = regValuesCount.Where(i => i.Value > 1);
            foreach (var hashValue in negativeList)
            {
                var list = workObj.Addresses.Where(i => i.Regex == hashValue.Key).ToList();
                foreach (var negativeAdr in list)
                    negativeAdr.Number = 0;
            }

        }
        private AddressModel GetNewAddress(string district, string address, int rowNumber, WorkObject workObj)
        {
            var adr = new AddressModel { District = district, Address = address};
            foreach (var item in workObj.Profile.Items.Where(i=>i.Column>0).ToList())
                adr.SetData(item.Name, workObj.ActiveRange[rowNumber, item.Column] + "");

            foreach (var rgx in _repository.RegexList.Items)
                adr.Regex = string.IsNullOrWhiteSpace(adr.Regex)
                    ? Regex.Replace(adr.Address, rgx.Expression, rgx.ReplceExpression)
                    : Regex.Replace(adr.Regex, rgx.Expression, rgx.ReplceExpression);

            adr.Regex = adr.District + adr.Regex;

            if (!string.IsNullOrWhiteSpace(adr.Regex)) //регулярки могут убить полностью значение
            {
                var findAdr = _repository.AddressList.Items.FirstOrDefault(i => i.Regex == adr.Regex);
                if (findAdr != null)
                {
                    adr.Number = findAdr.Number;
                    adr.KgiopStatus = findAdr.KgiopStatus;
                    adr.Uid = findAdr.Uid;
                }
            }

            return adr;
        }
        private List<WorkObject> GetActiveProfiles(IEnumerable<WorkSheetProfile> profiles)
        {
            //долгая операция если есть большие массивы данных т.к. при создании рабочих объектов запрашиваются диапазоны 

            var returnList = new List<WorkObject>();
            foreach (var profile in profiles)
                if (profile.IsActive && profile.Items.FirstOrDefault(i => i.Column > 0) != null)
                {
                    var workSheet = GetSheetInActiveWorkBook(profile.WorksheetName);
                    if (workSheet != null)
                        returnList.Add(new WorkObject { Profile = profile, Worksheet = workSheet });
                    else
                        _userMsgService.MsgShow($"В профиле [{profile.ProfileName}] не корректно указано имя листа");
                }
            return returnList;
        }
        private Worksheet GetSheetInActiveWorkBook(string sheetName)
        {
            Worksheet workSheet = null;
            foreach (Worksheet sheet in _excelApplication.Worksheets)
                if (sheet.Name == sheetName)
                {
                    workSheet = sheet;
                    break;
                }
            return workSheet;
        }

        /// <summary>
        /// Сообщает об удачной проверке целостности данных профиля
        /// </summary>
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

    }
}
