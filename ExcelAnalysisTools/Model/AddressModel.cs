using PropertyChanged;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExcelAnalysisTools.Model
{
    [ImplementPropertyChanged, Serializable]
    public class AddressModel
    {
        [XmlAttribute("name")]
        public string Address { get; set; }
        [XmlAttribute("district")]
        public string District { get; set; }
        [XmlAttribute("uid")]
        public string Uid { get; set; } = Guid.NewGuid().ToString();
        [XmlAttribute("description")]
        public string Description { get; set; } = Guid.NewGuid().ToString();

        [XmlIgnore]
        public string Regex { get; set; }

        [XmlIgnore]
        public long Number { get; set; }


        private Hashtable DataTable { get; } = new Hashtable();

        public void SetData(string dataName, string value)
        {
            DataTable[dataName] = value;
        }
        public string GetData(string dataName)
        {
            return (string)DataTable[dataName];
        }

        public Hashtable GetDataTable()
        {
            //object[,] array = new object[6 + 2, DataTable.Count];

            //var row = 0;
            //foreach (DictionaryEntry cell in DataTable)
            //{
            //    array[row, 0] = District;
            //    array[row, 1] = Address;
            //    array[row, 2] = Uid;
            //    array[row, 3] = Description;
            //    array[row, 4] = Regex;
            //    array[row, 5] = Number;
            //    array[row, 6] = cell.Key;
            //    array[row, 7] = cell.Value;

            //    row++;
            //}
            return DataTable;
        }


        public int GetRowCount()
        {
            return DataTable.Count;
        }

        public int GetColumnCount()
        {            
            //+6 столбцов только объекты адреса
            //+2 столбца это данные хештаблицы
            return 8;
        }
    }
}
