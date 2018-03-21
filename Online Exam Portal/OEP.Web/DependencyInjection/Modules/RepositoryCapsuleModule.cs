using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using OEP.Core.Data.Repository;
using OEP.Data.Repo;

namespace OEP.Web.DependencyInjection.Modules
{
    public class RepositoryCapsuleModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ReferencedAssemblies.Repositories).
                Where(_ => _.Name.EndsWith("Repository")).
                AsImplementedInterfaces().
                InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(EntityRepository<>)).As(typeof(IRepository<>)).InstancePerDependency();
        }

    }
}