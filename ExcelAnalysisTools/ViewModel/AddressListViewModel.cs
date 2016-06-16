﻿using Commander;
using Core.Interfaces;
using ExcelAnalysisTools.Model;
using ExcelAnalysisTools.Services;
using Microsoft.Practices.ServiceLocation;
using PropertyChanged;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ExcelAnalysisTools.ViewModel
{
    [ImplementPropertyChanged]
    public class AddressListViewModel
    {
        private readonly IServiceLocator _serviceLocator;
        private readonly IUserMsgService _userMsgService;
        private readonly Repository _repository;

        public ICollectionView Items { get; private set; }
        public string FindText { get; set; } = "";
        private CollectionViewSource CVS;

        public AddressListViewModel(IServiceLocator serviceLocator, IUserMsgService userMsgService, Repository repository)
        {
            _serviceLocator = serviceLocator;
            _userMsgService = userMsgService;
            _repository = repository;

            createView();

            (this as INotifyPropertyChanged).PropertyChanged += (obj, args) => { if (args.PropertyName == nameof(FindText)) Items?.Refresh(); };
            (repository as INotifyPropertyChanged).PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "AddressList")
                    createView();
            };
        }

        private void createView()
        {
            CVS = new CollectionViewSource();
            CVS.Source = _repository.AddressList.Items;
            CVS.View.Filter = FilterMethod;
            Items = CVS.View;
        }


        private bool FilterMethod(object obj)
        {
            if (string.IsNullOrWhiteSpace(FindText)) return true;

            var findLine = (obj as AddressModel)?.Address?.ToLower();
            var mass = FindText.ToLower().Split(' ', ',', '.');
            foreach (var substring in mass)
                if (!findLine.Contains(substring)) return false;
            return true;
        }

        [OnCommand("SaveListCommand")]
        private void SaveList()
        {
            
        }
    }
}
