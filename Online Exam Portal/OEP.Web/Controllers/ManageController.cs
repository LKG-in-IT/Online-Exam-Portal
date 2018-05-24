using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using OEP.Core.DomainModels.EducationModels;
using OEP.Core.DomainModels.Identity;
using OEP.Core.DomainModels.PackageModel;
using OEP.Core.Services;
using OEP.Resources.Admin;
using OEP.Resources.Common;
using OEP.Web.Helpers;
using OEP.Web.Models;

namespace OEP.Web.Controllers
{
    [AuthorizeUser(Roles = "User,Faculty,Admin")]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly IService<EducationType> _educationTypeService;
        private readonly IService<YearDetails> _YearDetailsService;
        private readonly IService<EducationDetails> _EducationDetailsService;
        private readonly IPackageService _packageService;

        public ManageController(IService<EducationType> educationTypeService, IPackageService packageService, IService<YearDetails> YearDetailsService, IService<EducationDetails> EducationDetailsService)
        {
            _educationTypeService = educationTypeService;
            _YearDetailsService = YearDetailsService;
            _EducationDetailsService = EducationDetailsService;
            _packageService = packageService;
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;

        }

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

        // Add Education Details
        public PartialViewResult EducationDetails()
        {
            var typelist =  _educationTypeService.GetAll();
            ViewBag.EducationTypeId = new SelectList(typelist.Where(i => i.Status == true), "Id", "Name");

            var yearlist =  _YearDetailsService.GetAll();
            ViewBag.YearFromId = new SelectList(yearlist.Where(i => i.Status == true), "Id", "Year");
            var userid = System.Web.HttpContext.Current.User.Identity.GetUserId();

            return PartialView();
        }

        // Add Education Details post
        [HttpPost]
        public async Task< JsonResult> AddEducationDetails(EducationDetailsResource educationDetailsResource)
        {
            if (ModelState.IsValid)
            {
                var userid = System.Web.HttpContext.Current.User.Identity.GetUserId();

                var educationdetails = Mapper.Map<EducationDetailsResource, EducationDetails>(educationDetailsResource);
                educationdetails.EducationTypeId = educationDetailsResource.EducationTypeId;
                educationdetails.InstituteName = educationDetailsResource.InstituteName;
                educationdetails.YearFromId = educationDetailsResource.YearFromId;
                educationdetails.CreatedDate = DateTime.Now;
                educationdetails.UpdatedDate = DateTime.Now;
                educationdetails.UserId = userid;
                educationdetails.YearToId = educationDetailsResource.YearToId;
                educationdetails.ApplicationUserId = userid;
                educationdetails.Status = true;

              await  _EducationDetailsService.AddAsync(educationdetails);
                _EducationDetailsService.UnitOfWorkSaveChanges();
                return Json("Success");
            }


            return Json("Error");

        }
        
        //Edit Education Details

        [HttpPost]
        public async Task< JsonResult> EditEducationDetails(EducationDetailsResource educationDetailsResource)
        {
            var educationdetails = await _EducationDetailsService.GetByIdAsync(educationDetailsResource.Id);
            if (ModelState.IsValid)
            {
                var userid = System.Web.HttpContext.Current.User.Identity.GetUserId();

              
                educationdetails.EducationTypeId = educationDetailsResource.EducationTypeId;
                educationdetails.InstituteName = educationDetailsResource.InstituteName;
                educationdetails.YearFromId = educationDetailsResource.YearFromId;

                educationdetails.UpdatedDate = DateTime.Now;
                educationdetails.UserId = userid;
                educationdetails.YearToId = educationDetailsResource.YearToId;
                educationdetails.ApplicationUserId = userid;
                educationdetails.Status = true;

              await  _EducationDetailsService.UpdateAsync(educationdetails);
                _EducationDetailsService.UnitOfWorkSaveChanges();
                return Json("Suceess");
            }


            return Json("Error");

        }

        //Delete Education Details

        [HttpPost]
        public async Task<JsonResult> DeleteEducationDetails(EducationDetailsResource educationDetailsResource)
        {
            var educationdetails = await _EducationDetailsService.GetByIdAsync(educationDetailsResource.Id);
            if (ModelState.IsValid)
            {
                var userid = System.Web.HttpContext.Current.User.Identity.GetUserId();


                await _EducationDetailsService.DeleteAsync(educationdetails);
                _EducationDetailsService.UnitOfWorkSaveChanges();
                return Json("Deleted Successfully");
            }


            return Json("Error");

        }
        //Education Details List

