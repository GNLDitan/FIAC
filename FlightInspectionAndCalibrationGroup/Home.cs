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
using FlightInspectionAndCalibrationGroup.Database;
using FlightInspectionAndCalibrationGroup.Database.Home;
using FlightInspectionAndCalibrationGroup.Model.Airports;

namespace FlightInspectionAndCalibrationGroup
{
    public partial class Home : Form
    {
        UserModel _user = new UserModel();
        Airports _activeAirport = new Airports();
        BindingSource bs = new BindingSource();
        HomeService service;
        BindingList<UserModelProperties> UserProperty = new BindingList<UserModelProperties>();
        private enum RecordOperation
        {
            New,
            Refresh,
            Save,
            Delete
        }

        public Home()
        {
            InitializeComponent();
        }
        public Home(UserModel logInUser, Airports airport)
        {
            InitializeComponent();
            _activeAirport = airport;
            _user = logInUser;
        }
        private void Home_Load(object sender, EventArgs e)
        {
            #region Events
            btnRefresh.Click += RecordOperationEvents;
            btnNew.Click += RecordOperationEvents;
            btnSave.Click += RecordOperationEvents;
            btnDelete.Click += RecordOperationEvents;
            #endregion
            lbTitle.Text = string.Format(@"Flight Inspection and Calibration Group [{0}]", _activeAirport.Description);
            service = new HomeService();
            InitializeGridControls();
            InitializeBindings();
            textBox3.Text = "0";
            InitializeUser(this._user);
            
        }
        
        private void RecordOperationEvents(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button.Name== btnRefresh.Name)
                RecordOperationHandler(RecordOperation.Refresh);
            else if(button.Name == btnNew.Name)
                RecordOperationHandler(RecordOperation.New);
            else if (button.Name == btnSave.Name)
                RecordOperationHandler(RecordOperation.Save);
            else if (button.Name == btnDelete.Name)
                RecordOperationHandler(RecordOperation.Delete);
        }

        public void InitializeBindings()
        {
            UserProperty.Add(new UserModelProperties() { Id = 0, Password = string.Empty });
            txtUsername.DataBindings.Add("Text", UserProperty[0], "UserName");
            txtName.DataBindings.Add("Text", UserProperty[0], "Name");
            txtContactNo.DataBindings.Add("Text", UserProperty[0], "ContactNo");
            pictureBox1.DataBindings.Add("ImageLocation", UserProperty[0], "ImageLocation");

        }
        private bool RecordOperationHandler(RecordOperation e)
        {
            switch (e)
            {
                case RecordOperation.Refresh:
                    RefreshData();
                    break;
                case RecordOperation.New:
                    ClearFields();
                    break;
                case RecordOperation.Save:
                    string validation = AreValidEntries();
                    string currdate = ConnectionString.GetCurrentServerDate().ToString();
                    //bool isNew = textBox3.Text == "0" ? true : false;
                    bool isNew = true;
                    if (string.IsNullOrEmpty(validation))
                    {
                        service.Save(textBox3.Text, comboBox1.Text, textBox2.Text,
                        dateTimePicker1.Value, dateTimePicker2.Value, textBox1.Text, _user.Id.ToString(), currdate, _user.Id.ToString(), currdate, _activeAirport.Id,isNew);
                        MessageBox.Show(string.Format(@"Equipment {0} was saved successfully", textBox2.Text), "Success");
                        RefreshData();
                    }
                    else MessageBox.Show(validation,"Error");
                    
                    break;
                default:
                    break;
            }
            return true;
        }

        public string AreValidEntries()
        {
            string validation = string.Empty;

            if(string.IsNullOrEmpty(comboBox1.Text))
            {
                validation += "Cannot save. Station is required.\n";
            }
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                validation += "Cannot save. Equipment is required.\n";
            }

