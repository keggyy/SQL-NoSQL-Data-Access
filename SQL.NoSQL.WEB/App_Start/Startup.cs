using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//SQL.NoSQL.WEB.App_Start.Startup

namespace SQL.NoSQL.WEB.App_Start
{
    
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}