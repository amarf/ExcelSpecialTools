using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IRepository
    {
        DataType GetData<DataType>();
        string GetOption(string optionName);
    }
}
