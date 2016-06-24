using ExcelAnalysisTools.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelAnalysisTools.ViewModel.DesignDContext
{
    public class Addresses
    {
        public ObservableCollection<AddressModel> Items { get; set; }


        public Addresses()
        {
            Items = new ObservableCollection<AddressModel>
            {

                new AddressModel { District = "Район 9", Address = "новый адресс д.19", Description = "Описание ", KgiopStatus = "Объект федерального значения" },
                new AddressModel { District = "Район 9", Address = "новый адресс д.19", Description = "Описание ", KgiopStatus = "Объект федерального значения" },
                new AddressModel { District = "Район 9", Address = "новый адресс д.19", Description = "Описание ", KgiopStatus = "Объект федерального значения" },
                new AddressModel { District = "Район 9", Address = "новый адресс д.19", Description = "Описание ", KgiopStatus = "Объект федерального значения" },
                new AddressModel { District = "Район 9", Address = "новый адресс д.19", Description = "Описание ", KgiopStatus = "Объект федерального значения" },
                new AddressModel { District = "Район 9", Address = "новый адресс д.19", Description = "Описание ", KgiopStatus = "Объект федерального значения" },
                new AddressModel { District = "Район 9", Address = "новый адресс д.19", Description = "Описание ", KgiopStatus = "Объект федерального значения" },

            };
        }
    }
}
