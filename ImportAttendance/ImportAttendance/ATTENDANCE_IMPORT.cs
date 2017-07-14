using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportAttendance
{
    public class ATTENDANCE_IMPORT
    {
        public int AttendanceLogId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public int EmployeeId { get; set; }
        public TimeSpan InTime { get; set; }
        public TimeSpan OutTime { get; set; }
        public string PunchRecords { get; set; }
    }
}
