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
using OEP.Web.Helpers;

namespace OEP.Web.Areas.Admin.Controllers
{
    [AuthorizeUser(Roles = "Admin")]
    public class ApplicationUsersController : Controller
    {
        private readonly IApplicationUserService _applicationUserService;

        public ApplicationUsersController(IApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
        }

        // GET: Admin/ApplicationUsers
        public ActionResult Index()
        {
            var appliationuser = _applicationUserService.GetApplicationUsers();
            var appliationuserResource = Mapper.Map<List<ApplicationUser>, List<ApplicationUserResource>>(appliationuser);

            return View(appliationuserResource);
        }

        public ActionResult LoadUsers()
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

                        x => sortColumn == "Name" ? x.Name : null,

                        //filtering

                        x => searchValue != "" ? x.Name.Contains(searchValue) : x.Id != "0",

                        //sort by
                        (sortColumnDir == "desc" ? OrderBy.Descending : OrderBy.Ascending),

                        x => x.Roles

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
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = _applicationUserService.GetById(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            var userResource = Mapper.Map<ApplicationUser, ApplicationUserResource>(applicationUser);
            return View(userResource);
        }

        // GET: Admin/ApplicationUser/Edit/
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = _applicationUserService.GetById(id);

            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            var userResource = Mapper.Map<ApplicationUser, ApplicationUserResource>(applicationUser);

            return View(userResource);
        }

        // pOST:Admin/ApplicationUser/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ApplicationUserResource applicationUserResource)
        {

            if (ModelState.IsValid)
            {
                var exstuser = _applicationUserService.GetById(applicationUserResource.Id);
                exstuser.Name = applicationUserResource.Name;
                exstuser.Email = applicationUserResource.Email;
                exstuser.PhoneNumber = applicationUserResource.PhoneNumber;
                exstuser.UserName = applicationUserResource.UserName;
                string result = _applicationUserService.Update(exstuser);
                if (result == "Success")
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

    }
}
