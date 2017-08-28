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

        public List<TodayAttenanceViewModel> GetTodayAttenanceReport(filter f)
        {
            using (var _dbcontext = new PayRollEntities())
            {
                SqlConnection con = _dbcontext.Database.Connection as SqlConnection;
                var eMPLOYEEData = con.Query<EMPLOYEE>("[dbo].[SP_GetEmployeelist] @Mode,@UNITID,@DEPARTMENTID,@DESIGNATIONID,@WAGES_TYPEID", new { Mode = f.Mode, UNITID = f.UNIT, DEPARTMENTID = f.Department, DESIGNATIONID = f.DESIGNATION, WAGES_TYPEID = f.Wagetype });


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

        public List<AttenanceViewModel> GetAttenanceReport(filter f)
        {
            using (var _dbcontext = new PayRollEntities())
            {
                SqlConnection con = _dbcontext.Database.Connection as SqlConnection;
                var eMPLOYEEData = con.Query<EMPLOYEE>("[dbo].[SP_GetEmployeelist] @Mode,@UNITID,@DEPARTMENTID,@DESIGNATIONID,@WAGES_TYPEID", new { Mode = f.Mode, UNITID = f.UNIT, DEPARTMENTID = f.Department, DESIGNATIONID = f.DESIGNATION, WAGES_TYPEID = f.Wagetype });

                var Empdetails = (from e in eMPLOYEEData
                                  join u in _dbcontext.UNITs on e.UNITID equals u.UnitId
                                  join dep in _dbcontext.Departments on e.DEPARTMENTID equals dep.DepId
                                  join des in _dbcontext.Designations on e.DESIGNATIONID equals des.DegId
                                  select new AttenanceViewModel()
                                  {
                                      EMPID = e.EMPID,
                                      Unit = u.Name,
                                      Department = dep.DEPT,
                                      Designation = des.DESG,
                                      EmpName = e.EMP_NAME,
                                      EmpCode = e.EMPCODE
                                  }).ToList();

                var ProcessedData = (from pd in _dbcontext.ProcessedData
                                     where pd.Date == DbFunctions.TruncateTime(f.Date)
                                     select pd).ToList();

                var Attdetails = (from ai in _dbcontext.ATTENDANCE_IMPORT
                                  where ai.AttendanceDate == DbFunctions.TruncateTime(f.Date)
                                  select ai).ToList();

                var res = (from e in Empdetails
                           join at in Attdetails on e.EmpCode equals at.EmployeeId into Attendance
                           from ai in Attendance.DefaultIfEmpty()
                           join pd in ProcessedData on e.EmpCode equals pd.EmpCode into Data
                           from d in Data.DefaultIfEmpty()
                           select new AttenanceViewModel()
                           {
                               EMPID = e.EMPID,
                               Unit = e.Unit,
                               Department = e.Department,
                               Designation = e.Designation,
                               EmpName = e.EmpName,
                               EmpCode = e.EmpCode,
                               InTime = ai != null ? ai.InTime : null,
                               OutTime = ai != null ? ai.OutTime : null,
                               PunchRecords = ai != null ? ai.PunchRecords : "",
                               Status = ai != null ? "Present" : "Absent",
                               Attenanceid = ai != null ? ai.Id : 0,
                               ShiftCount = d != null ? d.ShiftCount : 0m
                           }).ToList();

                return res;
            }
        }

        public ATTENDANCE_IMPORT GetAttenancedate(int id)
        {
            using (var _dbcontext = new PayRollEntities())
            {
                return _dbcontext.ATTENDANCE_IMPORT.Where(x => x.Id == id).FirstOrDefault();
            }
        }

        public bool UpdateAttenance(AttenanceModel am)
        {
            using (var _dbcontext = new PayRollEntities())
            {
                var at = (from a in _dbcontext.ATTENDANCE_IMPORT where a.Id == am.Id select a).FirstOrDefault();
                at.InTime = am.InTime;
                at.OutTime = am.OutTime;
                at.PunchRecords = am.PunchRecords;
                _dbcontext.Entry(at).State = EntityState.Modified;
                _dbcontext.SaveChanges();
                return true;
            }
        }

        public int AddAttenance(ATTENDANCE_IMPORT ai)
        {
            using (var _dbcontext = new PayRollEntities())
            {
                var att = (from aim in _dbcontext.ATTENDANCE_IMPORT where aim.EmployeeId == ai.EmployeeId && aim.AttendanceDate == DbFunctions.TruncateTime(ai.AttendanceDate) select aim).FirstOrDefault();
                if (att == null)
                {
                    _dbcontext.ATTENDANCE_IMPORT.Add(ai);
                    _dbcontext.SaveChanges();
                    return ai.Id;
                }
                else
                {
                    return -2;
                }

            }
        }

        #region Pick List

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

        #endregion
    }
}
