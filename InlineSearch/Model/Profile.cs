using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlineSearch.Model
{
    public class Profile
    {
        public string ProfileName { get; set; }
        public string SheetName { get; set; }
        public int StartRow { get; set; }
        public int EndRow { get; set; }
        public bool IsTarget { get; set; } = false; //по умолчанию профиль является источником


        public ObservableCollection<KeyItem> Keys { get; set; }



        public Profile()
        {
            Keys = new ObservableCollection<KeyItem>();
        }

    }
}
