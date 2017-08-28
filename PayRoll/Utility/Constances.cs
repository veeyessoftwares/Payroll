namespace Utility
{
    public static class Constances
    {
        public const string EMPCODE = "EMP CODE";
        public const string MACCODE = "MAC CODE";
        public const string EMP_NAME = "EMP NAME";
        public const string FATHER_NAME = "FATHER";
        public const string MOTHER_NAME = "MOTHER";
        public const string DOB = "DOB";
        public const string DOJ = "DOJ";
        public const string MARITAL_STATUS = "MERITAL STATUS";
        public const string GENDER = "GENDER";
        public const string ACTUAL_SALARY = "ACTUAL";
        public const string ADUITING_SALARY = "ADUITING";
        public const string UNITID = "UNIIT";
        public const string DEPARTMENTID = "DEPARTMENT";
        public const string DESIGNATIONID = "DESIGNATION";
        public const string WAGES_TYPEID = "WAGES TYPE";
        public const string IMAGE = "IMAGE";
        public const string Mode = "Mode";
        public const string IsActive = "Is Active";
        public const string IsDeleted = "Is Deleted";
        public const string CreatedBy = "Created By";
        public const string CreatedDate = "Created Date";
        public const string ModifiedBy = "Modified By";
        public const string ModifiedDate = "Modified Date";
        public const string UserId = "UserId";
        public const string ACTUAL = "ACTUAL";
        public const string ADUITING = "ADUITING";

        public const string sAttendanceDate = "Date";
        public const string sInTime = "In Time";
        public const string sOutTime = "Out Time";

    }

    public enum Gender
    {
        Male,
        FeMale
    }

    public enum MARITALSTATUS
    {
        Single,
        Married
    }

    public enum Mode
    {
        ACTUAL,
        ADUITING
    }

    public enum WageType
    {
        Monthly = 1,
        Weakly = 2
    }

}
