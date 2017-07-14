using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Web;

namespace ImportAttendance
{
    public class ErrorLog
    {
        public static void WriteErrorMessage(Exception ex)
        {
            string path = ConfigurationManager.AppSettings["Errorlogpath"].ToString();
            DirectoryInfo di = new DirectoryInfo(path);
            if (di.Exists == false)
            {
                di.Create();
            }
            path = path + "\\" + DateTime.Now.ToString("MMddyyyy") + ".log";

            StreamWriter sw = new StreamWriter(path, true);
            sw.WriteLine(DateTime.Now.ToString(CultureInfo.CurrentCulture) + " : " + ex.Message.ToString() + "\n" + ex.StackTrace.ToString());
            sw.WriteLine("====================================================================================");
            sw.Close();
        }
    }
}
