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

        public WorkSheetProfile()
        {
            Items = new ObservableCollection<WorkSheetProfileItem>
            {
                new WorkSheetProfileItem { Name="ЭС" , Column = -1},
                new WorkSheetProfileItem { Name="ТС" , Column = -1},
                new WorkSheetProfileItem { Name="ГС" , Column = -1},
                new WorkSheetProfileItem { Name="ХВС" , Column = -1},
                new WorkSheetProfileItem { Name="ГВС" , Column = -1},
                new WorkSheetProfileItem { Name="ВО" , Column = -1},
                new WorkSheetProfileItem { Name="Фундамент" , Column = -1},
                new WorkSheetProfileItem { Name="АППЗ" , Column = -1},
                new WorkSheetProfileItem { Name="Подвал" , Column = -1},
                new WorkSheetProfileItem { Name="Лифты" , Column = -1},
                new WorkSheetProfileItem { Name="Крыша" , Column = -1},
                new WorkSheetProfileItem { Name="Фасад" , Column = -1},
                new WorkSheetProfileItem { Name="РАСК" , Column = -1},
                new WorkSheetProfileItem { Name="ПД" , Column = -1},
                new WorkSheetProfileItem { Name="ПСД ЭС" , Column = -1},
                new WorkSheetProfileItem { Name="ПСД ТС" , Column = -1},
                new WorkSheetProfileItem { Name="ПСД ГС" , Column = -1},
                new WorkSheetProfileItem { Name="ПСД ХВС" , Column = -1},
                new WorkSheetProfileItem { Name="ПСД ГВС" , Column = -1},
                new WorkSheetProfileItem { Name="ПСД ВО" , Column = -1},
                new WorkSheetProfileItem { Name="ПСД Фундамент" , Column = -1},
                new WorkSheetProfileItem { Name="ПСД АППЗ" , Column = -1},
                new WorkSheetProfileItem { Name="ПСД Подвал" , Column = -1},
                new WorkSheetProfileItem { Name="ПСД Лифты" , Column = -1},
                new WorkSheetProfileItem { Name="ПСД Крыша" , Column = -1},
                new WorkSheetProfileItem { Name="ПСД Фасад" , Column = -1},
                new WorkSheetProfileItem { Name="ПСД РАСК" , Column = -1},
            };
        }


        public static WorkSheetProfile Create(string WorkSheetName = null)
        {
            return new WorkSheetProfile
            {
                ProfileName = "Новый профиль",
                WorksheetName = WorkSheetName
            };
        }
    }
    
}
