using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OEP.Core.DomainModels.Identity;
using OEP.Data;
using OEP.Core.Services;
using AutoMapper;
using OEP.Resources.Admin;
using OEP.Core.Data;
using OEP.Core.DomainModels;
using OEP.Web.Models;
using OEP.Web.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace OEP.Web.Areas.Admin.Controllers
{
    public class ApplicationUsersController : Controller
    {
        private readonly IApplicationUserService _applicationUserService;
   

       

        public ApplicationUsersController(IApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
         
          
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        // GET: Admin/ApplicationUsers
        public  ActionResult Index()
        {
            var appliationuser = _applicationUserService.GetApplicationUsers();
            var appliationuserResource = Mapper.Map<List< ApplicationUser>,List< ApplicationUserResource>>(appliationuser);

            return View(appliationuserResource);
        }



        public  ActionResult LoadUsers()
        {
            try
            {
                if (Request.Form != null)
                {
                    var draw = Request.Form.GetValues("draw").FirstOrDefault();
                    var start = Request.Form.GetValues("start").FirstOrDefault();
                    var length = Request.Form.GetValues("length").FirstOrDefault();
                    var sortColumn =
                        Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                    var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                    var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();


                    //Paging Size (10,20,50,100)    
                    int pageSize = length != null ? Convert.ToInt32(length) : 0;
                    int skip = start != null ? Convert.ToInt32(start) : 1;
                    int recordsTotal = 0;

                    var userList = _applicationUserService.GetApplicationUsers
                        (
                        skip,
                        pageSize, 
                        
                        x => sortColumn == "Name" ? x.Name : (sortColumn=="UserName"?x.UserName:null),

                        //filtering
                        
                        x => searchValue != "" ? x.Name.Contains(searchValue) : x.Id != "0",

                        //sort by
                        (sortColumnDir == "desc" ? OrderBy.Descending : OrderBy.Ascending)

                       

                        );


             

                    var resp = Mapper.Map<List<ApplicationUser>, List<ApplicationUserResource>>(userList);
               
                    //total number of rows count     
                    recordsTotal = userList.TotalCount;
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = resp });
                }
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
            return Content("Error");
        }

        // GET: Admin/ApplicationUser/Details/5
        public ActionResult Details()
        {
            string UserName = Request.QueryString["UserName"].ToString();
            if (UserName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = _applicationUserService.GetById(UserName);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            var userResource = Mapper.Map<ApplicationUser, ApplicationUserResource>(applicationUser);
            return View(userResource);
        }

        // GET: Admin/ApplicationUser/Edit/
        public ActionResult Edit()
        {
            string UserName = Request.QueryString["UserName"].ToString();
            if (UserName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = _applicationUserService.GetById(UserName);

            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            var userResource = Mapper.Map<ApplicationUser, ApplicationUserResource>(applicationUser);

            return View(userResource);
        }

        // POST:Admin/ApplicationUser/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
       public ActionResult Edit(ApplicationUserResource applicationUserResource)
        {

            if(ModelState.IsValid)
            {
                var exstuser = _applicationUserService.GetById(applicationUserResource.UserName);
                exstuser.Name = applicationUserResource.Name;
                exstuser.Email = applicationUserResource.Email;
                exstuser.PhoneNumber = applicationUserResource.PhoneNumber;
                exstuser.UserName = applicationUserResource.UserName;
                string result = _applicationUserService.Update(exstuser);
             if(result=="Success")
                {


                    return RedirectToAction("Index");
                }
             else
                {
                    return View(applicationUserResource);
                }

            }
            return View();

        }

        public ActionResult Disable()
        {
            string UserName = Request.QueryString["UserName"].ToString();
            if (UserName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = _applicationUserService.GetById(UserName);

            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            var userResource = Mapper.Map<ApplicationUser, ApplicationUserResource>(applicationUser);

            return View(userResource);
        }

        // POST:Admin/ApplicationUser/Disable/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disable(ApplicationUserResource applicationUserResource)
        {

           
                var exstuser = _applicationUserService.GetById(applicationUserResource.UserName);
            if(exstuser.Status==true)
                {
                    exstuser.Status = false;
                }
            else
                {
                    exstuser.Status = true;
                }
                string result = _applicationUserService.UpdateStatus(exstuser);
                if (result == "Success")
                {


                    return RedirectToAction("Index");
                }
                else
                {
                    return View(applicationUserResource);
                }

           
          

        }


        //GET :Admin/ApplicationUser/AddRoles/

        public ActionResult AddRoles()
        {

            string UserName = Request.QueryString["UserName"].ToString();
            if (UserName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = _applicationUserService.GetById(UserName);

            string[] selectedrole = applicationUser.Role.Split(',');

            ViewBag.Selectedrole = selectedrole;
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            var userResource = Mapper.Map<ApplicationUser, ApplicationUserResource>(applicationUser);
            var allroles = _applicationUserService.GetRoles().ToList();
          



            ViewBag.RolesList = allroles;
            return View(userResource);

   

        }

   


        // POST:Admin/ApplicationUser/AddRoles/
        [HttpPost]
      
        public JsonResult AddRolespost(string username,string roles,string names)
        {
            string[] nameslist = names.Split(',');
            _applicationUserService.DeleteRoles(username);

            foreach (string item in nameslist)
            {
                _applicationUserService.UpdateRole(username, item);
            }

          


            return Json("Added");

        }

        // GET: Admin/ApplicationUser/AddUser

        public ActionResult AddUser()
        {
            var allroles = _applicationUserService.GetRoles().ToList();
            ViewBag.RolesList = allroles;
            return View();
        }

        // POST:Admin/ApplicationUser/AddUserPost/
        [HttpPost]

        public JsonResult AddUserPost(ApplicationUserResource applicationUserResource)
        {


            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {

                    UserName = applicationUserResource.Email,
                    Email = applicationUserResource.Email,
                    Name = applicationUserResource.Name,
                    Role = applicationUserResource.Role,
                 
                    PhoneNumber = applicationUserResource.PhoneNumber
                };


            

                    return Json(_applicationUserService.AddUser(user));
                }
            return Json("error");

        }



        // GET: Admin/ApplicationUser/ResetPassword

        public ActionResult ResetPassword()
        {
            string UserName = Request.QueryString["UserName"].ToString();
            if (UserName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = _applicationUserService.GetById(UserName);

            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            var userResource = Mapper.Map<ApplicationUser, ApplicationUserResource>(applicationUser);

            return View(userResource);
        }

        // GET: Admin/ApplicationUser/ResetPassword
        [HttpPost]
        public ActionResult ResetPassword(ApplicationUserResource applicationUserResource)
        {

            string result = _applicationUserService.ResetPassword(applicationUserResource.UserName);

            if(result=="Changed")
            {

                return RedirectToAction("Index");
            }

            else
            {
                return View(applicationUserResource);

            }



           
        }
    }

}

