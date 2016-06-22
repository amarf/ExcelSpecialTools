using PropertyChanged;
using System;
using System.Xml.Serialization;
using System.Linq;
using System.Reflection;
using System.IO;

namespace ExcelAnalysisTools.Model
{
    [ImplementPropertyChanged, Serializable, XmlRoot("options", Namespace = "", IsNullable = false)]
    public class Options
    {
        [XmlIgnore]
        public static string OptionsFileSubPath { get;} = @"\ExcelToolsOptions.xml";
        [XmlIgnore]
        public static string OptionsFolderPath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        [XmlIgnore]
        public static string OptionsFileFullPath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\ExcelToolsOptions.xml";

        string _addressListPath;
        string _regexListPath;
        string _profileListPath;

        [XmlElement("addressPath")]
        public string AddressListPath
        {
            get
            {
                if (File.Exists(_addressListPath))
                    return _addressListPath;
                else
                    return null;
            }
            set
            {
                _addressListPath = value;
            }
        }
        [XmlElement("regexPath")]
        public string RegexListPath
        {
            get
            {
                if (File.Exists(_regexListPath))
                    return _regexListPath;
                else
                    return null;
            }
            set
            {
                _regexListPath = value;
            }
        }
        [XmlElement("profilePath")]
        public string ProfileListPath
        {
            get
            {
                if (File.Exists(_profileListPath))
                    return _profileListPath;
                else
                    return null;
            }
            set
            {
                _profileListPath = value;
            }
        }


        public string GetDataPath<T>()
        {
            var property = GetProperty<T>();
            if (property != null)
                return (string)property.GetValue(this);
            return null;
        }

        public void SetDataPath<T>(string path)
        {
            var property = typeof(T).Name != "Options" ? GetProperty<T>() : null;
            if (property!=null)
                property.SetValue(this, path);
        }

        private PropertyInfo GetProperty<T>()
        {
            if (typeof(T).Name == "Options")
                return this.GetType().GetProperty(nameof(OptionsFileFullPath));
            else if (typeof(T).Name == "AddressList")
                return this.GetType().GetProperty(nameof(AddressListPath));
            else if (typeof(T).Name == "RegexExpressionList")
                return this.GetType().GetProperty(nameof(RegexListPath));
            else if (typeof(T).Name == "ProfileList")
                return this.GetType().GetProperty(nameof(ProfileListPath));
            else
                return null;
        }
    }
}
