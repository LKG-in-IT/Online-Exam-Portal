using System.Data.Entity;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using OEP.Core.Data.Repository;
using OEP.Data;
using OEP.Web.DependencyInjection.Modules;

namespace OEP.Web.DependencyInjection
{
    public class WebCapsule
    {
        public void Initialise(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType(typeof(UnitOfWork)).As(typeof(IUnitOfWork)).InstancePerLifetimeScope();

            builder.RegisterFilterProvider();

            const string nameOrConnectionString = "name=OepDbConnection";
            builder.Register<DbContext>(b =>
            {
                var context = new OepDbContext(nameOrConnectionString);
                return context;
            }).InstancePerLifetimeScope();

            builder.RegisterModule<RepositoryCapsuleModule>();
            builder.RegisterModule<ServiceCapsuleModule>();
            builder.RegisterModule<ControllerCapsuleModule>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            //For Web API
            //var resolver = new AutofacWebApiDependencyResolver(container);
            //config.DependencyResolver = resolver;
        }
    }
}