using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using System.Web.SessionState;
using Common;
using Utility;
using DAL;
using System.IO;
using Microsoft.AspNet.Identity;

namespace PayRoll.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddEmployee()
        {
            EmployeeViewModel e = new EmployeeViewModel();
            CommonRepository _repo = new CommonRepository();
            ViewBag.ListDepartment = _repo.ListDepartment();
            ViewBag.ListDesignation = _repo.ListDesignation();
            ViewBag.ListUNIT = _repo.ListUNIT();
            ViewBag.ListWAGESTYPE = _repo.ListWAGESTYPE();

            ViewBag.ListGENDER = new List<SelectListItem>
            {
                new SelectListItem{ Text = Gender.Male.ToString(), Value = Gender.Male.ToString() },
                new SelectListItem{ Text = Gender.FeMale.ToString(), Value = Gender.FeMale.ToString() }
            };

            ViewBag.ListMARITALSTATUS = new List<SelectListItem>
            {
                new SelectListItem{ Text = MARITALSTATUS.Single.ToString(), Value = MARITALSTATUS.Single.ToString() },
                new SelectListItem{ Text = MARITALSTATUS.Married.ToString(), Value = MARITALSTATUS.Married.ToString()}
            };

            return View(e);
        }
        [HttpPost]
        public ActionResult AddEmployee(EmployeeViewModel model, HttpPostedFileBase profpic)
        {
            if (ModelState.IsValid)
            {
                EMPLOYEE e = new EMPLOYEE();
                e.ACTUAL_SALARY = model.ACTUAL_SALARY;
                e.ADUITING_SALARY = model.ADUITING_SALARY;
                e.CreatedBy = Session["UserID"] != null ? Convert.ToInt32(Session["UserID"]) : 0;
                e.CreatedDate = model.CreatedDate;
                e.DEPARTMENTID = model.DEPARTMENTID;
                e.DESIGNATIONID = model.DESIGNATIONID;
                e.DOB = model.DOB;
                e.DOJ = model.DOJ;
                e.EMPCODE = model.EMPCODE;
                e.EMP_NAME = model.EMP_NAME;
                e.FATHER_NAME = model.FATHER_NAME;
                e.MOTHER_NAME = model.MOTHER_NAME;
                e.GENDER = model.GENDER;
                e.MARITAL_STATUS = model.MARITAL_STATUS;
                e.MACCODE = model.MACCODE;
                e.UNITID = model.UNITID;
                e.WAGES_TYPEID = model.WAGES_TYPEID;
                e.ModifiedBy = Session["UserID"] != null ? Convert.ToInt32(Session["UserID"]) : 0;
                e.ModifiedDate = model.ModifiedDate;

                EmployeeRepository emp = new EmployeeRepository();
                var id = emp.AddEmployee(e);

                if (id > 0 && profpic != null && profpic.ContentLength > 0)
                {
                    string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" };
                    var validfile = formats.Any(item => profpic.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));

                    if (validfile)
                    {
                        var path = Server.MapPath("~/EmployeeImage");

                        DirectoryInfo di = new DirectoryInfo(path);
                        if (!di.Exists)
                        {
                            di.Create();
                        }
                        string filename = id + "_" + profpic.FileName.Replace(" ", "_");
                        profpic.SaveAs(path + "\\" + filename);

                        var empDetails = emp.UpdateEmployeeImage(id, filename);
                    }
                }
            }
            else
            {
                CommonRepository _repo = new CommonRepository();
                ViewBag.ListDepartment = _repo.ListDepartment();
                ViewBag.ListDesignation = _repo.ListDesignation();
                ViewBag.ListUNIT = _repo.ListUNIT();
                ViewBag.ListWAGESTYPE = _repo.ListWAGESTYPE();

                ViewBag.ListGENDER = new List<SelectListItem>
            {
                new SelectListItem{ Text = Gender.Male.ToString(), Value = Gender.Male.ToString() },
                new SelectListItem{ Text = Gender.FeMale.ToString(), Value = Gender.FeMale.ToString() }
            };

                ViewBag.ListMARITALSTATUS = new List<SelectListItem>
            {
                new SelectListItem{ Text = MARITALSTATUS.Single.ToString(), Value = MARITALSTATUS.Single.ToString() },
                new SelectListItem{ Text = MARITALSTATUS.Married.ToString(), Value = MARITALSTATUS.Married.ToString()}
            };
                return View(model);
            }

            return RedirectToAction("Index");

        }

        public ActionResult TodayAttenance()
        {
            CommonRepository c = new CommonRepository();
            var res = c.GetTodayAttenanceReport();
            foreach(var r in res)
            {
                if (r.InTime != null)
                {
                    r.Hours = DateTime.Now - (DateTime.Now.Date + r.InTime);
                }
            }
            return View(res);
        }
    }


}