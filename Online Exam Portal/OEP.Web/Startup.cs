using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OEP.Web.Startup))]
namespace OEP.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
