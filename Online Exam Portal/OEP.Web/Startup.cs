using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Owin;
using OEP.Web.DependencyInjection;
using Owin;

[assembly: OwinStartup(typeof(OEP.Web.Startup))]
namespace OEP.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //https://code.msdn.microsoft.com/windowsdesktop/Architecture-real-world-8ac333a2/sourcecode?fileId=141308&pathId=366130227
            //Code sample

            ConfigureAuth(app);

            //Mapping
            //var mappingDefinitions = new MappingDefinitions();
            // mappingDefinitions.Initialise();

            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            //WebApiConfig.Register(config);

            new WebCapsule().Initialise(config);

            //app.UseWebApi(config);
        }
    }
}