using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using OEP.Core.Services;
using OEP.Services;

namespace OEP.Web.DependencyInjection.Modules
{
    public class ServiceCapsuleModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ReferencedAssemblies.Services).
                Where(_ => _.Name.EndsWith("Service")).
                AsImplementedInterfaces().
                InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(BaseService<>)).As(typeof(IService<>)).InstancePerDependency();
        }

    }
}