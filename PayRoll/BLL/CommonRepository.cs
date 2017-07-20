namespace BLL
{
    using Common;
    using DAL;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using Dapper;
    public class CommonRepository
    {
        public List<ATTENDANCE_IMPORT> GetAttenanceReport()
        {
            using (var _dbcontext = new PayRollEntities())
            {
                var res = (from ai in _dbcontext.ATTENDANCE_IMPORT select ai).ToList();
                return res;
            }
        }

        public List<TodayAttenanceViewModel> GetTodayAttenanceReport(string Mode, int UNIT, int Department, int DESIGNATION)
        {
            using (var _dbcontext = new PayRollEntities())
            {
                SqlConnection con = _dbcontext.Database.Connection as SqlConnection;
                var eMPLOYEEData = con.Query<EMPLOYEE>("[dbo].[SP_GetEmployeelist] @Mode,@UNITID,@DEPARTMENTID,@DESIGNATIONID", new { Mode = Mode, UNITID = UNIT, DEPARTMENTID = Department, DESIGNATIONID = DESIGNATION });


                var Empdetails = (from e in eMPLOYEEData
                                  join u in _dbcontext.UNITs on e.UNITID equals u.UnitId
                                  join dep in _dbcontext.Departments on e.DEPARTMENTID equals dep.DepId
                                  join des in _dbcontext.Designations on e.DESIGNATIONID equals des.DegId
                                  select new TodayAttenanceViewModel()
                                  {
                                      EMPID = e.EMPID,
                                      Unit = u.Name,
                                      Department = dep.DEPT,
                                      Designation = des.DESG,
                                      EmpName = e.EMP_NAME,
                                      EmpCode = e.EMPCODE
                                  }).ToList();

                var Attdetails = (from ai in _dbcontext.ATTENDANCE_IMPORT
                                  where ai.AttendanceDate == DbFunctions.TruncateTime(DateTime.Now)
                                  select ai).ToList();

                var res = (from e in Empdetails
                           join at in Attdetails on e.EmpCode equals at.EmployeeId into Attendance
                           from ai in Attendance.DefaultIfEmpty()
                           select new TodayAttenanceViewModel()
                           {
                               EMPID = e.EMPID,
                               Unit = e.Unit,
                               Department = e.Department,
                               Designation = e.Designation,
                               EmpName = e.EmpName,
                               EmpCode = e.EmpCode,
                               InTime = ai != null ? ai.InTime : null,
                               OutTime = ai != null ? ai.OutTime : null,
                               Status = ai != null ? "Present" : "Absent"

                           }).ToList();

                return res;
            }
        }

        public IEnumerable<SelectListItem> ListDepartment()
        {
            using (var context = new PayRollEntities())
            {
                IEnumerable<SelectListItem> listData;
                listData = context.Database.SqlQuery<SelectListItem>("select Convert(nvarchar(10),DepId) as Value, DEPT as Text from Department where IsActive=1").ToList();
                return listData;
            }
        }

        public IEnumerable<SelectListItem> ListDesignation()
        {
            using (var context = new PayRollEntities())
            {
                IEnumerable<SelectListItem> listData;
                listData = context.Database.SqlQuery<SelectListItem>("select Convert(nvarchar(10),DegId) as Value, DESG as Text from Designation where IsActive=1").ToList();
                return listData;
            }
        }

        public IEnumerable<SelectListItem> ListUNIT()
        {
            using (var context = new PayRollEntities())
            {
                IEnumerable<SelectListItem> listData;
                listData = context.Database.SqlQuery<SelectListItem>("select Convert(nvarchar(10),UnitId) as Value, Name as Text from UNIT where IsActive=1").ToList();
                return listData;
            }
        }

        public IEnumerable<SelectListItem> ListWAGESTYPE()
        {
            using (var context = new PayRollEntities())
            {
                IEnumerable<SelectListItem> listData;
                listData = context.Database.SqlQuery<SelectListItem>("select Convert(nvarchar(10),WGId) as Value, Type as Text from WAGESTYPE where IsActive=1").ToList();
                return listData;
            }
        }

    }
}
