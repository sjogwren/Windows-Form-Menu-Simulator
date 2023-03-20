using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Menusimulator
{
    public class OptionsModel
    {
        public List<users> UseroptionsViewModel { get; set; }
    }
    public class users
    {
        public string Username { get; set; }
        public List<Useroptions> menuItems { get; set; }
    }
    public class Useroptions
    {
        public string Username { get; set; }
        public string Menuitem { get; set; }
    }
    public class ListItemViewModel
    {
        public List<string> ListItemsList { get; set; }
    }

    public class Root
    {
        public List<User> users { get; set; }
    }
    public class User
    {
        public string userName { get; set; }
        public List<string> menuItems { get; set; }
    }


}
