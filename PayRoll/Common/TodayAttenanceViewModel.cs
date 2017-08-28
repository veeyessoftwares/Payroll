using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class TodayAttenanceViewModel
    {
        public string Unit { get; set; }
        public int EMPID { get; set; }
        public int? EmpCode { get; set; }
        public string EmpName { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string Status { get; set; }
        public TimeSpan? InTime { get; set; }
        public string sInTime { get; set; }
        public TimeSpan? OutTime { get; set; }
        public string sOutTime { get; set; }
        public string Hours { get; set; }
    }

    public class AttenanceViewModel
    {
        public string Unit { get; set; }
        public int EMPID { get; set; }
        public int? EmpCode { get; set; }
        public string EmpName { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string Status { get; set; }
        public TimeSpan? InTime { get; set; }
        public string sInTime { get; set; }
        public TimeSpan? OutTime { get; set; }
        public string sOutTime { get; set; }
        public string Hours { get; set; }
        public string PunchRecords { get; set; }
        public int? Attenanceid { get; set; }
        public decimal ShiftCount { get; set; }
    }

    public class filter
    {
        public int UNIT { get; set; }
        public int Department { get; set; }
        public int DESIGNATION { get; set; }
        public int Wagetype { get; set; }
        public DateTime Date { get; set; }
        public string Mode { get; set; }
    }

    public class Processeddata
    {
        public int EmpId { get; set; }
        public int Attenanceid { get; set; }
        public string EmpName { get; set; }
        public TimeSpan HoursWorked { get; set; }
        public string sHoursWorked { get; set; }
        public decimal ShiftCount { get; set; }
        public string Status { get; set; }
    }

}
