using SQL.NoSQL.Library.Config;
using SQL.NoSQL.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL.NoSQL.Library.Mixed
{
    public class UnitFactory
    {
        private static Dictionary<string, Type> factory;

        public UnitFactory()
        {
            if (factory == null || factory.Count == 0)
            {
                RepositoryConfiguration configuration = RepositoryConfiguration.GetConfig();
                RepositoryCollection col = configuration.RepositoryCollection;
                IEnumerable<Repository> Repo = col.Cast<Repository>();
                factory = new Dictionary<string, Type>();
                foreach (Repository item in Repo)
                {
                    Type type = Type.GetType(item.DataBase);
                    factory.Add(item.RepositoryName, type);
                }
            }
        }

        public IUnitOfWork GetUnit(IRepository repo)
        {
            Type tipo = factory[repo.GetType().Name];
            return (IUnitOfWork)Activator.CreateInstance(tipo);
        }
    }
}
