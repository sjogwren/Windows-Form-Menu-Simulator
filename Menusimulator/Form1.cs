using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Menusimulator
{
    public partial class Form1 : Form
    {
        private List<Useroptions> _useroptions;
        DataTable dt2 = new DataTable();

        private Useroptions currentUseroption;
        private int useroption = 0;

        public Form1()
        {
            InitializeComponent();

        }
        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string menuoption = textBox2.Text;

            if (textBox1.Text == "")
            {
                MessageBox.Show("Please Input a Username", "ERROR", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("Please Input a Menu Option", "ERROR", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
            else
            {
                var dir = AppDomain.CurrentDomain.BaseDirectory;
                _useroptions = new List<Useroptions>();
                _useroptions.Add(new Useroptions() { Username = username, Menuitem = menuoption });
                XmlDocument doc = new XmlDocument();
                doc = new XmlDocument();
                doc.Load(dir + "\\Employee.xml");

                if (File.Exists(dir + "\\Employee.xml"))
                {
                    var file = dir + "Employee.xml";
                    if (file == (dir + "Employee.xml"))
                    {

                        doc.Load(dir + "Employee.xml");
                        XmlDocument nd = new XmlDocument();
                        nd.LoadXml(doc.InnerXml);
                        XmlNode nl = doc.SelectSingleNode("//user");
                        XmlDocument xd2 = new XmlDocument();
                        xd2.LoadXml("<newuser><username>" + username + "</username><menuoption>" + menuoption + "</menuoption></newuser>");
                        XmlNode n = doc.ImportNode(xd2.FirstChild, true);
                        nl.AppendChild(n);
                        doc.Save(dir + "Employee.xml");
                    }

                    string[] lines = File.ReadLines(dir + "menus.txt").ToArray();
                    string search = ", ";
                    List<string> menuitems = new List<string>();

                    foreach (var item in lines)
                    {
                        string result = item.Substring(item.IndexOf(search) + search.Length);
                        menuitems.Add(result);
                    }

                    List<string> vs = new List<string>();
                    XElement root = XElement.Load(dir + "Employee.xml");
                    var selection = from subject in root.Descendants()
                                    where subject.Name.LocalName.Contains("newuser")
                                    select new Useroptions()
                                    {
                                        Username = subject.Element("username").Value,
                                        Menuitem = subject.Element("menuoption").Value
                                    };
                    List<Useroptions> chars = new List<Useroptions>();
                    List<users> ul = new List<users>();
                    var UseroptionsViewModel = new users();
                    var OptionsModel = new OptionsModel();
                    OptionsModel.UseroptionsViewModel = new List<users>();
                    foreach (var item in selection)
                    {
                        UseroptionsViewModel = ToSubscriptFormula(item.Menuitem, item.Username, menuitems);
                        var c = UseroptionsViewModel.menuItems.GroupBy(z => z.Username)
                             .Select(group => new { Username = group.Key, Items = group.ToList() })
                             .ToList();

                        foreach (var x in c)
                        {
                            OptionsModel.UseroptionsViewModel.Add(new users()
                            {
                                Username = x.Username,
                                menuItems = x.Items
                            });
                        }
                    }



                    MessageBox.Show("Successfully added new user", "SUCEESS", MessageBoxButtons.OK);
                    textBox1.Text = "";
                    textBox2.Text = "";
                }
            }
        }

        public users ToSubscriptFormula(string input, string Username, List<string> MenuItems)
        {
            input = input.Replace(" ", "");
            var characters = input.ToArray();
            String result = "";
            int i = 0;
            foreach (var c in input)
            {
                if (characters[i] == 'Y')
                {
                    result += i.ToString();
                }
                else if (characters[i] == 'N') result += "N";
                i++;
            }


            var list = result.ToArray();
            users Useroptions = new users();
            Useroptions.menuItems = new List<Useroptions>();
            ListItemViewModel lvm = new ListItemViewModel();
            lvm.ListItemsList = new List<string>();
            foreach (var x in list)
            {
                lvm.ListItemsList.Add(x.ToString());
            }
            lvm.ListItemsList = (from x in lvm.ListItemsList
                                 where x != "N"
                                 select x).ToList();
            foreach (var c in lvm.ListItemsList)
            {
                int l = Convert.ToInt16(c);
                foreach (var item in MenuItems.ToList()[l])
                {
                    if (l == Convert.ToInt16(c))
                    {
                        Useroptions u = new Useroptions();
                        u.Username = Username;
                        u.Menuitem = MenuItems.ToList()[l];

                        Useroptions.menuItems.Add(u);
                        break;
                    }
                }
                i++;
            }

            Root r = new Root();
            User user = new User();
            user.menuItems = new List<string>();
            r.users = new List<User>();
            user.userName = Username;
            foreach (var item in Useroptions.menuItems)
            {
                user.menuItems.Add(item.Menuitem);
            }
            r.users.Add(user);
            var dir = AppDomain.CurrentDomain.BaseDirectory;


            // this is of datatype string
            var json =  JsonConvert.SerializeObject(r.users, Newtonsoft.Json.Formatting.Indented);
            //var newStr = json.Substring(1, json.Length - 1);
         
            //newStr = newStr.Replace("}]", "},");


            using (System.IO.StreamWriter sw = System.IO.File.AppendText(dir+"\\Output.json"))
            {
                sw.WriteLine(json);
            }

            
            return Useroptions;

        }
    }


}
