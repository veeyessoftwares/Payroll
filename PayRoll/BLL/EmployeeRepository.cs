
namespace BLL
{
    using Common;
    using DAL;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;
    public class EmployeeRepository
    {
        public int AddEmployee(EMPLOYEE e)
        {
            using (var _dbcontext = new PayRollEntities())
            {
                var emp = (from em in _dbcontext.EMPLOYEEs where em.EMPCODE == e.EMPCODE select em).FirstOrDefault();
                if (emp == null)
                {
                    _dbcontext.EMPLOYEEs.Add(e);
                    _dbcontext.SaveChanges();
                    return e.EMPID;
                }
                else
                {
                    return -2;
                }

            }
        }

        public bool UpdateEmployee(EMPLOYEE model)
        {
            using (var _dbcontext = new PayRollEntities())
            {
                var emp = (from e in _dbcontext.EMPLOYEEs where e.EMPID == model.EMPID select e).FirstOrDefault();
                emp.ACTUAL_SALARY = model.ACTUAL_SALARY;
                emp.ADUITING_SALARY = model.ADUITING_SALARY;
                emp.DEPARTMENTID = model.DEPARTMENTID;
                emp.DESIGNATIONID = model.DESIGNATIONID;
                emp.DOB = model.DOB;
                emp.DOJ = model.DOJ;
                emp.EMPCODE = model.EMPCODE;
                emp.EMP_NAME = model.EMP_NAME;
                emp.FATHER_NAME = model.FATHER_NAME;
                emp.MOTHER_NAME = model.MOTHER_NAME;
                emp.GENDER = model.GENDER;
                emp.MARITAL_STATUS = model.MARITAL_STATUS;
                emp.MACCODE = model.MACCODE;
                emp.UNITID = model.UNITID;
                emp.WAGES_TYPEID = model.WAGES_TYPEID;
                emp.ModifiedBy = model.ModifiedBy;
                emp.ModifiedDate = model.ModifiedDate;
                emp.Mode = model.Mode;
                emp.IsActive = model.IsActive;
                emp.IsDeleted = model.IsDeleted;
                emp.IMAGE = model.IMAGE != "" ? model.IMAGE : emp.IMAGE;
                _dbcontext.Entry(emp).State = EntityState.Modified;
                _dbcontext.SaveChanges();
                return true;
            }
        }

        public bool UpdateEmployeeImage(int EMPID, string imgPath)
        {
            using (var _dbcontext = new PayRollEntities())
            {
                var emp = (from e in _dbcontext.EMPLOYEEs where e.EMPID == EMPID select e).FirstOrDefault();
                emp.IMAGE = imgPath;
                _dbcontext.Entry(emp).State = EntityState.Modified;
                _dbcontext.SaveChanges();
                return true;
            }
        }

        public EmployeeViewModel GetEmployee(int EMPID)
        {
            using (var _dbcontext = new PayRollEntities())
            {
                return (from e in _dbcontext.EMPLOYEEs
                        where e.EMPID == EMPID
                        select new EmployeeViewModel()
                        {
                            EMPID = e.EMPID,
                            EMPCODE = e.EMPCODE,
                            MACCODE = e.MACCODE,
                            EMP_NAME = e.EMP_NAME,
                            FATHER_NAME = e.FATHER_NAME,
                            MOTHER_NAME = e.MOTHER_NAME,
                            DOB = e.DOB,
                            DOJ = e.DOJ,
                            MARITAL_STATUS = e.MARITAL_STATUS,
                            GENDER = e.GENDER,
                            ACTUAL_SALARY = e.ACTUAL_SALARY,
                            ADUITING_SALARY = e.ADUITING_SALARY,
                            UNITID = e.UNITID,
                            DEPARTMENTID = e.DEPARTMENTID,
                            DESIGNATIONID = e.DESIGNATIONID,
                            WAGES_TYPEID = e.WAGES_TYPEID,
                            IsActive = (bool)e.IsActive,
                            IsDeleted = (bool)e.IsDeleted,
                            Mode = e.Mode,
                            IMAGE = e.IMAGE
                        }).FirstOrDefault();
            }
        }

        public List<EmployeeViewModel> ListEmployee(string mode)
        {
            using (var _dbcontext = new PayRollEntities())
            {
                if (mode == "ADUITING")
                {
                    var Emp = (from e in _dbcontext.EMPLOYEEs
                               join dep in _dbcontext.Departments on e.DEPARTMENTID equals dep.DepId
                               join des in _dbcontext.Designations on e.DESIGNATIONID equals des.DegId
                               join u in _dbcontext.UNITs on e.UNITID equals u.UnitId
                               join W in _dbcontext.WAGESTYPEs on e.WAGES_TYPEID equals W.WGId
                               where e.IsDeleted == false && e.Mode == mode
                               select new EmployeeViewModel()
                               {
                                   EMPID = e.EMPID,
                                   EMPCODE = e.EMPCODE,
                                   MACCODE = e.MACCODE,
                                   EMP_NAME = e.EMP_NAME,
                                   FATHER_NAME = e.FATHER_NAME,
                                   MOTHER_NAME = e.MOTHER_NAME,
                                   DOB = e.DOB,
                                   DOJ = e.DOJ,
                                   MARITAL_STATUS = e.MARITAL_STATUS,
                                   GENDER = e.GENDER,
                                   ACTUAL_SALARY = e.ACTUAL_SALARY,
                                   ADUITING_SALARY = e.ADUITING_SALARY,
                                   UNIT = u.Name,
                                   DEPARTMENT = dep.DEPT,
                                   DESIGNATION = des.DESG,
                                   WAGESTYPE = W.Type
                               }).OrderBy(x => x.EMPCODE).ToList();
                    return Emp;
                }
                else
                {
                    var Emp = (from e in _dbcontext.EMPLOYEEs
                               join dep in _dbcontext.Departments on e.DEPARTMENTID equals dep.DepId
                               join des in _dbcontext.Designations on e.DESIGNATIONID equals des.DegId
                               join u in _dbcontext.UNITs on e.UNITID equals u.UnitId
                               join W in _dbcontext.WAGESTYPEs on e.WAGES_TYPEID equals W.WGId
                               where e.IsDeleted == false
                               select new EmployeeViewModel()
                               {
                                   EMPID = e.EMPID,
                                   EMPCODE = e.EMPCODE,
                                   MACCODE = e.MACCODE,
                                   EMP_NAME = e.EMP_NAME,
                                   FATHER_NAME = e.FATHER_NAME,
                                   MOTHER_NAME = e.MOTHER_NAME,
                                   DOB = e.DOB,
                                   DOJ = e.DOJ,
                                   MARITAL_STATUS = e.MARITAL_STATUS,
                                   GENDER = e.GENDER,
                                   ACTUAL_SALARY = e.ACTUAL_SALARY,
                                   ADUITING_SALARY = e.ADUITING_SALARY,
                                   UNIT = u.Name,
                                   DEPARTMENT = dep.DEPT,
                                   DESIGNATION = des.DESG,
                                   WAGESTYPE = W.Type
                               }).OrderBy(x => x.EMPCODE).ToList();
                    return Emp;
                }
            }
        }

        //public bool DeleteEmployee(int EMPID)
        //{
        //    using (var _dbcontext = new PayRollEntities())
        //    {
        //        var emp = (from e in _dbcontext.EMPLOYEEs where e.EMPID == EMPID select e).FirstOrDefault();
        //        _dbcontext.EMPLOYEEs.Remove(emp);
        //        _dbcontext.SaveChanges();
        //        return true;
        //    }
        //}
    }
}
