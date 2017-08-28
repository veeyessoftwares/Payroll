namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ShiftMaster")]
    public partial class ShiftMaster
    {
        public int ID { get; set; }
        public int WGID { get; set; }
        public TimeSpan InTime { get;set; }
        public TimeSpan OutTime { get; set; }
        public TimeSpan Hours { get; set; }
    }
}
