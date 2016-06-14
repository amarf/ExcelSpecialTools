using ExcelAnalysisTools.Aspects;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelAnalysisTools.ViewModel
{
    public class ShellViewModel
    {
        private readonly IServiceLocator _serviceLocator;

        public ShellViewModel(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        PrimaryProcessingsViewModel _primaryProcessings;
        OptionsViewModel _options;

        public PrimaryProcessingsViewModel PrimaryProcessings { get { return GetIns(ref _primaryProcessings); } }
        public OptionsViewModel Options { get { return GetIns(ref _options); } }



        private T GetIns<T>(ref T obj)
        {
            if (obj == null)
                return (obj = _serviceLocator.GetInstance<T>());
            else
                return obj;
        }
    }
}
