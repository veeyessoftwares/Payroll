using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Common
{
    public class EmployeeViewModel
    {
        public int EMPID { get; set; }

        [Required]
        [Display(Name = Constances.EMPCODE)]
        public Nullable<int> EMPCODE { get; set; }

        [Display(Name = Constances.MACCODE)]
        public string MACCODE { get; set; }

        [Required]
        [Display(Name = Constances.EMP_NAME)]
        public string EMP_NAME { get; set; }

        [Display(Name = Constances.FATHER_NAME)]
        public string FATHER_NAME { get; set; }

        [Display(Name = Constances.MOTHER_NAME)]
        public string MOTHER_NAME { get; set; }

        [Display(Name = Constances.DOB)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> DOB { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = Constances.DOJ)]
        public Nullable<System.DateTime> DOJ { get; set; }

        [Display(Name = Constances.MARITAL_STATUS)]
        public string MARITAL_STATUS { get; set; }

        [Display(Name = Constances.GENDER)]
        public string GENDER { get; set; }

        [Display(Name = Constances.ACTUAL_SALARY)]
        public Nullable<decimal> ACTUAL_SALARY { get; set; }

        [Display(Name = Constances.ADUITING_SALARY)]
        public Nullable<decimal> ADUITING_SALARY { get; set; }

        [Required]
        [Display(Name = Constances.UNITID)]
        public Nullable<int> UNITID { get; set; }

        public string UNIT { get; set; }

        [Required]
        [Display(Name = Constances.DEPARTMENTID)]
        public Nullable<int> DEPARTMENTID { get; set; }

        public string DEPARTMENT { get; set; }

        [Required]
        [Display(Name = Constances.DESIGNATIONID)]
        public Nullable<int> DESIGNATIONID { get; set; }

        public string DESIGNATION { get; set; }

        [Required]
        [Display(Name = Constances.WAGES_TYPEID)]
        public Nullable<int> WAGES_TYPEID { get; set; }

        public string WAGESTYPE { get; set; }

        [Display(Name = Constances.IMAGE)]
        public string IMAGE { get; set; }

        [Required]
        [Display(Name = Constances.Mode)]
        public string Mode { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public Nullable<int> CreatedBy { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }

        public Nullable<int> ModifiedBy { get; set; }

        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
