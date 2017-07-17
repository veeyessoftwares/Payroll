
namespace DAL
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ATTENDANCE_IMPORT")]
    public partial class ATTENDANCE_IMPORT
    {
        public int Id { get; set; }
        public int AttendanceLogId { get; set; }
        public Nullable<System.DateTime> AttendanceDate { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public Nullable<System.TimeSpan> InTime { get; set; }
        public Nullable<System.TimeSpan> OutTime { get; set; }
        public string PunchRecords { get; set; }
    }
}
