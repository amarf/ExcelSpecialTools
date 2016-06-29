using PropertyChanged;
using System;
using System.Xml.Serialization;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.IO;
using System.ComponentModel;

namespace Core.Model
{
    [ImplementPropertyChanged, Serializable, XmlRoot("options", Namespace = "", IsNullable = false)]
    public class CoreOption
    {
        [XmlIgnore]
        public static string FileName { get;} = @"ExcelToolsOptions.xml";
        [XmlIgnore]
        public static string Folder { get; } = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        [XmlIgnore]
        public static string FileFullName { get; } = Path.Combine(Folder, FileName);

        [XmlArray("values"), XmlArrayItem("option")]
        public ObservableCollection<CoreOptionItem> Values { get; set; }

        public CoreOption()
        {
            Values = new ObservableCollection<CoreOptionItem>();
        }

        public string this[string optionKey]
        {
            get
            {
                var item = Values.LastOrDefault(i => i.Key == optionKey);
                return item?.Value ?? null;
            }
            set
            {
                var item = Values.LastOrDefault(i => i.Key == optionKey);
                if (item != null)
                    item.Value = value;
                else
                    Values.Add(new CoreOptionItem { Key = optionKey, Value = value, AssociativeType = value.GetType().FullName });
            }
        }
        public string this[Type optionDataType]
        {
            get
            {
                var item = Values.LastOrDefault(i => i.AssociativeType == optionDataType.FullName);
                return item?.Value ?? null;
            }
            set
            {
                var item = Values.LastOrDefault(i => i.AssociativeType == optionDataType.FullName);
                if (item != null)
                    item.Value = value;
                else
                    Values.Add(new CoreOptionItem { Key = value.GetHashCode() + "", Value = value , AssociativeType = optionDataType.FullName });
            }
        }
    }
}
