using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Common
{
    public class AttenanceModel
    {
        public int Id { get; set; }
        public int AttendanceLogId { get; set; }
        public Nullable<System.DateTime> AttendanceDate { get; set; }
        [Required]
        [Display(Name = Constances.sAttendanceDate)]
        public string sAttendanceDate { get; set; }
        [Required]
        public Nullable<int> EmployeeId { get; set; }
        public Nullable<System.TimeSpan> InTime { get; set; }
        [Required]
        [Display(Name = Constances.sInTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm tt}")]
        public string sInTime { get; set; }
        public Nullable<System.TimeSpan> OutTime { get; set; }
        [Required]
        [Display(Name = Constances.sOutTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm tt}")]
        public string sOutTime { get; set; }
        public string PunchRecords { get; set; }
    }
}
