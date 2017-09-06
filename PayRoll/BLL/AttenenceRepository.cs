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

    public class AttenenceRepository
    {

        public WAGESTYPE GetWAGESTYPE(int WGID)
        {
            using (var _dbcontext = new PayRollEntities())
            {
                var res = (from w in _dbcontext.WAGESTYPEs where w.WGId == WGID select w).FirstOrDefault();
                return res;
            }
        }

        public List<ShiftMaster> GetShiftMasterData(int WGID)
        {
            using (var _dbcontext = new PayRollEntities())
            {
                var res = (from sm in _dbcontext.ShiftMaster where sm.WGID == WGID select sm).ToList();
                return res;
            }
        }

        public List<AttenanceProcessModel> GetAttenancedata(DateTime date, int WGID)
        {
            using (var _dbcontext = new PayRollEntities())
            {
                var Attdetails = (from ai in _dbcontext.ATTENDANCE_IMPORT
                                  join e in _dbcontext.EMPLOYEEs on ai.EmployeeId equals e.EMPCODE
                                  where ai.AttendanceDate == DbFunctions.TruncateTime(date) && e.WAGES_TYPEID == WGID
                                  select new AttenanceProcessModel
                                  {
                                      Id = ai.Id,
                                      AttendanceLogId = ai.AttendanceLogId,
                                      AttendanceDate = ai.AttendanceDate,
                                      EmployeeId = ai.EmployeeId,
                                      InTime = ai.InTime,
                                      OutTime = ai.OutTime,
                                      PunchRecords = ai.PunchRecords,
                                      ActualSalary = e.ACTUAL_SALARY,
                                      AduitingSalary = e.ADUITING_SALARY
                                  }).ToList();
                return Attdetails;
            }

        }

        public bool InsertProcessData(ProcessedData p)
        {
            using (var _dbcontext = new PayRollEntities())
            {
                var pd = (from d in _dbcontext.ProcessedData where d.Date == DbFunctions.TruncateTime(p.Date) && d.EmpCode == p.EmpCode select d).FirstOrDefault();

                if (pd != null)
                {
                    pd.HoursWorked = p.HoursWorked;
                    pd.ShiftCount = p.ShiftCount;
                    pd.Status = p.Status;
                    pd.ActualSalary = p.ActualSalary;
                    pd.AduitingSalary = p.AduitingSalary;
                    _dbcontext.Entry(pd).State = EntityState.Modified;
                    _dbcontext.SaveChanges();
                }
                else
                {
                    _dbcontext.ProcessedData.Add(p);
                    _dbcontext.SaveChanges();
                }

                return true;
            }
        }

        public List<Processeddata> GetProcessedAttenancedata(filter f)
        {
            using (var _dbcontext = new PayRollEntities())
            {
                var Attdetails = (from pd in _dbcontext.ProcessedData
                                  join ai in _dbcontext.ATTENDANCE_IMPORT on pd.EmpCode equals ai.EmployeeId
                                  join e in _dbcontext.EMPLOYEEs on pd.EmpCode equals e.EMPCODE
                                  where pd.Date == DbFunctions.TruncateTime(f.Date) && ai.AttendanceDate == DbFunctions.TruncateTime(f.Date)
                                  select new Processeddata
                                  {
                                      EmpId = pd.EmpCode,
                                      EmpName = e.EMP_NAME,
                                      HoursWorked = pd.HoursWorked,
                                      ShiftCount = pd.ShiftCount,
                                      Attenanceid = ai.Id,
                                      Status = pd.Status
                                  }).ToList();
                return Attdetails;
            }

        }

    }
}
