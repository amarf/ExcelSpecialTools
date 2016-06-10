using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Core.Interfaces
{
    public interface IPaneManager<PaneType>
    {
        PaneType CreateCustomTaskPane<View, ViewModel>(string Header) where View : FrameworkElement;
    }
}
