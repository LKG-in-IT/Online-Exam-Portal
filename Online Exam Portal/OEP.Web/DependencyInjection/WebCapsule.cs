using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using OEP.Core.Data.Repository;
using OEP.Data;
using OEP.Services;
using OEP.Web.DependencyInjection.Modules;
using OEP.Web.DependencyInjection.Modules.AttributeFilter;
using OEP.Web.Helpers;

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
            builder.Register<IEntitiesContext>(b =>
            {
                var context = new OepDbContext(nameOrConnectionString);
                return context;
            }).InstancePerLifetimeScope();

            builder.RegisterModule<RepositoryCapsuleModule>();
            builder.RegisterModule<ServiceCapsuleModule>();
            builder.RegisterModule<ControllerCapsuleModule>();

            /*Abdul---Added this method to register dependancies for Filters.
            1.public IPackageService _packageService { get; set; }
            2.We need to add services like this.
            3.add class inside OEP.Web.DependencyInjection.Modules.AttributeFilter folder -->AutofacFilterAttributeFilterProvider.cs
            4.Add extension method for ContainerBuilder in the file ContainerBuilderExtension.cs*/
            builder.RegisterFilter();


            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            //For Web API
            //var resolver = new AutofacWebApiDependencyResolver(container);
            //config.DependencyResolver = resolver;
        }
    }
}