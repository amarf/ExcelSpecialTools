using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Core.Interfaces
{
    public interface IOptionsService
    {
        string OptionsFileSubPath { get; }
        string OptionsFolderPath { get; }
        string OptionsFileFullPath { get; }

        string AddressListPath { get; set; }
        string RegexListPath { get; set; }
    }
}
