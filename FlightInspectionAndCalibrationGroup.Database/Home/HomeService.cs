using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace FlightInspectionAndCalibrationGroup.Database.Home
{
	public class HomeService
	{
		private readonly string _connectionString;
		private const string procName = "[FI].[nsp_CheckStatusBoard]";
		public HomeService()
		{
			string conStr = ConnectionString.GetConnectionString();
			if (string.IsNullOrWhiteSpace(conStr))
			{
				throw new ArgumentNullException("connectionString");
			}
			_connectionString= conStr;
		}

		public DataTable GetData(int AirportId)
		{
			DataTable dt = new DataTable();
			SqlCommand cmd = new SqlCommand();

			cmd.CommandText = procName;
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@querytype", 0);
            cmd.Parameters.AddWithValue("@AirportId", AirportId);
            return Procedure.GetData(cmd,_connectionString);
		}

        public DataTable GetDataByStation(string station, int AirportId)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = procName;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@querytype", 4);
            cmd.Parameters.AddWithValue("@StationDesc", station);
            cmd.Parameters.AddWithValue("@AirportId", AirportId);
            return Procedure.GetData(cmd, _connectionString);
        }

        public bool Save(string id, string station, string equipment, DateTime checkInDate, DateTime checkOutDate,
							string remarks, string recuser, string recdate, string moduser, string moddate, int aiportId,bool isNew)
		{
			SqlCommand cmd = new SqlCommand();

			cmd.CommandText = procName;
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@id", id);
			cmd.Parameters.AddWithValue("@stationdesc", station);
			cmd.Parameters.AddWithValue("@equipmentdesc", equipment);
			cmd.Parameters.AddWithValue("@CheckInDate", checkInDate);
			cmd.Parameters.AddWithValue("@CheckOutDate", checkOutDate);
			cmd.Parameters.AddWithValue("@Remarks", remarks);
			cmd.Parameters.AddWithValue("@CreatedBy", recuser);
			cmd.Parameters.AddWithValue("@ModifiedBy", moduser);
            cmd.Parameters.AddWithValue("@AirportId", aiportId);

            cmd.Parameters.AddWithValue("@querytype", isNew?1:2);


			return Procedure.ExecProcedure(cmd,_connectionString);
		}

		public bool Delete(string id)
		{
			SqlCommand cmd = new SqlCommand();

			cmd.CommandText = procName;
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@id", id);
			cmd.Parameters.AddWithValue("@querytype", 3);

			return Procedure.ExecProcedure(cmd, _connectionString);
		}
	}
}
