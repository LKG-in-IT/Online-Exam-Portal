using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OEP.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Login",
                url: "login",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
                namespaces: new[] { "OEP.Web.Controllers" }
            );

            routes.MapRoute(
               name: "UnAuthorized",
               url: "unauthorized",
               defaults: new { controller = "Account", action = "UnAuthorized", id = UrlParameter.Optional },
               namespaces: new[] { "OEP.Web.Controllers" }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "OEP.Web.Controllers" }
            );
        }
    }
}
