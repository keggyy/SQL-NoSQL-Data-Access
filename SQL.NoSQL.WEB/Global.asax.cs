using SQL.NoSQL.BLL.Common.Helper;
using SQL.NoSQL.Library.SQL;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SQL.NoSQL.WEB
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            NHIbernateContext.ApplySchemaChanges();
            DataHelper.EnsureNHibernateData();
            DataHelper.EnsureMongoDData();
        }
    }
}
