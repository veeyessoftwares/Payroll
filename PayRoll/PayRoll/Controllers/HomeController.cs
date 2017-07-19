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

        #region Employee

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

            ViewBag.ListMode = new List<SelectListItem>
            {
                new SelectListItem{ Text = Mode.ACTUAL.ToString(), Value = Mode.ACTUAL.ToString() },
                new SelectListItem{ Text = Mode.ADUITING.ToString(), Value = Mode.ADUITING.ToString()}
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
                e.CreatedBy = Session[Constances.UserId] != null ? Convert.ToString(Constances.UserId) : "";
                e.CreatedDate = DateTime.Now;
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
                e.ModifiedBy = Session[Constances.UserId] != null ? Convert.ToString(Session[Constances.UserId]) : "";
                e.ModifiedDate = DateTime.Now;
                e.Mode = model.Mode;
                e.IsActive = true;
                e.IsDeleted = false;

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
                ViewBag.ListMode = new List<SelectListItem>
            {
                new SelectListItem{ Text = Mode.ACTUAL.ToString(), Value = Mode.ACTUAL.ToString() },
                new SelectListItem{ Text = Mode.ADUITING.ToString(), Value = Mode.ADUITING.ToString()}
            };
                return View(model);
            }

            return RedirectToAction("Index");

        }

        public ActionResult UpdateEmployee(int EMPID)
        {
            EmployeeViewModel e = new EmployeeViewModel();

            EmployeeRepository er = new EmployeeRepository();
            e = er.GetEmployee(EMPID);

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

            ViewBag.ListMode = new List<SelectListItem>
            {
                new SelectListItem{ Text = Mode.ACTUAL.ToString(), Value = Mode.ACTUAL.ToString() },
                new SelectListItem{ Text = Mode.ADUITING.ToString(), Value = Mode.ADUITING.ToString()}
            };

            return View(e);
        }

        [HttpPost]
        public ActionResult UpdateEmployee(EmployeeViewModel model, HttpPostedFileBase profpic)
        {
            if (ModelState.IsValid)
            {
                EMPLOYEE e = new EMPLOYEE();
                e.EMPID = model.EMPID;
                e.ACTUAL_SALARY = model.ACTUAL_SALARY;
                e.ADUITING_SALARY = model.ADUITING_SALARY;
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
                e.ModifiedBy = Session[Constances.UserId] != null ? Convert.ToString(Session[Constances.UserId]) : "";
                e.ModifiedDate = DateTime.Now;
                e.Mode = model.Mode;
                e.IsActive = model.IsActive;
                e.IsDeleted = model.IsDeleted;



                if (profpic != null && profpic.ContentLength > 0)
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
                        string filename = model.EMPID + "_" + profpic.FileName.Replace(" ", "_");

                        FileInfo fi = new FileInfo(path + "\\" + filename);

                        if (fi.Exists)
                        {
                            fi.Delete();
                        }

                        profpic.SaveAs(path + "\\" + filename);
                        e.IMAGE = filename;
                    }
                }
                else
                {
                    e.IMAGE = "";
                }

                EmployeeRepository emp = new EmployeeRepository();
                var id = emp.UpdateEmployee(e);

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
                ViewBag.ListMode = new List<SelectListItem>
            {
                new SelectListItem{ Text = Mode.ACTUAL.ToString(), Value = Mode.ACTUAL.ToString() },
                new SelectListItem{ Text = Mode.ADUITING.ToString(), Value = Mode.ADUITING.ToString()}
            };
                return View(model);
            }

            return RedirectToAction("ListEmployee");

        }

        public ActionResult ListEmployee()
        {
            EmployeeRepository emp = new EmployeeRepository();
            var res = emp.ListEmployee(Session[Constances.Mode].ToString());
            return View(res);
        }

        #endregion

        public ActionResult TodayAttenance()
        {
            CommonRepository c = new CommonRepository();
            var res = c.GetTodayAttenanceReport();
            foreach (var r in res)
            {
                if (r.InTime != null)
                {
                    r.Hours = DateTime.Now - (DateTime.Now.Date + r.InTime);
                }
            }
            return View(res);
        }

        public JsonResult UpdateMode()
        {
            if (Session[Constances.UserId] != null)
            {
                if (Session[Constances.Mode].ToString() == Constances.ACTUAL)
                {
                    Session[Constances.Mode] = Constances.ADUITING;
                }
                else
                {
                    Session[Constances.Mode] = Constances.ACTUAL;
                }
            }
            return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
        }

    }


}