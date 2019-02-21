using FlightInspectionAndCalibrationGroup.Database.User;
using FlightInspectionAndCalibrationGroup.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlightInspectionAndCalibrationGroup
{
    public partial class CreateNewUser : Form
    {
        string connectionString = Database.ConnectionString.GetConnectionString();
        UserService _UserService;
        public UserModel _User = new UserModel();
        BindingList<UserModelProperties> UserProperty = new BindingList<UserModelProperties>();
        
        public CreateNewUser()
        {
            InitializeComponent();
            InitializeBindings();
            _UserService = new UserService(Database.ConnectionString.GetConnectionString());
        }

        public CreateNewUser(UserModel user)
        {
            InitializeComponent();
            _User = user;
            _UserService = new UserService(Database.ConnectionString.GetConnectionString());
            InitializeBindings();
        }

        private void CreateNewUser_Load(object sender, EventArgs e)
        {
            PopulateControls(false);
        }

        public void InitializeBindings()
        {
            UserProperty.Add(new UserModelProperties() { Id = 0, Password = string.Empty });
            txtUserName.DataBindings.Add("Text", UserProperty[0], "UserName");
            txtPassword.DataBindings.Add("Text", UserProperty[0], "Password");
            txtConfirmPassword.DataBindings.Add("Text", UserProperty[0], "ConfirmPassword");
            txtName.DataBindings.Add("Text", UserProperty[0], "Name");
            txtContact.DataBindings.Add("Text", UserProperty[0],"ContactNo");
            pictureBox1.DataBindings.Add("ImageLocation", UserProperty[0],"ImageLocation");
        }

       public void PopulateControls(bool isSave)
        {
            if (!isSave)
            {
                UserProperty[0].id = _User.Id;
                UserProperty[0].userName = _User.UserName;
                UserProperty[0].password = _User.Password;
                UserProperty[0].ConfirmPassword = _User.ConfirmPassword;
                UserProperty[0].Name = _User.Name;
                UserProperty[0].ContactNo = _User.ContactNo;
                UserProperty[0].ImageLocation = Application.StartupPath+_User.ImageLocation;
            } else
            {
                _User.Id = UserProperty[0].id;
                _User.UserName = UserProperty[0].userName;
                _User.Password = UserProperty[0].password;

                _User.ConfirmPassword = UserProperty[0].ConfirmPassword;
                _User.Name = UserProperty[0].Name;
                _User.ContactNo = UserProperty[0].ContactNo;
                _User.ImageLocation = UserProperty[0].ImageLocation;
            }
            
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpeg;*.bmp;*.png;*.jpg)|*.jpeg;*.bmp;*.png;*.jpg";
            if(open.ShowDialog() == DialogResult.OK)
            {
                lbFileName.Text = open.FileName;
                FileInfo info = new FileInfo(open.FileName);
                string newFilePath = Application.StartupPath + "\\Image\\" + info.Name;
                File.Copy(info.FullName, newFilePath,true);

                UserProperty[0].ImageLocation = newFilePath;
                _User.Image = ConvertFileToByte(newFilePath);
            }
            open.Dispose();
        }


        private byte[] ConvertFileToByte(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, (int)fs.Length);
            fs.Close();
            return buffer;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            PopulateControls(true);
            var userTemp = _User;

            if (ValidateData()!= string.Empty)
            {
                MessageBox.Show(ValidateData());
                return;
            }


            var user = _UserService.SaveUser(userTemp,userTemp.ImageLocation.Replace(Application.StartupPath,""));
            _User = user[0];

            MessageBox.Show("Saved successfully.");

            this.Close();
        }


        string ValidateData()
        {
            string errorMsg = string.Empty;

            if (txtName.Text.Replace("'","''").Trim()== string.Empty)
            {
                errorMsg += "Cannot save. Name is required.\n";
            }

            if (txtUserName.Text.Replace("'", "''").Trim() == string.Empty)
            {
                errorMsg += "Cannot save. Username is required.\n";
            }

            if (txtContact.Text.Replace("'", "''").Trim() == string.Empty)
            {
                errorMsg += "Cannot save. Contact No. is required.\n";
            }
            if (txtPassword.Text.Replace("''", "''").Trim() == string.Empty)
            {
                errorMsg += "Cannot save. Contact No. is required.\n";
            }
            if (txtConfirmPassword.Text.Replace("''", "''").Trim() == string.Empty)
            {
                errorMsg += "Cannot save. Confirm password is required.\n";
            }

            if (pictureBox1.ImageLocation == string.Empty)
            {
                errorMsg += "Cannot save. Image is required.\n";
            }

            if (txtConfirmPassword.Text.Trim().Length>0 && txtPassword.Text.Trim().Length > 0
                && txtConfirmPassword.Text.Trim()!= txtPassword.Text.Trim()
                )
            {
                errorMsg += "Cannot save. Password do not matched.\n";
            }
            return errorMsg;
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
