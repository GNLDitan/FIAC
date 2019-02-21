using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FlightInspectionAndCalibrationGroup.Database
{
    public class ConnectionString
    {
        public static string _connectionString = "";

        public static string GetConnectionString()
        {
            _connectionString = File.ReadAllText("ConStr.txt");
            return _connectionString;
        }

        public static DateTime GetCurrentServerDate()
        {
            DateTime dateTime = new DateTime();

            DateTime.TryParse(Sql.ReturnScalar("Select getdate()", _connectionString), out dateTime);

            return dateTime;
        }


    }
}
