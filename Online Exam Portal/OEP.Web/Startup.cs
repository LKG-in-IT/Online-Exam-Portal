using System.Web.Http;
using Microsoft.Owin;
using OEP.Web.DependencyInjection;
using Owin;
using OEP.Web.Mapping;

[assembly: OwinStartupAttribute(typeof(OEP.Web.Startup))]
namespace OEP.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);


            //Mapping
            MappingDefinitions.CreateMap();

            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            new WebCapsule().Initialise(config);

           
        }
    }
}
