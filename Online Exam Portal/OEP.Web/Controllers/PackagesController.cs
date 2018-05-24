using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using OEP.Core.DomainModels.Identity;
using OEP.Core.DomainModels.PackageModel;
using OEP.Core.Services;
using OEP.Resources.Admin;
using OEP.Resources.Common;

namespace OEP.Web.Controllers
{
    public class PackagesController : Controller
    {
        private readonly IPackageService _packageService;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
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
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public PackagesController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        public PackagesController( ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        // GET: Packages
        public async Task<ActionResult> Index()
        {
            var packagePageResource = await PackagePageResource();
            return View(packagePageResource);
        }

        private async Task<PackagePageResource> PackagePageResource()
        {
            var resp = Mapper.Map<List<Package>, List<PackageResource>>(_packageService.FindBy(x=>x.Status));
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);
            var userResource = Mapper.Map<ApplicationUser, ApplicationUserResource>(user);
            PackagePageResource packagePageResource = new PackagePageResource() {User = userResource};
            if (resp != null)
            {
                userResource.Package = resp.FirstOrDefault(x => x.Id == user.PackageId);

                if (userResource.Package != null)
                {
                    var startDate = user.StartDate;
                    var duration = userResource.Package.Duration;
                    var expiryDate = startDate.AddMonths(duration);
                    packagePageResource.ExpiryDate = expiryDate;
                    packagePageResource.Expired = DateTime.Now > expiryDate ? true : false;
                }

                packagePageResource.Packages = resp;
            }
            return packagePageResource;
            
        }


        public ActionResult Upgrade(int Id)
        {
            var currentPackage = Mapper.Map<Package, PackageResource>(_packageService.FindBy(x=>x.Id==Id).FirstOrDefault());
            return View(currentPackage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Confirm(int packageId)
        {
            var package = _packageService.GetById(packageId);
            if (package != null)
            {
                var userId = User.Identity.GetUserId();
                var userprofile = await UserManager.FindByIdAsync(userId);
                userprofile.PackageId = package.Id;
                userprofile.StartDate=DateTime.Now;
                var result = await UserManager.UpdateAsync(userprofile);
                if (result.Succeeded)
                {
                    // create a new identity 
                    var identity = new ClaimsIdentity(User.Identity);

                    // Remove the existing claim value of current user from database
                    if (identity.FindFirst("NameOfUser") != null)
                        await UserManager.RemoveClaimAsync(userprofile.Id, identity.FindFirst("NameOfUser"));
                    if (identity.FindFirst("ProfilePicture") != null)
                        await UserManager.RemoveClaimAsync(userprofile.Id, identity.FindFirst("ProfilePicture"));
                    if (identity.FindFirst("PackageId") != null)
                        await UserManager.RemoveClaimAsync(userprofile.Id, identity.FindFirst("PackageId"));

                    // Update customized claim 
                    await UserManager.AddClaimAsync(userprofile.Id, new Claim("NameOfUser", userprofile.Name));
                    await UserManager.AddClaimAsync(userprofile.Id, new Claim("ProfilePicture", userprofile.ProfilePicture));
                    await UserManager.AddClaimAsync(userprofile.Id, new Claim("PackageId", userprofile.PackageId.ToString()));


                    // the claim has been updated, We need to change the cookie value for getting the updated claim
                    AuthenticationManager.SignOut(identity.AuthenticationType);
                    await SignInManager.SignInAsync(userprofile, isPersistent: false, rememberBrowser: false);
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Upgrade",new {id=packageId});
        }
    }
}