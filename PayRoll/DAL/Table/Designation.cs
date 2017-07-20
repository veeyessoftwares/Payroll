namespace DAL
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Designation")]
    public partial class Designation
    {
     

        [Key]
        public int DegId { get; set; }
        public string DESG { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    
        
    }
}
