﻿using Commander;
using ExcelAnalysisTools.Aspects;
using Microsoft.Practices.ServiceLocation;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelAnalysisTools.ViewModel
{
    [ImplementPropertyChanged]
    public class ShellViewModel
    {
        private readonly IServiceLocator _serviceLocator;

        public ShellViewModel(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        PrimaryProcessingsViewModel _primaryProcessings;
        OptionsViewModel _options;
        ProfileViewModel _profile;
        RegexListViewModel _regex;
        AddressListViewModel _addreses;

        public PrimaryProcessingsViewModel PrimaryProcessings { get { return GetIns(ref _primaryProcessings); } }
        public OptionsViewModel Options { get { return GetIns(ref _options); } }
        public ProfileViewModel Profile { get { return GetIns(ref _profile); } }
        public RegexListViewModel Regex { get { return GetIns(ref _regex); } }
        public AddressListViewModel Addreses { get { return GetIns(ref _addreses); } }

        private T GetIns<T>(ref T obj)
        {
            if (obj == null)
                return (obj = _serviceLocator.GetInstance<T>());
            else
                return obj;
        }
    }

    
}
