using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using OEP.Core.DomainModels.Identity;

namespace OEP.Data.IdentityConfig
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            // Allows cors for the /token endpoint this is different from webapi endpoints. 
            // We have added enable cors method in WebApiConfig.cs for other requests
            //*****************************************Added  BY Abdul
            //*******************Access-Control-Allow-Origin
            //if (context.Request.Method != "OPTIONS" && context.Request.Path.Value == "/token")
            //{
            //    context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });// <-- This is the line you need
            //}
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<OepDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
     
    }

    public class AppRole : IdentityRole
    {
        public AppRole() : base() { }
        public AppRole(string name) : base(name) { }
        // extra properties here 
    }
}