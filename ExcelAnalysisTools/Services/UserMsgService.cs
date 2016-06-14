using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelAnalysisTools.Services
{
    public class UserMsgService : IUserMsgService
    {
        public void MsgShow(string msg, string Header = null)
        {
            System.Windows.MessageBox.Show(msg);
        }
    }
}
