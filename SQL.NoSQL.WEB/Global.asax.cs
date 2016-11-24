using SQL.NoSQL.BLL.Common.Helper;
using SQL.NoSQL.Library.SQL;
using System.Web.Http;
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
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            NHIbernateContext.ApplySchemaChanges();
            DataHelper.EnsureData();
            //DataHelper.EnsureMongoDData();
        }
    }
}
