using Commander;
using Core.Interfaces;
using ExcelAnalysisTools.Model;
using ExcelAnalysisTools.Services;
using Microsoft.Practices.ServiceLocation;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ExcelAnalysisTools.ViewModel
{
    [ImplementPropertyChanged]
    public class RegexListViewModel
    {
        private readonly IServiceLocator _serviceLocator;
        private readonly IUserMsgService _userMsgService;
        private readonly Repository _repository;
        private CollectionViewSource CVS;

        public RegexListViewModel(IServiceLocator serviceLocator, IUserMsgService userMsgService, Repository repository)
        {
            _serviceLocator = serviceLocator;
            _userMsgService = userMsgService;
            _repository = repository;

            SetItems();
            (repository as INotifyPropertyChanged).PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "RegexList")
                    SetItems();
            };
        }

        private void SetItems()
        {
            CVS = new CollectionViewSource();
            CVS.Source = privateItems = _repository.RegexList.Items;
            CVS.View.SortDescriptions.Add(new SortDescription { Direction = ListSortDirection.Ascending, PropertyName = nameof(RegexReplaceExpression.Order) });
            Items = CVS.View;
        }

        private ObservableCollection<RegexReplaceExpression> privateItems;
        public ICollectionView Items { get; set; }


        [OnCommand("SaveRegexListCommand")]
        private void SaveRegexList()
        {
            _repository.Save<RegexExpressionList>();
        }

        [OnCommand("AddNewregexCommand")]
        private void AddNewregex()
        {
            privateItems.Add(new RegexReplaceExpression
            {
                Order = privateItems.Max(i=>i.Order) + 1
            });
        }

        [OnCommand("RemovePaternCommand")]
        private void RemovePatern(RegexReplaceExpression patern)
        {
            privateItems.Remove(patern);
        }
        [OnCommand("ReloadRegexListCommand")]
        private void ReloadRegexList()
        {
            _repository.Load<RegexExpressionList>(_repository.Options.GetDataPath<RegexExpressionList>());
        }

    }
}
