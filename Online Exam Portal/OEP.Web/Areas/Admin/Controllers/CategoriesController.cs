using System;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using OEP.Core.DomainModels.CategoryModel;
using OEP.Core.Services;
using OEP.Resources.Admin;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using OEP.Core.Data;
using OEP.Core.DomainModels.ExamModels;

namespace OEP.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
       
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        // GET: Admin/Categories
        public ActionResult Index()
        {
            return View();
        }

        // POST: Admin/Categories/LoadCategories

        public async Task<ActionResult> LoadCategories()
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

                    var categoryList = await _categoryService.GetAllAsync(
                        skip,
                        pageSize,

                        //sorting
                        x => sortColumn == "Name" ? x.Name : null,

                        //filtering
                        x=> searchValue != "" ? x.Name.Contains(searchValue) : x.Id != 0,

                        //sort by
                        (sortColumnDir == "desc" ? OrderBy.Descending : OrderBy.Ascending)
                     );

                    var resp = Mapper.Map<List<Category>, List<CategoryResource>>(categoryList);

                    //total number of rows count     
                    recordsTotal = categoryList.TotalCount;
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = resp });
                }
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
            return Content("Error");
        }

        // GET: Admin/Categories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await _categoryService.GetByIdAsync(Convert.ToInt32(id));
            if (category == null)
            {
                return HttpNotFound();
            }
            var categoryResource = Mapper.Map<Category, CategoryResource>(category);
            return View(categoryResource);
        }

        // GET: Admin/Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CategoryResource categoryResource)
        {
            if (ModelState.IsValid)
            {
                var category=Mapper.Map<CategoryResource, Category>(categoryResource);
                category.CreatedDate=DateTime.Now;
                category.UpdatedDate=DateTime.Now;
                var userId= System.Web.HttpContext.Current.User.Identity.GetUserId();
                category.UserId = userId;
                await _categoryService.AddAsync(category);
                _categoryService.UnitOfWorkSaveChanges();
                return RedirectToAction("Index");
            }

            return View(categoryResource);
        }

        // GET: Admin/Categories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await _categoryService.GetByIdAsync(Convert.ToInt32(id));
            if (category == null)
            {
                return HttpNotFound();
            }
            var categoryResource = Mapper.Map<Category, CategoryResource>(category);
            return View(categoryResource);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CategoryResource categoryResource)
        {
            if (ModelState.IsValid)
            {
                var exstcategory = await _categoryService.GetByIdAsync(Convert.ToInt32(categoryResource.Id));
                exstcategory.Name = categoryResource.Name;
                exstcategory.Status = categoryResource.Status;
                exstcategory.UpdatedDate = DateTime.Now;
                var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                exstcategory.UserId = userId;
                await _categoryService.UpdateAsync(exstcategory);
                _categoryService.UnitOfWorkSaveChanges();                
                return RedirectToAction("Index");
            }
            return View(categoryResource);
        }

        // GET: Admin/Categories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await _categoryService.GetByIdAsync(Convert.ToInt32(id));
            if (category == null)
            {
                return HttpNotFound();
            }
            var categoryResource = Mapper.Map<Category, CategoryResource>(category);

            return View(categoryResource);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Category category = await _categoryService.GetByIdAsync(Convert.ToInt32(id));
            await _categoryService.DeleteAsync(category);
            _categoryService.UnitOfWorkSaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // db.Dispose();
                _categoryService.Dispose();
            }
            base.Dispose(disposing);
        }
        
    }
}
