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
        public async Task<ActionResult> Index()
        {
            var exams = await _examservice.GetAllAsync();
            var examsList = Mapper.Map<List<Exam>, List<ExamResource>>(exams);
            return View(examsList);
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

            ViewBag.ExamtypeId = new SelectList(examtype, "Id", "Name");
            ViewBag.SubcategoryId = new SelectList(subcategory, "Id", "Name");
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
            ViewBag.ExamtypeId = new SelectList(examtype, "Id", "Name", examresource.Examtypeid);
            ViewBag.SubcategoryId = new SelectList(subcategory, "Id", "Name", examresource.SubcategoryId);
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

            ViewBag.ExamtypeId = new SelectList(examtype, "Id", "Name", examResource.Examtypeid);
            ViewBag.SubcategoryId = new SelectList(subcategory, "Id", "Name", examResource.SubcategoryId);
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
            ViewBag.ExamtypeId = new SelectList(examtype.Result, "Id", "Name", examResource.Examtypeid);
            ViewBag.SubcategoryId = new SelectList(subcategory.Result, "Id", "Name", examResource.SubcategoryId);
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