        public async Task< ActionResult> Educationdetailstable()
        {

            var typelist = await _educationTypeService.GetAllAsync();
            ViewBag.EducationTypeId = typelist;

            var yearlist = await _YearDetailsService.GetAllAsync();
            ViewBag.YearFromId = yearlist;



            var userid = System.Web.HttpContext.Current.User.Identity.GetUserId();
     

            var res = _EducationDetailsService.FindByAsync(i => i.ApplicationUserId == userid );
            if (res.Result.Any())
            {
               

                var exsteducationlist = await _EducationDetailsService.FindByAsync(x => x.UserId == userid);
               

                var educationDetailsResource = Mapper.Map<List<EducationDetails>, List<EducationDetailsResource>>(exsteducationlist);
               

                return View(educationDetailsResource);


            }
            else
            {

                return View(new List<EducationDetailsResource>());
            }
        }
       

        public async Task<PartialViewResult> UserProfile()
        {
            var resp = Mapper.Map<List<Package>, List<PackageResource>>(_packageService.FindBy(x => x.Status));
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);
            var userResource = Mapper.Map<ApplicationUser, ApplicationUserResource>(user);
            PackagePageResource packagePageResource = new PackagePageResource() { User = userResource };
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
            return PartialView(packagePageResource);
        }
        [HttpPost]
        public async Task<ActionResult> UserProfile(ApplicationUser applicationUser, HttpPostedFileBase file)
        {
            var message = ManageMessageId.Error;

            var userprofile = await UserManager.FindByIdAsync(applicationUser.Id);
            userprofile.Name = applicationUser.Name;
            userprofile.Gender = applicationUser.Gender;
            userprofile.DatOfBirth = applicationUser.DatOfBirth;
            userprofile.Address = applicationUser.Address;

            if (string.IsNullOrEmpty(userprofile.Email))
            {
                userprofile.Email = userprofile.UserName;
            }

            if (file!=null && file.ContentLength > 0)
            {
                //delete existing one 
                if (!string.IsNullOrEmpty(userprofile.ProfilePicture))
                {
                    var filePath = Server.MapPath(userprofile.ProfilePicture);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                        userprofile.ProfilePicture = "";
                    }
                }
                // code for saving the image file to a physical location.
                var fileName = Path.GetFileName(file.FileName);
                if (fileName != null)
                {
                    var path = Path.Combine(Server.MapPath("~/Uploads/ProfileImages"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    file.SaveAs(path);
                    // prepare a relative path to be stored in the database and used to display later on.
                    userprofile.ProfilePicture = Url.Content(Path.Combine("~/Uploads/ProfileImages", fileName));
                }
                
            }

            var result = await UserManager.UpdateAsync(userprofile);
            if (result.Succeeded)
            {
                // create a new identity 
                var identity = new ClaimsIdentity(User.Identity);

                // Remove the existing claim value of current user from database
                if(identity.FindFirst("NameOfUser")!=null)
                    await UserManager.RemoveClaimAsync(userprofile.Id, identity.FindFirst("NameOfUser"));
                if (identity.FindFirst("ProfilePicture") != null)
                    await UserManager.RemoveClaimAsync(userprofile.Id, identity.FindFirst("ProfilePicture"));
                if (identity.FindFirst("PackageId") != null)
                    await UserManager.RemoveClaimAsync(userprofile.Id, identity.FindFirst("PackageId"));

                // Update customized claim 
                await UserManager.AddClaimAsync(userprofile.Id, new Claim("NameOfUser", userprofile.Name));
                await UserManager.AddClaimAsync(userprofile.Id, new Claim("ProfilePicture", userprofile.ProfilePicture));
                await UserManager.AddClaimAsync(userprofile.Id, new Claim("PackageId", userprofile.PackageId.ToString()));


                // the claim has been updates, We need to change the cookie value for getting the updated claim
                AuthenticationManager.SignOut(identity.AuthenticationType);
                await SignInManager.SignInAsync(userprofile, isPersistent: false, rememberBrowser: false);
                
                return RedirectToAction("Index", new { Message = ManageMessageId.ProfileUpdated });
            }
            return RedirectToAction("Index", new { Message = message });

        }

        [HttpPost]
        public async Task<string> DeleteUserProfilePicture()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var user = await UserManager.FindByIdAsync(userId);
                if (!string.IsNullOrEmpty(user.ProfilePicture))
                {
                    var filePath = Server.MapPath(user.ProfilePicture);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);

                        user.ProfilePicture = "";
                        var result = await UserManager.UpdateAsync(user); // save profile picture path as blank
                        if (result.Succeeded)
                        {
                            return JsonConvert.SerializeObject(new ResponseContent<string>() { Status = "Success", Message = "The profile picture successfully removed!", Result = "" });
                        }
                    }
                }
            }
            catch (Exception)
            {
                return JsonConvert.SerializeObject(new ResponseContent<string>() { Status = "Error", Message = "Something went wrong", Result = "" });
            }
            return JsonConvert.SerializeObject(new ResponseContent<string>() { Status = "Error", Message = "Something went wrong", Result = "" });
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : message == ManageMessageId.ProfileUpdated ? "Your profile has been successfully updated."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error,
            ProfileUpdated
        }

        #endregion
    }
}