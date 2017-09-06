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
using System.Text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.text;

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
        public JsonResult ProceedAttenanceData(string Date)
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
                    p.ActualSalary = at.ActualSalary;
                    p.AduitingSalary = at.AduitingSalary;
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
                        else if (Convert.ToDouble(ts.Hours + (Convert.ToDecimal(ts.Minutes) / 100)) > 4.5 && ts.Hours < 9)
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
        public JsonResult ProceedAttenanceJson(string Date)
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

                foreach (var d in data)
                {
                    d.sHoursWorked = Convert.ToDateTime(DateTime.Now.Date + d.HoursWorked).ToString("HH:mm");
                }

                return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);

            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SalaryInfo()
        {
            CommonRepository _repo = new CommonRepository();
            ViewBag.ListWAGESTYPE = _repo.ListWAGESTYPE();
            return View();
        }

        [HttpPost]
        public JsonResult GetSalaryDataJson()
        {
            if (Session[Constances.UserId] != null)
            {
                filter f = new filter();
                f.Wagetype = Request.Form["Wagetype"].ToString() != "" ? Convert.ToInt32(Request.Form["Wagetype"]) : 0;
                f.StartDate = Request.Form["StartDate"].ToString() != "" ? Convert.ToDateTime(Request.Form["StartDate"]) : DateTime.Now.AddDays(-1);
                f.EndDate = Request.Form["EndDate"].ToString() != "" ? Convert.ToDateTime(Request.Form["EndDate"]) : DateTime.Now.AddDays(-1);
                f.Mode = Session[Constances.Mode].ToString();

                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int totalRecords = 0;

                CommonRepository repo = new CommonRepository();
                var res = repo.GetSalaryData(f);

                totalRecords = res.Count();
                var data = res.Skip(skip).Take(pageSize).ToList();



                return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);

            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CreatePDF(int Wagetype, string StartDate, string EndDate)
        {

            filter f = new filter();
            f.Wagetype = Wagetype;
            f.StartDate = Convert.ToDateTime(StartDate);
            f.EndDate = Convert.ToDateTime(EndDate);
            f.Mode = Session[Constances.Mode].ToString();

            CommonRepository repo = new CommonRepository();
            var res = repo.GetSalaryData(f);

            StringBuilder sb = new StringBuilder();
            //sb.Append("<html>");
            //sb.Append("<head><title>test</title>");
            //sb.Append("<style type='text / css'>");
            //sb.Append(".responstable {");
            //sb.Append("margin: 1em 0;");
            //sb.Append("width: 100%;");
            //sb.Append("  overflow: hidden;");
            //sb.Append("  background: #FFF;");
            //sb.Append("  color: #024457;");
            //sb.Append("  border-radius: 10px;");
            //sb.Append("  border: 1px solid #167F92;");
            //sb.Append("}");
            //sb.Append(".responstable tr {");
            //sb.Append("  border: 1px solid #D9E4E6;");
            //sb.Append("}");
            //sb.Append(".responstable tr:nth-child(odd) {");
            //sb.Append("  background-color: #EAF3F3;");
            //sb.Append("}");
            //sb.Append(".responstable th {");
            //sb.Append("  display: none;");
            //sb.Append("  border: 1px solid #FFF;");
            //sb.Append("  background-color: #167F92;");
            //sb.Append("  color: #FFF;");
            //sb.Append("  padding: 1em;");
            //sb.Append("}");
            //sb.Append(".responstable th:first-child {");
            //sb.Append("  display: table-cell;");
            //sb.Append("  text-align: center;");
            //sb.Append("}");
            //sb.Append(".responstable th:nth-child(2) {");
            //sb.Append("  display: table-cell;");
            //sb.Append("}");
            //sb.Append(".responstable th:nth-child(2) span {");
            //sb.Append("  display: none;");
            //sb.Append("}");
            //sb.Append(".responstable th:nth-child(2):after {");
            //sb.Append("  content: attr(data-th);");
            //sb.Append("}");
            //sb.Append("@media (min-width: 480px) {");
            //sb.Append("  .responstable th:nth-child(2) span {");
            //sb.Append("    display: block;");
            //sb.Append("  }");
            //sb.Append("  .responstable th:nth-child(2):after {");
            //sb.Append("    display: none;");
            //sb.Append("  }");
            //sb.Append("}");
            //sb.Append(".responstable td {");
            //sb.Append("  display: block;");
            //sb.Append("  word-wrap: break-word;");
            //sb.Append("  max-width: 7em;");
            //sb.Append("}");
            //sb.Append(".responstable td:first-child {");
            //sb.Append("  display: table-cell;                                                                             ");
            //sb.Append("  text-align: center;");
            //sb.Append("  border-right: 1px solid #D9E4E6;");
            //sb.Append("}");
            //sb.Append("@media (min-width: 480px) {");
            //sb.Append("  .responstable td {");
            //sb.Append("    border: 1px solid #D9E4E6;");
            //sb.Append("  }");
            //sb.Append("}");
            //sb.Append(".responstable th, .responstable td {");
            //sb.Append("  text-align: left;");
            //sb.Append("  margin: .5em 1em;");
            //sb.Append("}");
            //sb.Append("@media (min-width: 480px) {");
            //sb.Append("  .responstable th, .responstable td {");
            //sb.Append("    display: table-cell;");
            //sb.Append("    padding: 1em;");
            //sb.Append("  }");
            //sb.Append("}");
            //sb.Append("</style>");
            //sb.Append("</head>");
            //sb.Append("<body>");
            sb.Append("<div><table style='width: 100%;'>");
            sb.Append("<tbody><tr><td align='center'>Employee Salary Report</td></tr> ");
            sb.Append("<tr><td align='center'> Duration between " + StartDate + " to " + EndDate + "</td></tr>");
            sb.Append("</tbody></table></div>");
            sb.Append("<div>");
            sb.Append("<table width='100%' border='1' class='responstable'>");
            sb.Append("<tr>");
            sb.Append("<td>EmpCode</td>");
            sb.Append("<td>Employee Name</td>");
            sb.Append("<td>UnitName</td>");
            sb.Append("<td>Department</td>");
            sb.Append("<td>Designation</td>");
            sb.Append("<td>ShiftCount</td>");
            sb.Append("<td>Per Day Salary</td>");
            sb.Append("<td>Salary</td>");
            sb.Append("</tr>");

            foreach (var r in res)
            {
                sb.Append("<tr>");
                sb.Append("<td>" + r.EMPCODE + "</td>");
                sb.Append("<td>" + r.EMP_NAME + " Name</td>");
                sb.Append("<td>" + r.UnitName + "</td>");
                sb.Append("<td>" + r.Department + "</td>");
                sb.Append("<td>" + r.Designation + "</td>");
                sb.Append("<td>" + r.ShiftCount + "</td>");
                sb.Append("<td>" + r.Perdaysalary + "</td>");
                sb.Append("<td>" + r.Salary + "</td>");
                sb.Append("</tr>");
            }

            sb.Append("</table>");
            sb.Append("</div>");

            //StringReader sr = new StringReader(sb.ToString());
            //Document pdfDoc = new Document(PageSize.A4.Rotate(), 5f, 5f, 5f, 5f);
            //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);

            //using (MemoryStream memoryStream = new MemoryStream())
            //{
            //    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
            //    pdfDoc.Open();
            //    htmlparser.Parse(sr);
            //    pdfDoc.Close();

            //    byte[] bytes = memoryStream.ToArray();
            //    memoryStream.Close();

            //    string strPDFFileName = string.Format("Salary Report" + DateTime.Now.ToString("ddMMyyyy hh:mm:tt") + ".pdf");
            //    return File(memoryStream, "application/pdf", strPDFFileName);
            //}


            Document pdfDoc = new Document(PageSize.A4, 5f, 5f, 5f, 5f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            MemoryStream memoryStream = new MemoryStream();

            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
            writer.CloseStream = false;
            pdfDoc.Open();

            StringReader sr = new StringReader(sb.ToString());
            htmlparser.Parse(sr);

            pdfDoc.Close();
            byte[] bytes = memoryStream.ToArray();
            memoryStream.Write(bytes, 0, bytes.Length);
            memoryStream.Position = 0;

            string strPDFFileName = string.Format("Salary Report" + DateTime.Now.ToString("ddMMyyyy hh:mm:tt") + ".pdf");
            return File(memoryStream, "application/pdf", strPDFFileName);

        }

    }
}