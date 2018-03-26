using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Autofac;
using Autofac.Integration.Mvc;

namespace OEP.Web.DependencyInjection.Modules
{
    public class ControllerCapsuleModule:Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register the MVC Controllers
            //builder.RegisterControllers(Assembly.Load("OEP.Web"));
            //builder.RegisterControllers(Assembly.Load("OEP.Web.Areas.Admin"));

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
        }
    }
}