using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExcelAnalysisTools.Model
{
    [ImplementPropertyChanged, Serializable]
    public class WorkSheetProfile
    {
        [XmlIgnore]
        public bool IsActive { get; set; }

        [XmlAttribute("isPrintResult")]
        public bool IsPrintResult { get; set; }

        [XmlAttribute("profileName")]
        public string ProfileName { get; set; }

        [XmlAttribute("worksheetName")]
        public string WorksheetName { get; set; }

        [XmlElement("firstDistrictCell")]
        public Cell FirstDistrictCell { get; set; }

        [XmlElement("firstAddressCell")]
        public Cell FirstAddressCell { get; set; }

        [XmlElement("lastAddressCell")]
        public Cell LastAddressCell { get; set; }

        [XmlArray("columns"), XmlArrayItem("column")]
        public ObservableCollection<WorkSheetProfileItem> Items { get; set; }


        public int? GetLastColumn()
        {
            return Items?.Max(i => i.Column) == 0 ? null : Items?.Max(i => i.Column);
        }
        public int? GetLastRow()
        {
            return LastAddressCell?.Row == 0 ? null : LastAddressCell?.Row;
        }

        public static WorkSheetProfile Create(string WorkSheetName = null)
        {
            return new WorkSheetProfile
            {
                ProfileName = "Новый профиль",
                WorksheetName = WorkSheetName,
                Items = new ObservableCollection<WorkSheetProfileItem>
                {
                    new WorkSheetProfileItem { Name="ЭС" , Column = 0},
                    new WorkSheetProfileItem { Name="ТС" , Column = 0},
                    new WorkSheetProfileItem { Name="ГС" , Column = 0},
                    new WorkSheetProfileItem { Name="ХВС" , Column = 0},
                    new WorkSheetProfileItem { Name="ГВС" , Column = 0},
                    new WorkSheetProfileItem { Name="ВО" , Column = 0},
                    new WorkSheetProfileItem { Name="Фундамент" , Column = 0},
                    new WorkSheetProfileItem { Name="АППЗ" , Column = 0},
                    new WorkSheetProfileItem { Name="Подвал" , Column = 0},
                    new WorkSheetProfileItem { Name="Лифты" , Column = 0},
                    new WorkSheetProfileItem { Name="Крыша" , Column = 0},
                    new WorkSheetProfileItem { Name="Фасад" , Column = 0},
                    new WorkSheetProfileItem { Name="РАСК" , Column = 0},
                    new WorkSheetProfileItem { Name="ПД" , Column = 0},
                    new WorkSheetProfileItem { Name="ПСД ЭС" , Column = 0},
                    new WorkSheetProfileItem { Name="ПСД ТС" , Column = 0},
                    new WorkSheetProfileItem { Name="ПСД ГС" , Column = 0},
                    new WorkSheetProfileItem { Name="ПСД ХВС" , Column = 0},
                    new WorkSheetProfileItem { Name="ПСД ГВС" , Column = 0},
                    new WorkSheetProfileItem { Name="ПСД ВО" , Column = 0},
                    new WorkSheetProfileItem { Name="ПСД Фундамент" , Column = 0},
                    new WorkSheetProfileItem { Name="ПСД АППЗ" , Column = 0},
                    new WorkSheetProfileItem { Name="ПСД Подвал" , Column = 0},
                    new WorkSheetProfileItem { Name="ПСД Лифты" , Column = 0},
                    new WorkSheetProfileItem { Name="ПСД Крыша" , Column = 0},
                    new WorkSheetProfileItem { Name="ПСД Фасад" , Column = 0},
                    new WorkSheetProfileItem { Name="ПСД РАСК" , Column = 0},
                }
            };
        }
    }
    
}
