
namespace BLL
{
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

        public bool UpdateEmployee(EMPLOYEE e)
        {
            using (var _dbcontext = new PayRollEntities())
            {
                _dbcontext.Entry(e).State = EntityState.Modified;
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

        public EMPLOYEE GetEmployee(int EMPID)
        {
            using (var _dbcontext = new PayRollEntities())
            {
                return (from e in _dbcontext.EMPLOYEEs where e.EMPID == EMPID select e).FirstOrDefault();
            }

        }

        public List<EMPLOYEE> GetEmployee()
        {
            using (var _dbcontext = new PayRollEntities())
            {
                var Emp = (from e in _dbcontext.EMPLOYEEs
                           join dep in _dbcontext.Departments on e.DEPARTMENTID equals dep.DepId
                           join des in _dbcontext.Designations on e.DESIGNATIONID equals des.DegId
                           join u in _dbcontext.UNITs on e.UNITID equals u.UnitId
                           join W in _dbcontext.WAGESTYPEs on e.WAGES_TYPEID equals W.WGId
                           select e).ToList();
                return Emp;
            }

        }

        public bool DeleteEmployee(int EMPID)
        {
            using (var _dbcontext = new PayRollEntities())
            {
                var emp = (from e in _dbcontext.EMPLOYEEs where e.EMPID == EMPID select e).FirstOrDefault();
                _dbcontext.EMPLOYEEs.Remove(emp);
                _dbcontext.SaveChanges();
                return true;
            }

        }
    }
}
