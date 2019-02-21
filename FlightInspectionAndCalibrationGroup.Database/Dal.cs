using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace FlightInspectionAndCalibrationGroup.Database
{
    public class Sql
    {
        public static string  ReturnScalar(string query,string ConnString)
        {
            string scalarString = string.Empty;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        scalarString= cmd.ExecuteScalar().ToString();
                    }
                }
            }
            catch (Exception)
            {
                scalarString= string.Empty;
            }
            return scalarString;
        }
    }
    public class Procedure
    {
        public static DataTable GetData(SqlCommand cmd, string ConnString)
        {
            DataTable dt = new DataTable();
           
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    cmd.Connection = conn;
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException(ex.Message);
            }
            
            return dt;
        }

        public static bool ExecProcedure(SqlCommand cmd, string ConnString)
        {
            bool handled = false;
            SqlTransaction tran;
            try
            {
                using (SqlConnection conn= new SqlConnection(ConnString))
                {
                    conn.Open();

                    using (tran= conn.BeginTransaction())
                    {
                        cmd.Connection = conn;
                        cmd.Transaction = tran;
                        cmd.ExecuteNonQuery();
                        handled = true;
                        tran.Commit();
                    }

                    
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException(ex.Message);
                tran.Rollback();
            }


            return handled;
        }
    }
    public static class Dal
    {
        public static List<T> CreateListFromTable<T>(DataTable tbl) where T : new()
        {
            List<T> lst = new List<T>();

            foreach(DataRow r in tbl.Rows)
            {
                lst.Add(CreateItemFromRow<T>(r));
            }

            return lst;
        }

        public static T CreateItemFromRow<T>(DataRow row) where T : new()
        {
            T item = new T();
            SetItemFromRow(item, row);

            return item;
        }

        public static void SetItemFromRow<T>(T item, DataRow row) where T : new()
        {
            foreach(DataColumn c in row.Table.Columns)
            {
                PropertyInfo p = item.GetType().GetProperty(c.ColumnName);

                if(p != null && row[c] != DBNull.Value)
                {
                    p.SetValue(item, row[c], null);
                }
            }
        }
    }
}
