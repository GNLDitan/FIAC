using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FlightInspectionAndCalibrationGroup.Model;
using FlightInspectionAndCalibrationGroup.Database.User;
using FlightInspectionAndCalibrationGroup.Model.Airports;

namespace FlightInspectionAndCalibrationGroup
{
    public partial class Form1 : Form
    {
        //string connectionString = @"Data Source=DESKTOP-TEHRS41\SQL2014;Initial Catalog=FICC_DB;Integrated Security=True";
        string connectionString = Database.ConnectionString.GetConnectionString();
        UserService _UserService;
        UserModel _User = new UserModel();
        BindingList<UserModelProperties> UserProperty = new BindingList<UserModelProperties>();
        List<Airports> airports = new List<Airports>();
        Dictionary<int, string> comboBoxItems = new Dictionary<int, string>();
        Airports _airport = new Airports();

        public Form1()
        {
            InitializeComponent();
            InitializeBindings();
            _UserService = new UserService(connectionString);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            airports = _UserService.getAirportList();
            foreach(Airports airport in airports)
            {
                comboBoxItems.Add(airport.Id, airport.Description);
            }
            cbAirports.DataSource = new BindingSource(comboBoxItems, null);
            cbAirports.DisplayMember = "Value";
            cbAirports.ValueMember = "Key";
        }


        public void InitializeBindings()
        {
            UserProperty.Add(new UserModelProperties() { Id = 0, Password = string.Empty });
            txtUserName.DataBindings.Add("Text", UserProperty[0], "UserName");
            txtPassword.DataBindings.Add("Text", UserProperty[0], "Password");
            lbPrompt.DataBindings.Add("Text", UserProperty[0], "PromptError");
        }


        private void button1_Click(object sender, EventArgs e)
        {
            UserProperty[0].PromptError = string.Empty;
            var ModelList = _UserService.getUserByUserName(UserProperty[0].UserName);
            _airport = airports[cbAirports.SelectedIndex];
            string Prompt = string.Empty;
            if (ModelList.Count > 0)
            {
                _User = ModelList[0];

                if (_User.Id == 0)
                    Prompt = "User doesnt exists";
                else
                {
                    if (_User.Password != UserProperty[0].Password)
                        Prompt = "Invalid Password";
                }
            }
            else {
                if (string.IsNullOrEmpty(UserProperty[0].UserName))
                {
                    Prompt = "You must be fillup Username";
                }
                else Prompt = "Invalid Password";

            }

            if (string.IsNullOrEmpty(Prompt))
            {
                Home hm = new Home(_User, _airport);
                this.Hide();
                hm.ShowDialog();
            }
            else MessageBox.Show(Prompt, "Error");
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            CreateNewUser cr = new CreateNewUser(_User);
            cr.ShowDialog();
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode== Keys.Enter)
            {
                button1_Click(sender, new EventArgs());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
