using FlightInspectionAndCalibrationGroup.Database.Reporting;
using FlightInspectionAndCalibrationGroup.Model;
using FlightInspectionAndCalibrationGroup.Model.Airports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FlightInspectionAndCalibrationGroup
{
    public partial class SummaryReport : Form
    {
        SummaryReportService _summaryReport;
        DataTable dt = new DataTable();
        List<EquipmentModel> SummaryReportList = new List<EquipmentModel>();
        List<string> Stations = new List<string>();
        int MaxCount = 0;
        Airports _airport = new Airports();

        public SummaryReport()
        {
            _summaryReport = new SummaryReportService();
            InitializeComponent();
        }

        public SummaryReport(Airports airport)
        {
            _airport = airport;
            _summaryReport = new SummaryReportService();
            InitializeComponent();
        }

        private void SummaryReport_Load(object sender, EventArgs e)
        {
           var equipments = _summaryReport.getEquipments(_airport.Id);
           SummaryReportList = equipments;
           loadReportLists();
           InitializeGridControls();
           loadGridComponents();
           LoadGridDetails();
        }

        private void loadReportLists()
        {
            Stations = SummaryReportList.Select(sr => sr.StationDesc).Distinct().ToList();
            Stations.ForEach(st =>
            {
               var count = SummaryReportList.FindAll(sr => sr.StationDesc == st).Count();
                if (count > MaxCount)
                    MaxCount = count;
            });
        }

        public void loadGridComponents()
        {
            int cnt = 1;
            gv_Report.Columns.Add("Col_Station", "Station");
            while(cnt <= MaxCount)
            {
                string colName = String.Format(@"Col_{0}", cnt);
                string colHeader = String.Format(@"Equipment_{0}", cnt);
                gv_Report.Columns.Add(colName, colHeader);
                cnt++;
            }
            gv_Report.Rows.Add();
        }

        public void LoadGridDetails()
        {
            int row = 0;
            int col = 0;
            foreach(string station in Stations)
            {
                gv_Report.Rows[row].Cells[col].Value = station;

                foreach(EquipmentModel equipment in SummaryReportList.FindAll(sr => sr.StationDesc == station).ToList())
                {
                    col++;
                    gv_Report.Rows[row].Cells[col].Value = equipment.EquipmentDesc;
                    gv_Report[col, row].Style.BackColor = getBGColor(equipment.CheckOutDate);

                }
                row++;
                col = 0;
                gv_Report.Rows.Add(); 
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

            else if (NoTimeDueDate >= CurrentDate)
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

        public void InitializeGridControls()
        {
            gv_Report.BorderStyle = BorderStyle.None;
            gv_Report.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            gv_Report.RowTemplate.Height = 30;
            gv_Report.BackgroundColor = Color.White;
            gv_Report.EnableHeadersVisualStyles = false;
            gv_Report.ColumnHeadersHeight = 30;
            gv_Report.AllowUserToAddRows = false;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
