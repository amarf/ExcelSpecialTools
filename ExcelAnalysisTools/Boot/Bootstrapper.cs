using Core.Interfaces;
using ExcelAnalysisTools.XServices;
using ExcelDna.Integration;
using ExcelDna.Integration.CustomUI;
using Microsoft.Office.Interop.Excel;
using Prism.StructureMap;
using StructureMap.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace ExcelAnalysisTools.Boot
{
    public class Bootstrapper: StructureMapBootstrapper
    {
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();



            
            Container.Configure(r =>
            {
                r.For<IComponentConnector>().OnCreationForAll(s => s.InitializeComponent());
                r.For<IPaneManager<CustomTaskPane>>().Use<ExcelTaskPaneManager>();

                //r.For<IComponentConnector>().OnCreationForAll(s => s.InitializeComponent());
                //r.For<IAppService>().Use<AppService>().Singleton();
                //r.For<IRepository<ProjectRoot>>().Use<Repository>();
                //r.For<IDataService>().Use<DataService>().Singleton();
                //r.For<IDialogCoordinator>().Use<DialogCoordinator>().Singleton();
                //r.For<IFolderBrowserDialog>().Use<FolderBrowserDialog>().Singleton();
                //r.For<IFileBrowserDialog>().Use<FileBrowserDialog>();
                //r.For<IMemoryService>().Use<MemoryService>();
                //r.For<ILogger>().Use<Logger>().Singleton();
                //r.ForConcreteType<MetroDialogSettings>().Configure
                //    .Ctor<string>("AffirmativeButtonText").Is("ЕПТЫ БЛЯ")
                //    .Ctor<string>("NegativeButtonText").Is("НЕТ ТЫ ЧЕ"); //TODO: Localize
            });
        }
    }
}
