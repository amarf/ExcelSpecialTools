using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class ErrorTraceService : IErrorTraceService
    {
        public void Trace(Exception e)
        {
            throw new NotImplementedException();
        }

        public void Trace(Exception e, string message)
        {
            throw new NotImplementedException();
        }
    }
}
