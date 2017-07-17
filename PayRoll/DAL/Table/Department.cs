namespace DAL
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Department")]
    public partial class Department
    {
      

        [Key]
        public int DepId { get; set; }
        public string DEPT { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

       
    }
}
