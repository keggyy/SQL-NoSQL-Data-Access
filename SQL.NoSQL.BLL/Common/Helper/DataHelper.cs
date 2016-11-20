using SQL.NoSQL.BLL.Common.DTO;
using SQL.NoSQL.BLL.MixedAcces.Repository;
using SQL.NoSQL.BLL.NoSQL.Repository;
using SQL.NoSQL.BLL.SQL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL.NoSQL.BLL.Common.Helper
{
    /// <summary>
    /// Add custom data for first access
    /// </summary>
    public static class DataHelper
    {
        public static void EnsureNHibernateData()
        {
            SQLAppRepository appRep = new SQLAppRepository();
            SQLLogRepository logRep = new SQLLogRepository();

            List<AppDto> apps = appRep.GetAll();
            if(apps==null || apps.Count == 0)
            {
                appRep.Save(new AppDto { Name = "Applicazione 1" });
                appRep.Save(new AppDto { Name = "Applicazione 2" });

                AppDto app1 = appRep.GetByName("Applicazione 1");
                AppDto app2 = appRep.GetByName("Applicazione 2");

                logRep.Save(new LogDto { App = app1, Level = "Info", LogDate = DateTime.Now, Message = "Test message 1" });
                logRep.Save(new LogDto { App = app1, Level = "Warn", LogDate = DateTime.Now, Message = "Test message 2" });
                logRep.Save(new LogDto { App = app1, Level = "Info", LogDate = DateTime.Now, Message = "Test message 3" });
                logRep.Save(new LogDto { App = app1, Level = "Error", LogDate = DateTime.Now, Message = "Test message 4" });

                logRep.Save(new LogDto { App = app2, Level = "Info", LogDate = DateTime.Now, Message = "Test message 5" });
                logRep.Save(new LogDto { App = app2, Level = "Error", LogDate = DateTime.Now, Message = "Test message 6" });
                logRep.Save(new LogDto { App = app2, Level = "Info", LogDate = DateTime.Now, Message = "Test message 7" });
                logRep.Save(new LogDto { App = app2, Level = "Warn", LogDate = DateTime.Now, Message = "Test message 8" });
            } 
        }

        public static void EnsureMongoDData()
        {
            NoSQLLogRepository repo = new NoSQLLogRepository();
            LogRepository repoMixe = new LogRepository();
            SQLAppRepository appRep = new SQLAppRepository();

            List<LogDto> list = repo.GetAll();

            if (list == null || list.Count == 0)
            {
                //Per mantenere gli id uguali sui due db
                AppDto app1 = appRep.GetByName("Applicazione 1");
                AppDto app2 = appRep.GetByName("Applicazione 2");

                repo.Save(new LogDto { App = app1, Level = "Info", LogDate = DateTime.Now, Message = "Test message 1" });
                repo.Save(new LogDto { App = app1, Level = "Warn", LogDate = DateTime.Now, Message = "Test message 2" });
                repo.Save(new LogDto { App = app1, Level = "Info", LogDate = DateTime.Now, Message = "Test message 3" });
                repo.Save(new LogDto { App = app1, Level = "Error", LogDate = DateTime.Now, Message = "Test message 4" });

                repo.Save(new LogDto { App = app2, Level = "Info", LogDate = DateTime.Now, Message = "Test message 5" });
                repo.Save(new LogDto { App = app2, Level = "Error", LogDate = DateTime.Now, Message = "Test message 6" });
                repo.Save(new LogDto { App = app2, Level = "Info", LogDate = DateTime.Now, Message = "Test message 7" });
                repo.Save(new LogDto { App = app2, Level = "Warn", LogDate = DateTime.Now, Message = "Test message 8" });

                repoMixe.Save(new LogDto { App = app1, Level = "Info", LogDate = DateTime.Now, Message = "Test message 1" });
                repoMixe.Save(new LogDto { App = app1, Level = "Warn", LogDate = DateTime.Now, Message = "Test message 2" });
                repoMixe.Save(new LogDto { App = app1, Level = "Info", LogDate = DateTime.Now, Message = "Test message 3" });
                repoMixe.Save(new LogDto { App = app1, Level = "Error", LogDate = DateTime.Now, Message = "Test message 4" });

                repoMixe.Save(new LogDto { App = app2, Level = "Info", LogDate = DateTime.Now, Message = "Test message 5" });
                repoMixe.Save(new LogDto { App = app2, Level = "Error", LogDate = DateTime.Now, Message = "Test message 6" });
                repoMixe.Save(new LogDto { App = app2, Level = "Info", LogDate = DateTime.Now, Message = "Test message 7" });
                repoMixe.Save(new LogDto { App = app2, Level = "Warn", LogDate = DateTime.Now, Message = "Test message 8" });
            }
        }
    }
}
