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
using System.Collections.ObjectModel;
using SD = System.Data;

namespace ExcelAnalysisTools.ViewModel.vmServices
{
    public class Class2
    {
        private readonly Application _excelApplication;
        private readonly Repository _repository;
        private readonly IUserMsgService _userMsgService;
        private readonly IServiceLocator _serviceLocator;

        public Class2(Repository repository, IUserMsgService userMsgService, IServiceLocator serviceLocator)
        {
            _repository = repository;
            _userMsgService = userMsgService;
            _serviceLocator = serviceLocator;
            _excelApplication = (Application)ExcelDnaUtil.Application;
        }


        public SD.DataTable GoWork(IList<WorkObject> workObjList)
        {
            if (workObjList.Count == 0) return null;

            SortAddresses(workObjList);
            AddressModel curentMinNumberAdr;
            var resultTable = CreateTable(workObjList);
            var keys = GetResultKeys(workObjList);

            while (true)
            {
                curentMinNumberAdr = GetMinNumberAddress(workObjList);
                if (curentMinNumberAdr == null) break;
                AddDataToResultTable(resultTable, curentMinNumberAdr, workObjList, keys);
            }

            return resultTable;
        }

        private SD.DataTable CreateTable(IList<WorkObject> workObjList)
        {
            var dt = new System.Data.DataTable();

            dt.Columns.Add(new System.Data.DataColumn { ColumnName = "Район", DataType = typeof(string) });
            dt.Columns.Add(new System.Data.DataColumn { ColumnName = "Адрес", DataType = typeof(string) });
            dt.Columns.Add(new System.Data.DataColumn { ColumnName = "Вид работ", DataType = typeof(string) });

            for (int i = 0; i < workObjList.Count; i++)
                dt.Columns.Add(new System.Data.DataColumn { ColumnName = "Стоимость по " + workObjList[i].Profile.ProfileName, DefaultValue = 0, DataType=typeof(double) });

            dt.Columns.Add(new System.Data.DataColumn { ColumnName = "Примечание", DataType = typeof(string) });
            dt.Columns.Add(new System.Data.DataColumn { ColumnName = "Индификатор", DataType = typeof(string) });

            return dt;
        }
        private void AddDataToResultTable(SD.DataTable table, AddressModel curentMinNumberAdr, IList<WorkObject> workObjList, List<string> keys)
        {
            var updateCounterList = new List<WorkObject>();

            foreach (var key in keys) //строки
            {
                var row = table.NewRow();
                table.Rows.Add(row);
                row[0] = curentMinNumberAdr.District;
                row[1] = curentMinNumberAdr.Address;
                row[2] = key;

                for (int i = 0; i < workObjList.Count; i++)
                {
                    var wobj = workObjList[i];

                    if (wobj.Counter < wobj.Addresses.Count)
                    {
                        var address = wobj.Addresses[wobj.Counter];
                        if (address.Number > curentMinNumberAdr.Number)
                        {
                            row[i + 3] = 0;
                            row[3 + workObjList.Count] += $"Адрес и работа исключены из [{wobj.Profile.ProfileName}];"; /*занести в примечание сведения об исключении адреса*/
                        }
                        else if (address.Number == curentMinNumberAdr.Number)
                        {

                            string errorConverMsg;
                            row[i + 3] = address.GetData(key, true, out errorConverMsg);
                            if (!string.IsNullOrWhiteSpace(errorConverMsg))
                                row[3 + workObjList.Count] += errorConverMsg + "; ";

                            if (!updateCounterList.Contains(wobj))
                                updateCounterList.Add(wobj);
                        }
                        else
                            throw new ArgumentException();
                    }
                    else
                    {
                        row[i + 3] = 0;
                        row[3 + workObjList.Count] += $"Адрес и работа исключены из [{wobj.Profile.ProfileName}];"; /*занести в примечание сведения об исключении адреса*/
                    }
                }
            }

            foreach (var item in updateCounterList)
                item.Counter++;

        }
        private AddressModel GetMinNumberAddress(IList<WorkObject> workObjList)
        {
            AddressModel minValAddress = null;
            foreach (var wobj in workObjList)
            {
                var index = wobj.Counter;
                var list = wobj.Addresses;
                if (index < list.Count)
                    minValAddress = minValAddress == null
                        ? list[index]
                        : list[index].Number < minValAddress.Number 
                            ? list[index] 
                            : minValAddress;
            }
            return minValAddress;
        }
        private void SortAddresses(IList<WorkObject> workObjList)
        {
            foreach (var wo in workObjList)
                if (wo.Addresses.Count > 0)
                    wo.Addresses = new ObservableCollection<AddressModel>(wo.Addresses.Where(i => i.Number > 0).OrderBy(i => i.Number));
        }
        private List<string> GetResultKeys(IList<WorkObject> workObjList)
        {
            var returnList = new List<string>();
            Dictionary<string, int> hash = new Dictionary<string, int>();
            foreach (var item in workObjList)
                foreach (var key in item.Profile.Items)
                    if (key.Column > 0)
                        if (hash.ContainsKey(key.Name))
                            hash[key.Name]++;
                        else
                            hash.Add(key.Name, 1);

            foreach (var item in hash)
                if (item.Value == workObjList.Count)
                    returnList.Add(item.Key);

            return returnList;
        }


    }
}
