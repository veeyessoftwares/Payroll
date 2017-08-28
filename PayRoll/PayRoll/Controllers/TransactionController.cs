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
    public class TransactionController : Controller
    {
        // GET: Transaction
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProceedAttenance(string Date = "")
        {
            ViewBag.date = Date;
            return View();
        }

        [HttpPost]
        public JsonResult ProceedAttenanceJson(string Date)
        {
            AttenenceRepository ar = new AttenenceRepository();
            if (Session[Constances.UserId] != null)
            {
                DateTime Dateprocess = Date != "" ? Convert.ToDateTime(Date).Date : DateTime.Now.Date;

                // Monthly
                var Wage = ar.GetWAGESTYPE(Convert.ToInt32(WageType.Monthly));

                var shipmaster = ar.GetShiftMasterData(Convert.ToInt32(WageType.Monthly));

                var AttData = ar.GetAttenancedata(Dateprocess, Convert.ToInt32(WageType.Monthly));

                var Shiftstarttime = shipmaster.Select(x => x.InTime).FirstOrDefault();
                Shiftstarttime = Shiftstarttime.Add(Wage.Allowance);

                foreach (var at in AttData)
                {
                    ProcessedData p = new ProcessedData();
                    p.EmpCode = Convert.ToInt32(at.EmployeeId);
                    p.Date = Dateprocess;
                    var Intime = at.InTime;

                    if (Intime > Shiftstarttime)
                    {
                        p.Status = "Late";
                    }

                    if (at.PunchRecords.EndsWith(","))
                        at.PunchRecords = at.PunchRecords.Substring(0, at.PunchRecords.Length - 1);

                    string[] PunchRecords = at.PunchRecords.Split(',');

                    if (PunchRecords.Length % 2 != 0)
                    {
                        p.Status = "Invalid Punch records";
                    }
                    else
                    {
                        TimeSpan ts = TimeSpan.Parse("00:00");
                        for (int i = 0; i < PunchRecords.Length; i++)
                        {
                            PunchRecords[i] = PunchRecords[i].Substring(0, 5);
                            PunchRecords[i + 1] = PunchRecords[i + 1].Substring(0, 5);

                            if (TimeSpan.Parse(PunchRecords[i + 1]) > TimeSpan.Parse(PunchRecords[i]))
                            {
                                TimeSpan tsa = TimeSpan.Parse(PunchRecords[i + 1]).Subtract(TimeSpan.Parse(PunchRecords[i]));
                                ts = ts.Add(tsa);
                            }
                            else
                            {
                                TimeSpan day = TimeSpan.FromHours(24);
                                TimeSpan tsa = TimeSpan.Parse(PunchRecords[i + 1]).Add(day).Subtract(TimeSpan.Parse(PunchRecords[i]));
                                ts = ts.Add(tsa);
                            }
                            i++;
                        }
                        p.HoursWorked = ts;

                        if (ts.Hours >= 9)
                        {
                            p.ShiftCount = 1;
                        }
                        else if (Convert.ToDouble(ts.Hours + (Convert.ToDecimal(ts.Minutes)/100)) > 4.5 && ts.Hours < 9)
                        {
                            p.ShiftCount = 0.5m;
                        }
                        else
                        {
                            p.ShiftCount = 0m;
                        }

                        p.Status = "";
                    }

                    ar.InsertProcessData(p);
                }

            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ProceedAttenanceData(string Date)
        {
            AttenenceRepository ar = new AttenenceRepository();
            if (Session[Constances.UserId] != null)
            {
                filter f = new filter();
                f.Date = Date != "" ? Convert.ToDateTime(Date).Date : DateTime.Now.Date;

                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int totalRecords = 0;
                AttenenceRepository arepo = new AttenenceRepository();
                var res = arepo.GetProcessedAttenancedata(f);

                totalRecords = res.Count();
                var data = res.Skip(skip).Take(pageSize).ToList();

                foreach(var d in data)
                {
                    d.sHoursWorked= Convert.ToDateTime(DateTime.Now.Date + d.HoursWorked).ToString("HH:mm");
                }

                return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);

            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}