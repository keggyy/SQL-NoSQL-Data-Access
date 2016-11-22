using SQL.NoSQL.BLL.Common.DTO;
using SQL.NoSQL.BLL.MixedAcces.Repository;
using SQL.NoSQL.BLL.NoSQL.Repository;
using SQL.NoSQL.BLL.SQL.Repository;
using SQL.NoSQL.WEB.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SQL.NoSQL.WEB.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SQLAccess(Guid? SelectedApp, string TextToSearch,int? page)
        {
            ViewData["LogReport"] = new LogsReportModel();
            Stopwatch pageTime = new Stopwatch();
            pageTime.Start();
            LogListModel model = new LogListModel();
            model.App.AddRange((new SQLAppRepository()).GetAll().OrderBy(x => x.Name));
            model.Logs = new SQLLogRepository().Search(SelectedApp, TextToSearch, page ?? 1, 10);
            pageTime.Stop();
            model.PaginationTime = pageTime.ElapsedMilliseconds;

            return View(model);
        }

        
        public ActionResult SQLReport()
        {
            Stopwatch pageTime = new Stopwatch();
            pageTime.Start();
            LogsReportModel result = new LogsReportModel();
            result.Items.AddRange((new SQLLogRepository()).GetLogsReport());
            result.TimeReport = pageTime.ElapsedMilliseconds;
            return PartialView(result);
        }

        public ActionResult NoSQLReport()
        {
            Stopwatch pageTime = new Stopwatch();
            pageTime.Start();
            LogsReportModel result = new LogsReportModel();
            result.Items.AddRange((new NoSQLLogRepository()).GetLogsReport());
            result.TimeReport = pageTime.ElapsedMilliseconds;
            return PartialView(result);
        }

        public ActionResult MixedReport()
        {
            Stopwatch pageTime = new Stopwatch();
            pageTime.Start();
            LogsReportModel result = new LogsReportModel();
            result.Items.AddRange((new LogRepository()).GetLogsReport());
            result.TimeReport = pageTime.ElapsedMilliseconds;
            return PartialView(result);
        }

        public ActionResult NoSQLAccess(Guid? SelectedApp, string TextToSearch,int? page)
        {
            Stopwatch pageTime = new Stopwatch();
            pageTime.Start();
            LogListModel model = new LogListModel();
            model.App.AddRange((new NoSQLAppRepository()).GetAll().OrderBy(x => x.Name));
            model.Logs = new NoSQLLogRepository().Search(SelectedApp, TextToSearch, page ?? 1, 10);
            pageTime.Stop();
            model.PaginationTime = pageTime.ElapsedMilliseconds;

            return View(model);
        }

        public ActionResult MixedAccess(Guid? SelectedApp, string TextToSearch, int? page)
        {
            Stopwatch pageTime = new Stopwatch();
            pageTime.Start();
            LogListModel model = new LogListModel();
            model.App.AddRange((new AppRepository()).GetAll().OrderBy(x => x.Name));
            model.Logs = new LogRepository().Search(SelectedApp, TextToSearch, page ?? 1, 10);
            pageTime.Stop();
            model.PaginationTime = pageTime.ElapsedMilliseconds;

            return View(model);
        }
    }
}