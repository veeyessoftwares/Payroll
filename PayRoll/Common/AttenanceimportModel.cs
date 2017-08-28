using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class AttenanceimportModel
    {
        public int Id { get; set; }
        public int AttendanceLogId { get; set; }
        public Nullable<System.DateTime> AttendanceDate { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public Nullable<System.TimeSpan> InTime { get; set; }
        public Nullable<System.TimeSpan> OutTime { get; set; }
        public string PunchRecords { get; set; }
        public decimal ShiftCount { get; set; }
    }
}
