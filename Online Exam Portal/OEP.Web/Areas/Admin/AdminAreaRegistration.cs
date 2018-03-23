using System.Web.Mvc;

namespace OEP.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new {
                    area = "Admin",
                    controller = "Home",
                    action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "OEP.Web.Areas.Admin.Controllers" }
            );
        }
    }
}