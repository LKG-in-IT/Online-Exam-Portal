using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using OEP.Core.Services;

namespace OEP.Web.Helpers
{
    public class ValidatePackageStatus: ActionFilterAttribute
    {
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;
        public IPackageService _packageService { get; set; }


        public ValidatePackageStatus()
        {
           
        }

        public ValidatePackageStatus(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;

        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var userprofile = UserManager.FindById(userId);

            var packageId = userprofile.PackageId;

            var package = _packageService.GetById(packageId);

            if (package != null)
            {
                var startDate = userprofile.StartDate;
                var duration = package.Duration;
                var expiryDate = startDate.AddMonths(duration);
                if (DateTime.Now > expiryDate)
                {
                    filterContext.Result = new RedirectResult("/Packages");
                }
            }

            
        }

    }
}