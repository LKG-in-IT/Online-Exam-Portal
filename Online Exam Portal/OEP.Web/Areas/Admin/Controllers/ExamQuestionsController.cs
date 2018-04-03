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
using OEP.Resources.Admin;
using AutoMapper;
using Microsoft.AspNet.Identity;

namespace OEP.Web.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ExamQuestionsController : Controller
    {
        private readonly IExamQuestionService _examQuestionService;
        private readonly IQuestionService _questionService;
        private readonly IExamservice _examservice;

        public ExamQuestionsController(IExamQuestionService examQuestionService, IQuestionService questionService, IExamservice examservice)
        {

            _examQuestionService = examQuestionService;
            _questionService = questionService;
            _examservice=examservice;
        }

        // GET: Admin/ExamQuestions
        public async Task<ActionResult> Index()
        {
            var examQuestions = await _examQuestionService.GetAllAsync();
            var examQuestionsResource = Mapper.Map<List< ExamQuestion>,List< ExamQuestionResource>>(examQuestions);
            return View(examQuestionsResource);
        }

        // GET: Admin/ExamQuestions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamQuestion examQuestion =await _examQuestionService.GetByIdAsync(Convert.ToInt32(id));
            if (examQuestion == null)
            {
                return HttpNotFound();
            }
            var examQuestionsResource = Mapper.Map<ExamQuestion, ExamQuestionResource>(examQuestion);

            return View(examQuestionsResource);
        }

        // GET: Admin/ExamQuestions/Create
        public async Task< ActionResult >Create()
        {
            ViewBag.ExamId =new SelectList( await _examservice.GetAllAsync(), "Id", "Name");
            ViewBag.QuestionId = new SelectList(await _questionService.GetAllAsync(), "Id", "Question");
            return View();
        }

        // POST: Admin/ExamQuestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( ExamQuestionResource examQuestionResource)
        {
            if (ModelState.IsValid)
            {
                var examQuestion = Mapper.Map<ExamQuestionResource, ExamQuestion>(examQuestionResource);
                examQuestion.CreatedDate = DateTime.Now;
                examQuestion.UpdatedDate = DateTime.Now;
                var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                examQuestion.UserId = userId;
                await _examQuestionService.AddAsync(examQuestion);
                _examQuestionService.UnitOfWorkSaveChanges();


             
                return RedirectToAction("Index");
            }

            ViewBag.ExamId = new SelectList (await _examservice.GetAllAsync(), "Id", "Name", examQuestionResource.ExamId);
            ViewBag.QuestionId = new SelectList(await _questionService.GetAllAsync(), "Id", "Question", examQuestionResource.QuestionId);
            return View(examQuestionResource);
        }

        // GET: Admin/ExamQuestions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamQuestion examQuestion = await _examQuestionService.GetByIdAsync(Convert.ToInt32(id));
            if (examQuestion == null)
            {
                return HttpNotFound();
            }
            var examQuestionResource = Mapper.Map<ExamQuestion, ExamQuestionResource>(examQuestion);
            ViewBag.ExamId = new SelectList(await _examservice.GetAllAsync(), "Id", "Name", examQuestionResource.ExamId);
            ViewBag.QuestionId = new SelectList(await _questionService.GetAllAsync(), "Id", "Question", examQuestionResource.QuestionId);
            return View(examQuestionResource);
        }

        // POST: Admin/ExamQuestions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( ExamQuestionResource examQuestionResource)
        {
            if (ModelState.IsValid)
            {
                var exstexamQuestion = await _examQuestionService.GetByIdAsync(examQuestionResource.Id);
                exstexamQuestion.QuestionId = examQuestionResource.QuestionId;
                exstexamQuestion.ExamId = examQuestionResource.ExamId;
                var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                exstexamQuestion.UpdatedDate = DateTime.Now;
                await _examQuestionService.UpdateAsync(exstexamQuestion);
                _examQuestionService.UnitOfWorkSaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ExamId = new SelectList(await _examservice.GetAllAsync(), "Id", "Name", examQuestionResource.ExamId);
            ViewBag.QuestionId = new SelectList(await _questionService.GetAllAsync(), "Id", "Question", examQuestionResource.QuestionId);
            return View(examQuestionResource);
        }

        // GET: Admin/ExamQuestions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamQuestion examQuestion = await _examQuestionService.GetByIdAsync(Convert.ToInt32(id));
            if (examQuestion == null)
            {
                return HttpNotFound();
            }
            var examQuestionResource = Mapper.Map<ExamQuestion, ExamQuestionResource>(examQuestion);
            ViewBag.ExamId = new SelectList(await _examservice.GetAllAsync(), "Id", "Name", examQuestionResource.ExamId);
            ViewBag.QuestionId = new SelectList(await _questionService.GetAllAsync(), "Id", "Question", examQuestionResource.QuestionId);
            return View(examQuestionResource);
        }

        // POST: Admin/ExamQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ExamQuestion examQuestion = await _examQuestionService.GetByIdAsync(id);
            await _examQuestionService.DeleteAsync(examQuestion);
            _examQuestionService.UnitOfWorkSaveChanges();
           
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _examQuestionService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
