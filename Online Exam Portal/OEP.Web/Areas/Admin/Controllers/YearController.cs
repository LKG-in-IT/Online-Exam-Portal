using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using OEP.Core.DomainModels.EducationModels;
using OEP.Core.Services;
using OEP.Data;
using OEP.Resources.Admin;

namespace OEP.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class YearController : Controller
    {
        private readonly IYearService _yearService;

        public YearController(IYearService yearService)
        {
            _yearService = yearService;
        }

        // GET: Admin/Year
        public async Task<ActionResult> Index()
        {
            var year = await _yearService.GetAllAsync();
            var yearResourceList = Mapper.Map<List<YearDetails>, List<YearResource>>(year);
            return View(yearResourceList);
        }

        // GET: Admin/Year/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var year = await _yearService.GetByIdAsync(Convert.ToInt32(id));
            var yearResource= Mapper.Map<YearDetails, YearResource>(year);
            if (yearResource == null)
            {
                return HttpNotFound();
            }
            return View(yearResource);
        }

        // GET: Admin/Year/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Year/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(YearResource yearResource)
        {
            if (ModelState.IsValid)
            {
                var yearDetails = Mapper.Map<YearResource, YearDetails>(yearResource);
                yearDetails.CreatedDate = DateTime.Now;
                yearDetails.UpdatedDate = DateTime.Now;
                var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                yearDetails.UserId = userId;

                await _yearService.AddAsync(yearDetails);
                _yearService.UnitOfWorkSaveChanges();
                return RedirectToAction("Index");
            }

            return View(yearResource);
        }

        // GET: Admin/Year/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var year = await _yearService.GetByIdAsync(Convert.ToInt32(id));
            var yearResource = Mapper.Map<YearDetails, YearResource>(year);
            if (yearResource == null)
            {
                return HttpNotFound();
            }
            return View(yearResource);
        }

        // POST: Admin/Year/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(YearResource yearResource)
        {
            if (ModelState.IsValid)
            {
                var existingYear = await _yearService.GetByIdAsync(yearResource.Id);
                existingYear.Year = yearResource.Year;
                existingYear.Status = yearResource.Status;
                existingYear.UpdatedDate = DateTime.Now;
                var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                existingYear.UserId = userId;
                await _yearService.UpdateAsync(existingYear);
                _yearService.UnitOfWorkSaveChanges();
                return RedirectToAction("Index");
            }
            return View(yearResource);
        }

        // GET: Admin/Year/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var year = await _yearService.GetByIdAsync(Convert.ToInt32(id));
            var yearResource = Mapper.Map<YearDetails, YearResource>(year);
            if (yearResource == null)
            {
                return HttpNotFound();
            }
            return View(yearResource);
        }

        // POST: Admin/Year/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var year = await _yearService.GetByIdAsync(Convert.ToInt32(id));
            await _yearService.DeleteAsync(year);
            _yearService.UnitOfWorkSaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _yearService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
