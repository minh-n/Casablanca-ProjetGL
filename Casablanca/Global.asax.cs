using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Casablanca.Models;
using Casablanca.Models.Database;

namespace Casablanca
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            IDatabaseInitializer<DatabaseContext> init = new DatabaseInitialization();
            Database.SetInitializer(init);
            init.InitializeDatabase(new DatabaseContext());
            Dal d = new Dal();
            d.InitializeDatabase();
        }
    }
}
