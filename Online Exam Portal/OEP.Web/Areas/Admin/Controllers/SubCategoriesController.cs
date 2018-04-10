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
using OEP.Core.Data;
using OEP.Web.Helpers;

namespace OEP.Web.Areas.Admin.Controllers
{
    [AuthorizeUser(Roles = "Admin")]
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
        public ActionResult Index()
        {
          

            return View();
        }



        // POST: Admin/subCategories/LoadCategories

        public async Task<ActionResult> LoadSubCategories()
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


                  
                    var subcategoryList = await _subcategoryService.GetAllAsync(
                        skip,
                        pageSize,

                        //sorting


                        x => sortColumn == "Name" ? x.Name : (sortColumn== "CategoryName"?x.Category.Name:null),

                        //filtering
                        x => searchValue != "" ? x.Name.Contains(searchValue) : x.Id != 0,

                        //sort by
                        (sortColumnDir == "desc" ? OrderBy.Descending : OrderBy.Ascending),

                          // Include Category
                          x => x.Category
                     );


                  
                    var resp = Mapper.Map<List<SubCategory>, List<SubCategoryResource>>(subcategoryList);

                    //total number of rows count     
                    recordsTotal = subcategoryList.TotalCount;
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = resp });
                }
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
            return Content("Error");
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
            ViewBag.CategoryId = new SelectList(categorylist.Result.Where(i => i.Status == true), "Id", "Name");
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
            ViewBag.CategoryId = new SelectList(categorylist.Result.Where(i => i.Status == true), "Id", "Name", subCategoryresource.CategoryID);
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
            var categorylist = _CategoryService.GetAllAsync();
            ViewBag.CategoryId = new SelectList(categorylist.Result.Where(i => i.Status == true), "Id", "Name", subCategory.CategoryId);
            return View(SubcategoryResource);
        }

        // POST: Admin/SubCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(SubCategoryResource SubCategoryResource)
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
            var categorylist = _CategoryService.GetAllAsync();


            ViewBag.CategoryId = new SelectList(categorylist.Result.Where(i => i.Status == true), "Id", "Name", exstsubcategory.CategoryId);
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
            await _subcategoryService.DeleteAsync(subCategory);
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
