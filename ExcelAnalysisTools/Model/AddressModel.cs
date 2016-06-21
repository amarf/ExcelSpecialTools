using PropertyChanged;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
        public string Description { get; set; } 

        [XmlIgnore]
        public string Regex { get; set; }

        [XmlIgnore]
        public int Number { get; set; }


        private Hashtable DataTable { get; } = new Hashtable();

        public void SetData(string dataName, string value)
        {
            DataTable[dataName] = value;
        }
        public string GetData(string dataName)
        {
            return (string)DataTable[dataName];
        }

        public double GetData(string dataName, bool IsDouble, out string errorConverMsg)
        {
            errorConverMsg = "";
            string cur_val = GetData(dataName);

            if (cur_val.Trim() == "-" || cur_val == "0" || string.IsNullOrWhiteSpace(cur_val) || cur_val == null) //cur_val==null вообще ошибка разработки - такого не должно быть
                return 0.0;

            double ret_val = 0.0;
            if (double.TryParse(cur_val, NumberStyles.Any, CultureInfo.CurrentCulture, out ret_val))
            {
                if (ret_val > 200000000)
                    errorConverMsg = $"Внимание! Большое число! [{ret_val}]";
                return ret_val;
            }

            errorConverMsg = $"не удалось преобразовать значение [{cur_val}]";
            return 0;
        }

        public Hashtable GetDataTable()
        {
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