            return validation;
            

        }

       public void InitializeGridControls()
        {
            dgv_Details.BorderStyle = BorderStyle.None;
            dgv_Details.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            dgv_Details.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv_Details.DefaultCellStyle.SelectionBackColor = Color.LightGray;
            dgv_Details.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv_Details.BackgroundColor = Color.White;
            dgv_Details.EnableHeadersVisualStyles = false;
            dgv_Details.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv_Details.AllowUserToAddRows = false;
        }

        private void RefreshData()
        {
            ClearFields();
            DataTable dt = service.GetData(_activeAirport.Id);
            LoadAndBindToGrid(ref dgv_Details, ref dt,ref bs);
        }
        private void LoadAndBindToGrid(ref DataGridView gv, ref DataTable dt, ref BindingSource bs)
        {
            gv.DataSource = null;
            gv.AutoGenerateColumns = false;

            //start binding
            bs = new BindingSource();
            bs.DataSource = dt;
            gv.DataSource = bs;
            //end binding

            foreach (DataColumn item in dt.Columns)
                gv.Columns[item.ColumnName].DataPropertyName = item.ColumnName;


            this.BeginInvoke(new MethodInvoker(SetStatusColor));

        }

        private void SetStatusColor()
        {
            for (int i = 0; i < dgv_Details.Rows.Count; i++)
            {
                dgv_Details[col_Status.Index, i].Style.BackColor = getBGColor(DateTime.Parse(dgv_Details[Col_DueForFlightCheck.Index, i].Value.ToString()));
                dgv_Details[col_Status.Index, i].Style.SelectionBackColor = getBGColor(DateTime.Parse(dgv_Details[Col_DueForFlightCheck.Index, i].Value.ToString()));
            }
        }

        public Color getBGColor(DateTime DueDate)
        {
            var CurrentDate = DateTime.Parse(Database.ConnectionString.GetCurrentServerDate().ToString("dd/MM/yyyy"));
            var ShouldBeDue = DateTime.Parse(CurrentDate.AddMonths(1).ToString("dd/MM/yyyy"));
            var MonthLapseDue = DateTime.Parse(CurrentDate.AddMonths(-1).ToString("dd/MM/yyyy"));
            var NoTimeDueDate = DateTime.Parse(DueDate.ToString("dd/MM/yyyy"));
            Color returnColor = new Color();

           
             if (DueDate > CurrentDate && NoTimeDueDate < ShouldBeDue)//month before
            {
                returnColor = Color.Yellow;
            }

            else if(NoTimeDueDate >= CurrentDate)
            {
                returnColor = Color.Green;
            }

            else if (CurrentDate > DueDate && MonthLapseDue <= NoTimeDueDate) //Due
            {
                returnColor = Color.Orange;
            }

            else if (MonthLapseDue <= CurrentDate && NoTimeDueDate < CurrentDate) //Due
            {
                returnColor = Color.Crimson;
            }
            return returnColor;
        }

        private void ClearFields()
        {
            comboBox1.Text = string.Empty;
            textBox3.Text = 0.ToString() ;
            textBox1.Clear();
            textBox2.Clear();
            dateTimePicker1.Value = ConnectionString.GetCurrentServerDate();
            dateTimePicker2.Value = ConnectionString.GetCurrentServerDate();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void dgv_Details_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            
            
        }

        private void dgv_Details_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                comboBox1.Text = dgv_Details[Col_Station.Index, e.RowIndex].Value.ToString();
                textBox2.Text = dgv_Details[Col_Equipment.Index, e.RowIndex].Value.ToString();
                dateTimePicker1.Value = DateTime.Parse(dgv_Details[Col_DateCheck.Index, e.RowIndex].Value.ToString());
                dateTimePicker2.Value = DateTime.Parse(dgv_Details[Col_DueForFlightCheck.Index, e.RowIndex].Value.ToString());
                textBox1.Text= dgv_Details[Col_Remarks.Index, e.RowIndex].Value.ToString();
                textBox3.Text = dgv_Details[Col_Id.Index, e.RowIndex].Value.ToString();
            }
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
          
            this.Cursor = Cursors.Hand;
            panel17.Visible = true;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Arrow;
            panel17.Visible = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            CreateNewUser user = new CreateNewUser(_user);
            user.ShowDialog();

            InitializeUser(user._User);

        }

        private void InitializeUser(UserModel user)
        {

            UserProperty[0].Name = user.Name;
            UserProperty[0].userName = user.UserName;
            UserProperty[0].ContactNo = user.ContactNo;
            UserProperty[0].ImageLocation = Application.StartupPath+user.ImageLocation;

            txtUsername.Text= user.UserName;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();

            this.Hide();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SummaryReport sm = new SummaryReport(_activeAirport);
            sm.ShowDialog();
        }
        
        private void panel19_DoubleClick(object sender, EventArgs e)
        {
            this.WindowState = this.WindowState == FormWindowState.Maximized ? FormWindowState.Normal : FormWindowState.Maximized;
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar))
            {
                e.KeyChar = char.ToUpper(e.KeyChar);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ClearFields();
            DataTable dt = service.GetDataByStation(txtSearch.Text, _activeAirport.Id);
            LoadAndBindToGrid(ref dgv_Details, ref dt, ref bs);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dg = new DialogResult();
            dg = MessageBox.Show(string.Format("Are you sure you want to delete the equipment {0} ?", textBox2.Text), "Delete", MessageBoxButtons.YesNo);
            if (dg == DialogResult.Yes)
            {
                service.Delete(textBox3.Text);
                RefreshData();
                MessageBox.Show("Deleted Successfuly");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)bs.DataSource;

            if (dt != null && dt.Columns.Count > 0)
            {
                string rowFilter = "[Col_Station] like '%" + txtSearch.Text.Replace("'", "''") + "%'";
                dt.DefaultView.RowFilter = rowFilter;

                this.BeginInvoke(new MethodInvoker(SetStatusColor));
            }
        }
    }
}
