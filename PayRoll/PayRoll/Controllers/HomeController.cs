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

            return RedirectToAction("ListEmployee");

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
            return View();
        }

        [HttpPost]
        public JsonResult ListEmployeeJson()
        {
            if (Session[Constances.UserId] != null)
            {

                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                //Get Sort columns value
                //var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                //var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int totalRecords = 0;

                EmployeeRepository emp = new EmployeeRepository();
                var res = emp.ListEmployee(Session[Constances.Mode].ToString());

                totalRecords = res.Count();
                var data = res.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion

        public ActionResult TodayAttenance()
        {
            CommonRepository _repo = new CommonRepository();
            ViewBag.ListDepartment = _repo.ListDepartment();
            ViewBag.ListDesignation = _repo.ListDesignation();
            ViewBag.ListUNIT = _repo.ListUNIT();
            ViewBag.ListWAGESTYPE = _repo.ListWAGESTYPE();
            return View();
        }

        [HttpPost]
        public JsonResult TodayAttenanceJson()
        {
            if (Session[Constances.UserId] != null)
            {
                int UNIT = Request.Form["UNIT"].ToString() != "" ? Convert.ToInt32(Request.Form["UNIT"]) : 0;
                int Department = Request.Form["Department"].ToString() != "" ? Convert.ToInt32(Request.Form["Department"]) : 0;
                int DESIGNATION = Request.Form["DESIGNATION"].ToString() != "" ? Convert.ToInt32(Request.Form["DESIGNATION"]) : 0;

                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                //Get Sort columns value
                //var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
               // var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int totalRecords = 0;

                CommonRepository c = new CommonRepository();
                var res = c.GetTodayAttenanceReport(Session[Constances.Mode].ToString(), UNIT, Department, DESIGNATION);

                totalRecords = res.Count();
                var data = res.Skip(skip).Take(pageSize).ToList();

                foreach (var d in data)
                {
                    if (d.InTime != null)
                    {
                        d.sInTime = Convert.ToDateTime(DateTime.Now.Date + d.InTime).ToString("hh:mm tt");
                        d.sOutTime = Convert.ToDateTime(DateTime.Now.Date + d.OutTime).ToString("hh:mm tt");
                        d.Hours = Convert.ToDateTime(DateTime.Now.Date + (DateTime.Now - (DateTime.Now.Date + d.InTime))).ToString("HH:mm");
                    }
                }

                return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

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