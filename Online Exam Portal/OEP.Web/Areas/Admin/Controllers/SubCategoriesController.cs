using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OEP.Core.DomainModels.SubCategoryModel;
using OEP.Data;
using OEP.Core.Services;
using AutoMapper;
using OEP.Resources.Admin;
using Microsoft.AspNet.Identity;

namespace OEP.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SubCategoriesController : Controller
    {
        private readonly ISubCategoryService _subcategoryService;
        private readonly ICategoryService _CategoryService;
        public SubCategoriesController(ISubCategoryService subcategoryService, ICategoryService CategoryService)
        {
            _subcategoryService = subcategoryService;
            _CategoryService = CategoryService;
        }

        // GET: Admin/SubCategories
        public async Task<ActionResult> Index()
        {
            var subCategories = await _subcategoryService.GetAllAsync();
            var subCategorieslist = Mapper.Map<List<SubCategory>, List<SubCategoryResource>>(subCategories);

            return View(subCategorieslist);
        }

        // GET: Admin/SubCategories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subCategory = await _subcategoryService.GetByIdAsync(Convert.ToInt32(id));
            if (subCategory == null)
            {
                return HttpNotFound();
            }
            var subcategoryDetails = Mapper.Map<SubCategory, SubCategoryResource>(subCategory);
            return View(subcategoryDetails);
        }

        // GET: Admin/SubCategories/Create
        public ActionResult Create()
        {
            var categorylist = _CategoryService.GetAllAsync();
            ViewBag.CategoryId = new SelectList(categorylist.Result, "Id", "Name");
            return View();
        }

        // POST: Admin/SubCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SubCategoryResource subCategoryresource)
        {
           

            if (ModelState.IsValid)
            {
                var subcategory = Mapper.Map<SubCategoryResource, SubCategory>(subCategoryresource);
                subcategory.CreatedDate = DateTime.Now;
                subcategory.UpdatedDate = DateTime.Now;
                var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                subcategory.UserId = userId;
                await _subcategoryService.AddAsync(subcategory);
                _subcategoryService.UnitOfWorkSaveChanges();
                return RedirectToAction("Index");
            }

            var categorylist = _CategoryService.GetAllAsync();
            ViewBag.CategoryId = new SelectList(categorylist.Result, "Id", "Name", subCategoryresource.CategoryID);
            return View(subCategoryresource);
        }

        // GET: Admin/SubCategories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subCategory = await _subcategoryService.GetByIdAsync(Convert.ToInt32(id));

            if (subCategory == null)
            {
                return HttpNotFound();
            }
            var SubcategoryResource = Mapper.Map<SubCategory, SubCategoryResource>(subCategory);
            ViewBag.CategoryId = new SelectList(_CategoryService.GetAllAsync().Result, "Id", "Name",subCategory.CategoryId);
            return View(SubcategoryResource);
        }

        // POST: Admin/SubCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( SubCategoryResource SubCategoryResource)
        {
                var exstsubcategory = await _subcategoryService.GetByIdAsync(Convert.ToInt32(SubCategoryResource.Id));

            if (ModelState.IsValid)
            {

                exstsubcategory.CategoryId = SubCategoryResource.CategoryID;
                exstsubcategory.Name = SubCategoryResource.Name;
                exstsubcategory.Status = SubCategoryResource.Status;
                exstsubcategory.UpdatedDate = DateTime.Now;
                var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                exstsubcategory.UserId = userId;
                await _subcategoryService.UpdateAsync(exstsubcategory);
                _subcategoryService.UnitOfWorkSaveChanges();
                return RedirectToAction("Index");
            }
       
            ViewBag.CategoryId = new SelectList(_CategoryService.GetAllAsync().Result, "Id", "Name", exstsubcategory.CategoryId);
            return View(exstsubcategory);
        }

        // GET: Admin/SubCategories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subCategory = await _subcategoryService.GetByIdAsync(Convert.ToInt32(id));
            if (subCategory == null)
            {
                return HttpNotFound();
            }
            var SubcategoryResource = Mapper.Map<SubCategory, SubCategoryResource>(subCategory);

            return View(SubcategoryResource);
        }

        // POST: Admin/SubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SubCategory subCategory = await _subcategoryService.GetByIdAsync(Convert.ToInt32(id));
          await  _subcategoryService.DeleteAsync(subCategory);
            _subcategoryService.UnitOfWorkSaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _subcategoryService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
