namespace DAL
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EMPLOYEE")]
    public partial class EMPLOYEE
    {
        [Key]
        public int EMPID { get; set; }
        public Nullable<int> EMPCODE { get; set; }
        public string MACCODE { get; set; }
        public string EMP_NAME { get; set; }
        public string FATHER_NAME { get; set; }
        public string MOTHER_NAME { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public Nullable<System.DateTime> DOJ { get; set; }
        public string MARITAL_STATUS { get; set; }
        public string GENDER { get; set; }
        public Nullable<decimal> ACTUAL_SALARY { get; set; }
        public Nullable<decimal> ADUITING_SALARY { get; set; }
        public Nullable<int> UNITID { get; set; }
        public Nullable<int> DEPARTMENTID { get; set; }
        public Nullable<int> DESIGNATIONID { get; set; }
        public Nullable<int> WAGES_TYPEID { get; set; }
        public string IMAGE { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
     
    }
}
