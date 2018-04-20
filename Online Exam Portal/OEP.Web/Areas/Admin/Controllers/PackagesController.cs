using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OEP.Core.DomainModels.PackageModel;
using OEP.Data;
using OEP.Core.Services;
using AutoMapper;
using OEP.Resources.Admin;
using Microsoft.AspNet.Identity;
using OEP.Core.Data;

namespace OEP.Web.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    public class PackagesController : Controller
    {
        private readonly IPackageService _packageService;
        public PackagesController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        // GET: Admin/Packages
        public ActionResult Index()
        {
         
            return View();
        }

        // POST: Admin/Packages/LoadPackages
        public async Task<ActionResult> LoadPackages()
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

                    var packageList = await _packageService.GetAllAsync(
                        skip,
                        pageSize,

                        //sorting
                        x => sortColumn == "Name" ? x.Name : (sortColumn== "Prize"?x.Prize.ToString():(sortColumn== "Duration"?x.Duration:null)),

                        //filtering
                        x => searchValue != "" ? x.Name.Contains(searchValue) : x.Id != 0,

                        //sort by
                        (sortColumnDir == "desc" ? OrderBy.Descending : OrderBy.Ascending)
                     );

                    var resp = Mapper.Map<List<Package>, List<PackageResource>>(packageList);

                    //total number of rows count     
                    recordsTotal = packageList.TotalCount;
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = resp });
                }
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
            return Content("Error");
        }


        // GET: Admin/Packages/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Package package = await _packageService.GetByIdAsync(Convert.ToInt32(id));
            if (package == null)
            {
                return HttpNotFound();
            }
            var packageresource = Mapper.Map<Package, PackageResource>(package);

            return View(packageresource);
        }

        // GET: Admin/Packages/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Admin/Packages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PackageResource packageResource)
        {
            
            if (ModelState.IsValid)
            {
                var packages = Mapper.Map<PackageResource, Package>(packageResource);
                var userid=System.Web.HttpContext.Current.User.Identity.GetUserId();
                packages.CreatedDate = DateTime.Now;
                packages.UpdatedDate = DateTime.Now;
                packages.UserId = userid;
                await _packageService.AddAsync(packages);
          _packageService.UnitOfWorkSaveChanges();
                return RedirectToAction("Index");
            }

            return View(packageResource);
        }

        // GET: Admin/Packages/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Package package = await _packageService.GetByIdAsync(Convert.ToInt32(id));
            if (package == null)
            {
                return HttpNotFound();
            }
            var packageresource = Mapper.Map<Package, PackageResource>(package);
            return View(packageresource);
        }

        // POST: Admin/Packages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PackageResource packageResource)
        {
            if (ModelState.IsValid)
            {
                var exstpackage = await _packageService.GetByIdAsync(packageResource.Id);
                exstpackage.Name = packageResource.Name;
                exstpackage.Details = packageResource.Details;
                exstpackage.Prize = packageResource.Prize;
                exstpackage.Duration = packageResource.Duration;
                exstpackage.UpdatedDate = DateTime.Now;
               
                    exstpackage.Status = packageResource.Status;
                
                await _packageService.UpdateAsync(exstpackage);
                _packageService.UnitOfWorkSaveChanges();
                return RedirectToAction("Index");
            }
            return View(packageResource);
        }

        // GET: Admin/Packages/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Package package = await _packageService.GetByIdAsync(Convert.ToInt32(id));
            if (package == null)
            {
                return HttpNotFound();
            }
            var packageresource = Mapper.Map<Package, PackageResource>(package);
            return View(packageresource);
        }

        // POST: Admin/Packages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Package package = await _packageService.GetByIdAsync(Convert.ToInt32(id));
         await _packageService.DeleteAsync(package);
            _packageService.UnitOfWorkSaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _packageService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
