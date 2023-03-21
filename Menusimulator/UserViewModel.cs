using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Menusimulator
{
    public class User
    {
        public string Username { get; set; }
        public string MenuItem { get; set; }
    }

    public class OptionsModel
    {
        public List<users> UseroptionsViewModel { get; set; }
    }
    [Serializable]
    public class users
    {
        public string Username { get; set; }
        public List<string> menuItems { get; set; }
    }
    public class ListItemViewModel
    {
        public List<string> ListItemsList { get; set; }
    }


}
