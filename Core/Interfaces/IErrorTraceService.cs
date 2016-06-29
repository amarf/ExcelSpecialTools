using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IErrorTraceService
    {
        void Trace(Exception e);
        void Trace(Exception e, string message);
    }
}
