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
using System.Globalization;

namespace PayRoll.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        #region UpdateMode
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
        #endregion

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

        #region TodayAttenance

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

        #endregion

        #region AttenanceReport

        public ActionResult AttenanceReport(string Date="")
        {
            ViewBag.date = Date != "" ? Date : DateTime.Now.AddDays(-1).ToShortDateString();
            //CommonRepository _repo = new CommonRepository();
            //ViewBag.ListDepartment = _repo.ListDepartment();
            //ViewBag.ListDesignation = _repo.ListDesignation();
            //ViewBag.ListUNIT = _repo.ListUNIT();
            //ViewBag.ListWAGESTYPE = _repo.ListWAGESTYPE();
            return View();
        }

        [HttpPost]
        public JsonResult AttenanceReportJson()
        {
            if (Session[Constances.UserId] != null)
            {
                DateTime Date = Request.Form["Date"].ToString() != "" ? Convert.ToDateTime(Request.Form["Date"]) : DateTime.Now.AddDays(-1);

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
                var res = c.GetAttenanceReport(Session[Constances.Mode].ToString(), Date);

                totalRecords = res.Count();
                var data = res.Skip(skip).Take(pageSize).ToList();

                foreach (var d in data)
                {
                    if (d.InTime != null)
                    {
                        d.sInTime = Convert.ToDateTime(DateTime.Now.Date + d.InTime).ToString("hh:mm tt");
                        d.sOutTime = Convert.ToDateTime(DateTime.Now.Date + d.OutTime).ToString("hh:mm tt");

                        if (d.PunchRecords.EndsWith(","))
                            d.PunchRecords = d.PunchRecords.Substring(0, d.PunchRecords.Length - 1);

                        string[] PunchRecords = d.PunchRecords.Split(',');

                        if (PunchRecords.Length % 2 != 0)
                        {
                            d.Hours = "Invalid Punch records";
                        }
                        else
                        {
                            TimeSpan ts = TimeSpan.Parse("00:00");
                            for (int i = 0; i < PunchRecords.Length; i++)
                            {
                                PunchRecords[i] = PunchRecords[i].Substring(0, 5);
                                PunchRecords[i + 1] = PunchRecords[i + 1].Substring(0, 5);
                                TimeSpan tsa = TimeSpan.Parse(PunchRecords[i + 1]).Subtract(TimeSpan.Parse(PunchRecords[i]));
                                ts = ts.Add(tsa);
                                i++;
                            }
                            d.Hours = Convert.ToDateTime(DateTime.Now.Date + ts).ToString("HH:mm");
                        }

                        //d.Hours = Convert.ToDateTime(DateTime.Now.Date + (DateTime.Now - (DateTime.Now.Date + d.InTime))).ToString("HH:mm");
                    }
                }

                return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult UpdateAttenance(int id)
        {
            AttenanceModel am = new AttenanceModel();
            CommonRepository _repo = new CommonRepository();
            var at = _repo.GetAttenancedate(id);
            am.Id = at.Id;
            am.sAttendanceDate = Convert.ToDateTime(at.AttendanceDate).ToString("MM/dd/yyyy");
            am.EmployeeId = at.EmployeeId;
            am.sInTime = Convert.ToDateTime(DateTime.Now.Date + at.InTime).ToString("hh:mm tt");
            am.sOutTime = Convert.ToDateTime(DateTime.Now.Date + at.OutTime).ToString("hh:mm tt");
            am.PunchRecords = at.PunchRecords;
            ViewBag.date = am.sAttendanceDate;
            return View(am);
        }

        [HttpPost]
        public ActionResult UpdateAttenance(AttenanceModel am)
        {
            if (ModelState.IsValid)
            {
                am.InTime = DateTime.ParseExact(am.sInTime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                am.OutTime = DateTime.ParseExact(am.sOutTime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                CommonRepository _repo = new CommonRepository();
                _repo.UpdateAttenance(am);
            }
            else
            {
                return View(am);
            }

            return RedirectToAction("AttenanceReport", new { Date = Convert.ToDateTime(am.sAttendanceDate).ToString("MM/dd/yyyy") });
        }

        public ActionResult AddAttenance()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddAttenance(AttenanceModel am)
        {
            if (ModelState.IsValid)
            {
                ATTENDANCE_IMPORT ai = new ATTENDANCE_IMPORT();
                ai.AttendanceLogId = 0;
                ai.AttendanceDate = Convert.ToDateTime(am.sAttendanceDate);
                ai.EmployeeId = am.EmployeeId;
                ai.InTime = DateTime.ParseExact(am.sInTime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                ai.OutTime = DateTime.ParseExact(am.sOutTime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                ai.PunchRecords = am.PunchRecords;
                CommonRepository _repo = new CommonRepository();
                _repo.AddAttenance(ai);
            }
            else
            {
                return View(am);
            }

            return RedirectToAction("index");
        }

        #endregion

    }


}