namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ProcessedData")]
    public partial class ProcessedData
    {
        public int ID { get; set; }
        public int EmpCode { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan HoursWorked { get; set; }
        public decimal ShiftCount { get; set; }
        public string Status { get; set; }
        public Nullable<decimal> ActualSalary { get; set; }
        public Nullable<decimal> AduitingSalary { get; set; }
    }
}
