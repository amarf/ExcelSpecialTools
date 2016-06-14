using Core.Interfaces;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExcelAnalysisTools.Services
{
    [ImplementPropertyChanged, Serializable, XmlRoot("options", Namespace = "", IsNullable = false)]
    public class OptionsService : IOptionsService
    {
        [XmlIgnore]
        public string SubPath { get; } = @"\ExcelToolsOptions.xml";
        [XmlIgnore]
        public string PersonalPath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        [XmlIgnore]
        public string FullPath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\ExcelToolsOptions.xml";

        [XmlElement("addressPath")]
        public string AddressListPath { get; set; }
        [XmlElement("regexPath")]
        public string RegexListPath { get; set; }
    }
}
