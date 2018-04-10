using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OEP.Core.DomainModels.ExamModels;
using OEP.Data;
using OEP.Core.Services;
using AutoMapper;
using OEP.Resources.Admin;
using Microsoft.AspNet.Identity;
using OEP.Core.Data;

namespace OEP.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]

    public class ExamsController : Controller
    {
        private readonly IExamservice _examservice;
        private readonly IExamTypeService _examTypeService;
        private readonly ISubCategoryService _subCategoryService;

        public ExamsController(IExamservice examservice, IExamTypeService examTypeService, ISubCategoryService subCategoryService)
        {
            _examservice = examservice;
            _examTypeService = examTypeService;
            _subCategoryService = subCategoryService;
        }

        // GET: Admin/Exams
        public ActionResult Index()
        {
         
            return View();
        }

        public async Task<ActionResult> LoadExams()
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

                    var educationTypeList = await _examservice.GetAllAsync(
                        skip,
                        pageSize,

                        //sorting
                       // x => sortColumn == "Name" ? x.Name : null,
                          x => sortColumn == "Name" ? x.Name : (sortColumn == "ExamType" ? x.ExamType.Name : (sortColumn== "SubCategory" ? x.SubCategory.Name:(sortColumn== "Passmark"?x.Passmark.ToString():null))),
                        //filtering
                        x => searchValue != "" ? x.Name.Contains(searchValue) : x.Id != 0,

                        //sort by
                        (sortColumnDir == "desc" ? OrderBy.Descending : OrderBy.Ascending),

                        //Include ExamType
                        x => x.ExamType,
                        x=>x.SubCategory
                     );

                    var resp = Mapper.Map<List<Exam>, List<ExamResource>>(educationTypeList);

                    //total number of rows count     
                    recordsTotal = educationTypeList.TotalCount;
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = resp });
                }
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
            return Content("Error");
        }


        // GET: Admin/Exams/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exam exam = await _examservice.GetByIdAsync(Convert.ToInt32(id));
            if (exam == null)
            {
                return HttpNotFound();
            }
            var examResource = Mapper.Map<Exam, ExamResource>(exam);
            return View(examResource);
        }

        // GET: Admin/Exams/Create
        public async Task<ActionResult> Create()
        {
            var examtype =await _examTypeService.GetAllAsync();
            var subcategory = await _subCategoryService.GetAllAsync();

            ViewBag.ExamtypeId = new SelectList(examtype.Where(i=>i.Status==true), "Id", "Name");
            ViewBag.SubcategoryId = new SelectList(subcategory.Where(i => i.Status == true), "Id", "Name");
            return View();
        }

        // POST: Admin/Exams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ExamResource examresource)
        {
            if (ModelState.IsValid)
            {
                var exam = Mapper.Map<ExamResource, Exam>(examresource);
                exam.CreatedDate = DateTime.Now;
                exam.UpdatedDate = DateTime.Now;
                var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                exam.UserId = userId;

                await _examservice.AddAsync(exam);
                _examservice.UnitOfWorkSaveChanges();

                return RedirectToAction("Index");
            }
            var examtype = _examTypeService.GetAll();
            var subcategory = _subCategoryService.GetAll();
            ViewBag.ExamtypeId = new SelectList(examtype.Where(i => i.Status == true), "Id", "Name", examresource.Examtypeid);
            ViewBag.SubcategoryId = new SelectList(subcategory.Where(i => i.Status == true), "Id", "Name", examresource.SubcategoryId);
            return View(examresource);
        }

        // GET: Admin/Exams/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exam exam = await _examservice.GetByIdAsync(Convert.ToInt32(id));
            if (exam == null)
            {
                return HttpNotFound();
            }

            var examResource = Mapper.Map<Exam, ExamResource>(exam);
            var examtype =await _examTypeService.GetAllAsync();
            var subcategory =await  _subCategoryService.GetAllAsync();

            ViewBag.ExamtypeId = new SelectList(examtype.Where(i => i.Status == true), "Id", "Name", examResource.Examtypeid);
            ViewBag.SubcategoryId = new SelectList(subcategory.Where(i => i.Status == true), "Id", "Name", examResource.SubcategoryId);
            return View(examResource);
        }

        // POST: Admin/Exams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ExamResource examResource)
        {
            if (ModelState.IsValid)
            {
                var exstexam = await _examservice.GetByIdAsync(examResource.Id);
                exstexam.UpdatedDate = DateTime.Now;
                exstexam.Name = examResource.Name;
                exstexam.ExamtypeId = examResource.Examtypeid;
                exstexam.SubcategoryId = examResource.SubcategoryId;

                exstexam.Passmark = examResource.Passmark;
                exstexam.Status = examResource.Status;
                var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                exstexam.UserId = userId;

                await _examservice.UpdateAsync(exstexam);
                _examservice.UnitOfWorkSaveChanges();

                return RedirectToAction("Index");
            }

            var examtype =  _examTypeService.GetAllAsync();
            var subcategory = _subCategoryService.GetAllAsync();
            ViewBag.ExamtypeId = new SelectList(examtype.Result.Where(i => i.Status == true), "Id", "Name", examResource.Examtypeid);
            ViewBag.SubcategoryId = new SelectList(subcategory.Result.Where(i => i.Status == true), "Id", "Name", examResource.SubcategoryId);
            return View(examResource);
        }

        // GET: Admin/Exams/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exam exam = await _examservice.GetByIdAsync(Convert.ToInt32(id));
            if (exam == null)
            {
                return HttpNotFound();
            }
            var examResource = Mapper.Map<Exam, ExamResource>(exam);

            return View(examResource);
        }

        // POST: Admin/Exams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Exam exam = await _examservice.GetByIdAsync(id);
            await _examservice.DeleteAsync(exam);
            _examservice.UnitOfWorkSaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _examservice.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
