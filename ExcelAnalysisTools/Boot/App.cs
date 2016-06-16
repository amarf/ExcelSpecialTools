using ExcelAnalysisTools.WfHosts;
using ExcelDna.Integration.CustomUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Prism.StructureMap;
using StructureMap;
using ExcelAnalysisTools.View;
using Core.Interfaces;
using ExcelAnalysisTools.ViewModel;
using System.Diagnostics;
using System.Collections;
using System.Windows.Controls.Primitives;
using ExcelDna.Integration.Extensibility;

namespace ExcelAnalysisTools.Boot.Ribbon
{
    [ComVisible(true)]
    public class App: ExcelRibbon //Это точка входа в приложение //по идеии для каждого окна создается новая панель
    {
        public static void Main(string[] args) { /*точка входа в приложение*/}

        private readonly IContainer _container;


        public App()
        {
            var b = new Bootstrapper();
            b.Run();
            _container = b.Container;
        }



        public Hashtable PanelHash { get; set; } = new Hashtable();

        public void OpenToolPanelCommand(IRibbonControl control, bool state)
        {
            var uniCod = (control.Context as dynamic).Hwnd;
            var pane = PanelHash[uniCod] as CustomTaskPane;
            if (pane == null)
            {
                var paneManager = _container.GetInstance<IPaneManager<CustomTaskPane>>();
               // var ctPane = paneManager.CreateCustomTaskPane<PrimaryProcessingsView, PrimaryProcessingsViewModel>("Панел инструментов");
                var ctPane = paneManager.CreateCustomTaskPane<ToolsShell, ShellViewModel>("Панель инструментов");
                ctPane.Visible = true;

                PanelHash[uniCod] = ctPane;

                ctPane.VisibleStateChange += CustomTaskPane =>
                {
                    _customRibbonUI?.InvalidateControl("toggle_openToolPanel"); //выполняет валидацию контрола (всех коппий)
                };
            }
            else
            {
                pane.Visible = state;
            }
        }


        private IRibbonUI _customRibbonUI;
        public void OnLoadCustomUI(IRibbonUI obj) => _customRibbonUI = obj; //собитие создания риббон см. ExcelAnalysisTools.dna


        public bool ValidateIsPressed(IRibbonControl control)
        {
            var uniCod = (control.Context as dynamic).Hwnd;
            if (PanelHash.ContainsKey(uniCod))
            {
                var pane = PanelHash[uniCod] as CustomTaskPane;
                return pane.Visible;
            }
            else
            {
                return false;
            }
        }



        //http://stackoverflow.com/questions/36756227/how-do-i-get-this-excel-dna-wpf-custom-task-pane-to-not-eat-scrollwheel-events

        public override void OnDisconnection(ext_DisconnectMode RemoveMode, ref Array custom)
        {
            base.OnDisconnection(RemoveMode, ref custom);
        }
    }
}
