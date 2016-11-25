using SQL.NoSQL.BLL.Common.DTO;
using SQL.NoSQL.BLL.MixedAcces.DAL.Entity;
using SQL.NoSQL.BLL.MixedAcces.Repository;
using SQL.NoSQL.BLL.NoSQL.Repository;
using SQL.NoSQL.BLL.SQL.Repository;
using SQL.NoSQL.Library.SQL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQL.NoSQL.BLL.Common.Helper
{
    /// <summary>
    /// Add custom data for first access
    /// </summary>
    public static class DataHelper
    {

        public static void EnsureData()
        {
            SQLAppRepository appRep = new SQLAppRepository();
            SQLLogRepository SQLlogRep = new SQLLogRepository();
            NoSQLLogRepository NoSQLlogRep = new NoSQLLogRepository();
            LogRepository MixedlogRep = new LogRepository();

            List<AppDto> apps = appRep.GetAll();
            if(apps == null || apps.Count == 0)
            {
                appRep.Save(new AppDto { Name = "Applicazione 1" });
                appRep.Save(new AppDto { Name = "Applicazione 2" });
                appRep.Save(new AppDto { Name = "Applicazione 3" });
                appRep.Save(new AppDto { Name = "Applicazione 4" });
                appRep.Save(new AppDto { Name = "Applicazione 5" });

                AppDto app1 = appRep.GetByName("Applicazione 1");
                AppDto app2 = appRep.GetByName("Applicazione 2");
                AppDto app3 = appRep.GetByName("Applicazione 3");
                AppDto app4 = appRep.GetByName("Applicazione 4");
                AppDto app5 = appRep.GetByName("Applicazione 5");

                List<AppDto> listApp = appRep.GetAll();
                
                List<string> levelList = new List<string>();
                levelList.Add("Info");
                levelList.Add("Error");
                levelList.Add("Warn");
                levelList.Add("Debug");

                DateTime start = new DateTime(1995, 1, 1);
                
                int range = (DateTime.Today - start).Days;

                Random rand = new Random();
                Random randLevel = new Random();
                Random randDate = new Random();
                using (StreamReader sr = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin\\la_divin.txt"), Encoding.GetEncoding(1252)))
                {
                    using (UnitOfNhibernate op = new UnitOfNhibernate())
                    {
                        op.BeginTransaction();
                        int i = 0;
                        while (!sr.EndOfStream)
                        {
                            i++;
                            string line = sr.ReadLine();
                            AppDto app = listApp[rand.Next(0, listApp.Count - 1)];
                            string level = levelList[randLevel.Next(0, levelList.Count - 1)];
                            DateTime date = start.AddDays(randDate.Next(0, range)).AddHours(randDate.Next(0, range)).AddMilliseconds(randDate.Next(0, range)).AddMinutes(randDate.Next(0, range)).AddSeconds(randDate.Next(0, range));

                            LogEntity entity = new LogEntity { AppId = app.Id, Level = level, Message = line, LogDate = date };
                            op.SaveOrUpdate(entity);
                            if (i % 100000 == 0)
                                op.Commit();
                            //SQLlogRep.Save(new LogDto { App = app, Level = level, Message = line, LogDate = date });
                            NoSQLlogRep.Save(new LogDto { App = app, Level = level, Message = line, LogDate = date });
                            MixedlogRep.Save(new LogDto { App = app, Level = level, Message = line, LogDate = date });


                        }

                        op.Commit();
                    }
                }
                


            }
        }
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
