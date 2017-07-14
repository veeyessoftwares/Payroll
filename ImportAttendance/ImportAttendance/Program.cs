using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ImportAttendance
{
    public class Program
    {
        // Initialize sql connection
        public static SqlConnection sqlcon;

        // Configure ms access db path in app settings
        public static string path = ConfigurationManager.AppSettings["msaccessfilepath"].ToString();

        // Initialize Oledb connection
        public static OleDbConnection Olecon;

        static void Main(string[] args)
        {
            Import();
        }

        /// <summary>
        /// Retrive data from Ms Access Databse
        /// </summary>
        public static void Import()
        {
            try
            {
                List<ATTENDANCE_IMPORT> ai = new List<ATTENDANCE_IMPORT>();

                int LastAttendanceLogId = 0;
                sqlcon = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                Olecon = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;" + "data source=" + path);
                using (sqlcon)
                {
                    sqlcon.Open();
                    var query = "select isnull(max(AttendanceLogId),0) from ATTENDANCE_IMPORT";
                    SqlCommand cmd = new SqlCommand(query, sqlcon);
                    LastAttendanceLogId = (int)cmd.ExecuteScalar();
                    sqlcon.Close();
                }

                using (Olecon)
                {
                    Olecon.Open();
                    string query = "";

                    if (LastAttendanceLogId != 0)
                    {
                        query = "Select AttendanceLogId,AttendanceDate,EmployeeId,InTime,OutTime,PunchRecords From AttendanceLogs where InTime<>'1900-01-01 00:00:00' and AttendanceDate >=#" + DateTime.Now.AddDays(-1).Date + "#";
                    }
                    else
                    {
                        query = "Select AttendanceLogId,AttendanceDate,EmployeeId,InTime,OutTime,PunchRecords From AttendanceLogs where InTime<>'1900-01-01 00:00:00'";
                    }

                    var command = new OleDbCommand(query, Olecon);
                    OleDbDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ATTENDANCE_IMPORT a = new ATTENDANCE_IMPORT();
                        a.AttendanceLogId = Convert.ToInt32(reader["AttendanceLogId"]);
                        a.AttendanceDate = Convert.ToDateTime(reader["AttendanceDate"]);
                        a.EmployeeId = Convert.ToInt32(reader["EmployeeId"]);
                        a.InTime = Convert.ToDateTime(reader["InTime"]).TimeOfDay;
                        a.OutTime = Convert.ToDateTime(reader["OutTime"]).TimeOfDay;
                        a.PunchRecords = Convert.ToString(reader["PunchRecords"]);
                        ai.Add(a);
                    }
                    Olecon.Close();
                }

                if (ai != null)
                {
                    var addai = (from a in ai where a.AttendanceLogId > LastAttendanceLogId select a).ToList();

                    if (addai != null && addai.Count > 0)
                    {
                        InsertBulkData(addai);
                    }

                    var updateai = (from a in ai where a.AttendanceLogId <= LastAttendanceLogId select a).ToList();

                    if (updateai != null && updateai.Count > 0)
                    {
                        UpdateRecords(updateai);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ex);
            }
        }


        /// <summary>
        /// Insert all records at once using SqlBulkCopy
        /// </summary>
        /// <param name="ai"></param>
        public static void InsertBulkData(List<ATTENDANCE_IMPORT> ai)
        {
            try
            {
                sqlcon = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                DataTable dt = ToDataTable(ai);
                sqlcon.Open();
                SqlBulkCopy objbulk = new SqlBulkCopy(sqlcon);
                objbulk.DestinationTableName = "ATTENDANCE_IMPORT";
                objbulk.ColumnMappings.Add("AttendanceLogId", "AttendanceLogId");
                objbulk.ColumnMappings.Add("AttendanceDate", "AttendanceDate");
                objbulk.ColumnMappings.Add("EmployeeId", "EmployeeId");
                objbulk.ColumnMappings.Add("InTime", "InTime");
                objbulk.ColumnMappings.Add("OutTime", "OutTime");
                objbulk.ColumnMappings.Add("PunchRecords", "PunchRecords");
                objbulk.WriteToServer(dt);
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ex);
            }
        }

        public static void UpdateRecords(List<ATTENDANCE_IMPORT> ai)
        {
            try
            {
                sqlcon = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                using (sqlcon)
                {
                    sqlcon.Open();
                    foreach (var a in ai)
                    {
                        var query = "update ATTENDANCE_IMPORT set AttendanceDate='" + a.AttendanceDate + "',EmployeeId=" + a.EmployeeId + ",InTime='" + a.InTime + "',OutTime='" + a.OutTime + "',PunchRecords='" + a.PunchRecords + "' where AttendanceLogId=" + a.AttendanceLogId + "";
                        SqlCommand cmd = new SqlCommand(query, sqlcon);
                        cmd.ExecuteNonQuery();
                    }
                    sqlcon.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorMessage(ex);
            }
        }


        /// <summary>
        /// Convert List to DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

    }

}
