using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OEP.Data;
using OEP.Resources.Admin;
using OEP.Core.Services;
using AutoMapper;
using OEP.Core.DomainModels.StudyMaterialsModel;
using Microsoft.AspNet.Identity;
using System.IO;
using OEP.Core.Data;

namespace OEP.Web.Areas.Admin.Controllers
{
    public class StudyMaterialsController : Controller
    {
        private readonly IStudyMaterial _studyMaterial;
        private readonly ISubCategoryService _subCategoryService;

        public StudyMaterialsController(IStudyMaterial studyMaterial,ISubCategoryService subCategoryService)
        {
            _studyMaterial = studyMaterial;
            _subCategoryService = subCategoryService;
        }

        // GET: Admin/StudyMaterialsResources
        public async Task<ActionResult> Index()
        {
           
            return View();
        }


        public async Task<ActionResult> LoadstudyMaterial()
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

                    var materialsList = await _studyMaterial.GetAllAsync(
                        skip,
                        pageSize,

                          //sorting
                           x => sortColumn == "Name" ? x.Name : null,
                
                        //filtering
                        x => searchValue != "" ? x.Name.Contains(searchValue) : x.Id != 0,

                        //sort by
                        (sortColumnDir == "desc" ? OrderBy.Descending : OrderBy.Ascending),

                        //Include ExamType
                      
                        x => x.SubCategory
                     );

                    var resp = Mapper.Map<List<StudyMaterial>, List<StudyMaterialsResources>>(materialsList);

                    //total number of rows count     
                    recordsTotal = materialsList.TotalCount;
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = resp });
                }
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
            return Content("Error");
        }


        // GET: Admin/StudyMaterialsResources/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudyMaterial studyMaterials = await _studyMaterial.GetByIdAsync(Convert.ToInt32(id));
            if (studyMaterials == null)
            {
                return HttpNotFound();
            }
            var studyMaterialsResources = Mapper.Map<StudyMaterial, StudyMaterialsResources>(studyMaterials);
            return View(studyMaterialsResources);
        }

        // GET: Admin/StudyMaterialsResources/Create
        public ActionResult Create()
        {

            var Subcategorylist = _subCategoryService.GetAllAsync();
            ViewBag.SubcategoryID = new SelectList(Subcategorylist.Result.Where(i => i.Status == true), "Id", "Name");
            return View();
        }

        // POST: Admin/StudyMaterialsResources/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( StudyMaterialsResources studyMaterialsResources, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var Materials = Mapper.Map<StudyMaterialsResources, StudyMaterial>(studyMaterialsResources);
                Materials.Name = studyMaterialsResources.Name;
                Materials.CreatedDate = DateTime.Now;
                Materials.UpdatedDate = DateTime.Now;
                var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                Materials.Status = studyMaterialsResources.Status;
                Materials.UserId = userId;
                await _studyMaterial.AddAsync(Materials);

                var id = Materials.Id;
                if(file!=null&&file.ContentLength>0)
                {
                  
                    var fileName =Materials.Name+"-"+ id+ Path.GetExtension(file.FileName);
                   
                        var path = Path.Combine(Server.MapPath("~/Uploads/StudyMaterialFiles"), fileName);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                        file.SaveAs(path);
                       
                        Materials.FilePath = Url.Content(Path.Combine("~/Uploads/StudyMaterialFiles", fileName));
                   
                }
                await _studyMaterial.UpdateAsync(Materials);
                _studyMaterial.UnitOfWorkSaveChanges();

                return RedirectToAction("Index");
            }

            return View(studyMaterialsResources);
        }

        // GET: Admin/StudyMaterialsResources/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudyMaterial studyMaterial = await _studyMaterial.GetByIdAsync(Convert.ToInt32(id));
            if (studyMaterial == null)
            {
                return HttpNotFound();
            }
            var studyMaterialsResources = Mapper.Map<StudyMaterial, StudyMaterialsResources>(studyMaterial);
            var Subcategorylist = _subCategoryService.GetAllAsync();
            ViewBag.SubcategoryID = new SelectList(Subcategorylist.Result.Where(i => i.Status == true), "Id", "Name");
            return View(studyMaterialsResources);
        }

        // POST: Admin/StudyMaterialsResources/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( StudyMaterialsResources studyMaterialsResources,HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var exststudyMaterial = await _studyMaterial.GetByIdAsync(Convert.ToInt32(studyMaterialsResources.Id));
                exststudyMaterial.Name = studyMaterialsResources.Name;
                exststudyMaterial.SubcategoryID = studyMaterialsResources.SubcategoryID;
                exststudyMaterial.Status= studyMaterialsResources.Status;
                var id = exststudyMaterial.Id;
                if (file != null && file.ContentLength > 0)
                {

                    var fileName = exststudyMaterial.Name + "-" + id + Path.GetExtension(file.FileName);

                    var path = Path.Combine(Server.MapPath("~/Uploads/StudyMaterialFiles"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    file.SaveAs(path);

                    exststudyMaterial.FilePath = Url.Content(Path.Combine("~/Uploads/StudyMaterialFiles", fileName));

                }


                await _studyMaterial.UpdateAsync(exststudyMaterial);

                return RedirectToAction("Index");
            }
            return View(studyMaterialsResources);
        }

        // GET: Admin/StudyMaterialsResources/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudyMaterial studyMaterial = await _studyMaterial.GetByIdAsync(Convert.ToInt32(id));

            if (studyMaterial == null)
            {
                return HttpNotFound();
            }
            var studyMaterialsResources = Mapper.Map<StudyMaterial, StudyMaterialsResources>(studyMaterial);

            return View(studyMaterialsResources);
        }

        // POST: Admin/StudyMaterialsResources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            StudyMaterial studyMaterial = await _studyMaterial.GetByIdAsync(Convert.ToInt32(id));
            var path = Server.MapPath(studyMaterial.FilePath);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            await _studyMaterial.DeleteAsync(studyMaterial);
            _studyMaterial.UnitOfWorkSaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _studyMaterial.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
