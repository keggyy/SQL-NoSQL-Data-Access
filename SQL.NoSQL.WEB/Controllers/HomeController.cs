using SQL.NoSQL.BLL.Common.DTO;
using SQL.NoSQL.BLL.MixedAcces.Repository;
using SQL.NoSQL.BLL.NoSQL.Repository;
using SQL.NoSQL.BLL.SQL.Repository;
using SQL.NoSQL.WEB.Models;
using System;
using System.Collections.Generic;
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

        public ActionResult SQLAccess(Guid? SelectedApp,string TextToSearch)
        {
            LogListModel model = new LogListModel();
            model.App.AddRange((new SQLAppRepository()).GetAll().OrderBy(x => x.Name));
            model.Logs.AddRange((new SQLLogRepository().Search(SelectedApp,TextToSearch).OrderBy(x => x.LogDate)));

            return View(model);
        }

        public ActionResult NoSQLAccess(Guid? SelectedApp, string TextToSearch)
        {
            LogListModel model = new LogListModel();
            model.App.AddRange((new NoSQLAppRepository()).GetAll().OrderBy(x => x.Name));
            model.Logs.AddRange((new NoSQLLogRepository().Search(SelectedApp, TextToSearch).OrderBy(x => x.LogDate)));

            return View(model);
        }

        public ActionResult MixedAccess(Guid? SelectedApp, string TextToSearch)
        {
            LogListModel model = new LogListModel();
            model.App.AddRange((new AppRepository()).GetAll().OrderBy(x => x.Name));
            model.Logs.AddRange((new LogRepository().Search(SelectedApp, TextToSearch).OrderBy(x => x.LogDate)));

            return View(model);
        }
    }
}