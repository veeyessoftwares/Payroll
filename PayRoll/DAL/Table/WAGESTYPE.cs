namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("WAGESTYPE")]
    public partial class WAGESTYPE
    {

        [Key]
        public int WGId { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public TimeSpan Allowance { get; set; }
    }
}
