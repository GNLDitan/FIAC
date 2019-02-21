
using FlightInspectionAndCalibrationGroup.Model;
using FlightInspectionAndCalibrationGroup.Model.Airports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace FlightInspectionAndCalibrationGroup.Database.User
{
    public class UserService
    {
      
        private readonly string _connectionString;
        DataTable data;
        public UserService(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            _connectionString = connectionString;
        }

        public List<UserModel> getUserByUserName(string UserName)
        {
            data = new DataTable();
            SqlConnection cn = new SqlConnection(_connectionString);
            SqlCommand cm = new SqlCommand("[FI].[sp_GetUserByUserName]", cn);
            cm.CommandType = CommandType.StoredProcedure;
            cm.Parameters.Add("@UserName", SqlDbType.Text).Value = UserName;
            cn.Open();
            data.Load(cm.ExecuteReader());
            cn.Close();
            return Dal.CreateListFromTable<UserModel>(data);
        }


        public List<Airports> getAirportList()
        {
            data = new DataTable();
            SqlConnection cn = new SqlConnection(_connectionString);
            SqlCommand cm = new SqlCommand("[FI].[nsp_Airports]", cn);
            cm.CommandType = CommandType.StoredProcedure;
            cn.Open();
            data.Load(cm.ExecuteReader());
            cn.Close();
            return Dal.CreateListFromTable<Airports>(data);
        }


        public List<UserModel> SaveUser(UserModel createUser, string imagepath)
        {
            data = new DataTable();
            SqlConnection cn = new SqlConnection(ConnectionString.GetConnectionString());
            SqlCommand cm = new SqlCommand("[FI].[sp_CreateUser]", cn);
            cm.CommandType = CommandType.StoredProcedure;
            cm.Parameters.AddWithValue("@Id", createUser.Id);
            cm.Parameters.AddWithValue("@Name", createUser.Name);
            cm.Parameters.AddWithValue("@UserName", createUser.UserName);
            cm.Parameters.AddWithValue("@Password", createUser.Password);
            cm.Parameters.AddWithValue("@contactNo", createUser.ContactNo);
            cm.Parameters.AddWithValue("@Imagepath", imagepath);
            cm.Parameters.AddWithValue("@Image", createUser.Image);
            cn.Open();
            data.Load(cm.ExecuteReader());
            cn.Close();
            return Dal.CreateListFromTable<UserModel>(data);
        }


    }
}
