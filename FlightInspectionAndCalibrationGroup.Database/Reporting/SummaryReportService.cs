using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightInspectionAndCalibrationGroup.Model;

namespace FlightInspectionAndCalibrationGroup.Database.Reporting
{
    public class SummaryReportService
    {
        private readonly string _connectionString;
        DataTable data;

        public SummaryReportService()
        {
            string conStr = ConnectionString.GetConnectionString();
            if (string.IsNullOrWhiteSpace(conStr))
            {
                throw new ArgumentNullException("connectionString");
            }
            _connectionString = conStr;
        }

        public List<EquipmentModel> getEquipments(int AirportId)
        {
            data = new DataTable();
            SqlConnection cn = new SqlConnection(_connectionString);
            SqlCommand cm = new SqlCommand("[FI].[nsp_GetReport]", cn);
            cm.CommandType = CommandType.StoredProcedure;
            cm.Parameters.Add("@AirportId", SqlDbType.SmallInt).Value = AirportId;
            cn.Open();
            data.Load(cm.ExecuteReader());
            cn.Close();
            return Dal.CreateListFromTable<EquipmentModel>(data);
        }
    }
}
