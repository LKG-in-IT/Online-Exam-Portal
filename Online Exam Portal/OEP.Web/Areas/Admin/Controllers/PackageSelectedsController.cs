using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OEP.Core.DomainModels.PackageSelectedModels;
using OEP.Data;
using OEP.Core.Services;
using AutoMapper;
using OEP.Resources.Admin;
using Microsoft.AspNet.Identity;

namespace OEP.Web.Areas.Admin.Controllers
{
    public class PackageSelectedsController : Controller
    {
        private readonly IPackageSelectedService _packageSelectedService;
        private readonly IPackageService _packageService;
        public PackageSelectedsController(IPackageSelectedService packageSelectedService, IPackageService packageService)
        {
            _packageSelectedService = packageSelectedService;
            _packageService = packageService;
        }


        // GET: Admin/PackageSelecteds
        public async Task<ActionResult> Index()
        {
            var packageSelecteds = await _packageSelectedService.GetAllAsync();
            var packageSelectedsresource = Mapper.Map<List<PackageSelected>, List<PackageSelectedResource>>(packageSelecteds);
            return View(packageSelectedsresource);
        }

        // GET: Admin/PackageSelecteds/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PackageSelected packageSelected = await _packageSelectedService.GetByIdAsync(Convert.ToInt32(id));
            if (packageSelected == null)
            {
                return HttpNotFound();
            }
            var packageSelectedsresource = Mapper.Map<PackageSelected, PackageSelectedResource>(packageSelected);
            return View(packageSelectedsresource);
        }

        // GET: Admin/PackageSelecteds/Create
        public async Task<ActionResult> Create()
        {
            var package = await _packageService.GetAllAsync();
            ViewBag.PackageId = new SelectList(package.Where(I => I.Status == true), "Id", "Name");
            return View();
        }

        // POST: Admin/PackageSelecteds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PackageSelectedResource packageSelectedResource)
        {
            if (ModelState.IsValid)
            {
                var Packageselected = Mapper.Map<PackageSelectedResource, PackageSelected>(packageSelectedResource);

                Packageselected.CreatedDate = DateTime.Now;
                Packageselected.UpdatedDate = DateTime.Now;
                Packageselected.Status = true;
                var userid = System.Web.HttpContext.Current.User.Identity.GetUserId();
                Packageselected.UserId = userid;
                await _packageSelectedService.AddAsync(Packageselected);
                _packageService.UnitOfWorkSaveChanges();
                return RedirectToAction("Index");
            }
            var package = await _packageService.GetAllAsync();
            ViewBag.PackageId = new SelectList(package.Where(i => i.Status == true), "Id", "Name", packageSelectedResource.Id);
            return View(packageSelectedResource);
        }

        // GET: Admin/PackageSelecteds/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PackageSelected packageSelected = await _packageSelectedService.GetByIdAsync(Convert.ToInt32(id));
            if (packageSelected == null)
            {
                return HttpNotFound();
            }
            var packageSelectedResource = Mapper.Map<PackageSelected, PackageSelectedResource>(packageSelected);
            var package = await _packageService.GetAllAsync();
            ViewBag.PackageId = new SelectList(package.Where(i => i.Status == true), "Id", "Name", packageSelectedResource.Id);
            return View(packageSelectedResource);
        }

        // POST: Admin/PackageSelecteds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PackageSelectedResource packageSelectedResource)
        {
            if (ModelState.IsValid)
            {
                var exstpackageSelected = await _packageSelectedService.GetByIdAsync(packageSelectedResource.Id);
                exstpackageSelected.PackageId = packageSelectedResource.PackageId;
                exstpackageSelected.UpdatedDate = DateTime.Now;
                exstpackageSelected.Status = packageSelectedResource.Status;
                exstpackageSelected.Datefrom = packageSelectedResource.Datefrom;
                exstpackageSelected.Dateto = packageSelectedResource.Dateto;
                var userid = System.Web.HttpContext.Current.User.Identity.GetUserId();
                exstpackageSelected.UserId = userid;

                await _packageSelectedService.UpdateAsync(exstpackageSelected);
                _packageSelectedService.UnitOfWorkSaveChanges();


                return RedirectToAction("Index");
            }

            var package = await _packageService.GetAllAsync();
            ViewBag.PackageId = new SelectList(package.Where(i => i.Status == true), "Id", "Name", packageSelectedResource.Id);
            return View(packageSelectedResource);
        }

        // GET: Admin/PackageSelecteds/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PackageSelected packageSelected = await _packageSelectedService.GetByIdAsync(Convert.ToInt32( id));
            if (packageSelected == null)
            {
                return HttpNotFound();
            }
            var packageSelectedresource = Mapper.Map<PackageSelected, PackageSelectedResource>(packageSelected);
            return View(packageSelectedresource);
        }

        // POST: Admin/PackageSelecteds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PackageSelected packageSelected = await _packageSelectedService.GetByIdAsync(id);
            await _packageSelectedService.DeleteAsync(packageSelected);
            _packageSelectedService.UnitOfWorkSaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _packageSelectedService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
