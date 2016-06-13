using Core.Interfaces;
using ExcelAnalysisTools.WfHosts;
using ExcelDna.Integration.CustomUI;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExcelAnalysisTools.XServices
{
    public class ExcelTaskPaneManager: IPaneManager<CustomTaskPane>
    {
        private readonly IServiceLocator _serviceLocator;

        public ExcelTaskPaneManager(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public CustomTaskPane CreateCustomTaskPane<View, ViewModel>(string Header) where View: FrameworkElement
        {
            var ctPane = CustomTaskPaneFactory.CreateCustomTaskPane(typeof(HostToolsPane), Header);
            var view = _serviceLocator.GetInstance<View>();
            var viewModel = _serviceLocator.GetInstance<ViewModel>();
            view.DataContext = viewModel;
            (ctPane.ContentControl as HostToolsPane).Host.Child = view;
            //ctPane.Visible = true;
            //ctPane.VisibleStateChange += CtPane_VisibleStateChange;
            ctPane.DockPosition = MsoCTPDockPosition.msoCTPDockPositionFloating;
            ctPane.Width = 400;
            ctPane.Height = 300;
            return ctPane;
        }
    }
}
