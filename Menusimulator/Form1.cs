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
        private List<User> _useroptions;
        DataTable dt2 = new DataTable();

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
                _useroptions = new List<User>();
                _useroptions.Add(new User() { Username = username, MenuItem = menuoption });
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
                                    select new User()
                                    {
                                        Username = subject.Element("username").Value,
                                        MenuItem = subject.Element("menuoption").Value
                                    };
                    List<users> chars = new List<users>();
                    List<users> ul = new List<users>();
                    var OptionsModel = new OptionsModel();
                    OptionsModel.UseroptionsViewModel = new List<users>();
                    foreach (var item in selection)
                    {
                        OptionsModel.UseroptionsViewModel.Add(ToSubscriptFormula(item.MenuItem, item.Username, menuitems));



                    }

                    // this is of datatype string
                    var json = JsonConvert.SerializeObject(new { users = OptionsModel.UseroptionsViewModel }, Newtonsoft.Json.Formatting.Indented);
                    //var newStr = json.Substring(1, json.Length - 1);

                    //newStr = newStr.Replace("}]", "},");


                    using (System.IO.StreamWriter sw = System.IO.File.AppendText(dir + "\\Output.json"))
                    {
                        sw.WriteLine(json);
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
            Useroptions.menuItems = new List<string>();
            ListItemViewModel lvm = new ListItemViewModel();
            lvm.ListItemsList = new List<string>();
            foreach (var x in list)
            {
                lvm.ListItemsList.Add(x.ToString());
            }
            lvm.ListItemsList = (from x in lvm.ListItemsList
                                 where x != "N"
                                 select x).ToList();
            users uu = new users();
            uu.menuItems = new List<string>();

            foreach (var c in lvm.ListItemsList)
            {
                int l = Convert.ToInt16(c);
                foreach (var item in MenuItems.ToList()[l])
                {
                    if (l == Convert.ToInt16(c))
                    {
                        uu.menuItems.Add(MenuItems.ToList()[l]);
                        break;
                    }
                }
                i++;
            }
            users u = new users();
            foreach (var c in lvm.ListItemsList)
            {
                int l = Convert.ToInt16(c);
                foreach (var item in MenuItems.ToList()[l])
                {
                    if (l == Convert.ToInt16(c))
                    {
    
                        u.Username = Username;
                        u.menuItems = uu.menuItems;
                        break;
                    }
                }
                i++;
            }

           

            
            return u;

        }
    }


}
