using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OEP.Core.DomainModels.QuestionModel;
using OEP.Data;
using OEP.Core.Services;
using AutoMapper;
using OEP.Resources.Admin;
using Microsoft.AspNet.Identity;
using OEP.Core.Data;

namespace OEP.Web.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    public class QuestionsController : Controller
    {
        private readonly IQuestionService _questionService;
        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        // GET: Admin/Questions

        public ActionResult Index()
        {
            return View();
        }
        // POST: Admin/Questions/LoadQuestions
        public async Task<ActionResult> LoadQuestions()
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

                    var questionList = await _questionService.GetAllAsync(
                        skip,
                        pageSize,

                        //sorting
                        x => sortColumn == "Name" ? x.Question : null,

                        //filtering
                        x => searchValue != "" ? x.Question.Contains(searchValue) : x.Id != 0,

                        //sort by
                        (sortColumnDir == "desc" ? OrderBy.Descending : OrderBy.Ascending)
                     );

                    var resp = Mapper.Map<List<Questions>, List<QuestionsResource>>(questionList);

                    //total number of rows count     
                    recordsTotal = questionList.TotalCount;
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = resp });
                }
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
            return Content("Error");
        }


        // GET: Admin/Questions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Questions questions = await _questionService.GetByIdAsync(Convert.ToInt32(id));
            if (questions == null)
            {
                return HttpNotFound();
            }
            var questionsResource = Mapper.Map<Questions, QuestionsResource>(questions);
            return View(questionsResource);
        }

        // GET: Admin/Questions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(QuestionsResource questionsResource)
        {
            if (ModelState.IsValid)
            {
                var questions = Mapper.Map<QuestionsResource, Questions>(questionsResource);
                questions.CreatedDate = DateTime.Now;
                questions.UpdatedDate = DateTime.Now;
                var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                questions.UserId = userId;
                await _questionService.AddAsync(questions);
                _questionService.UnitOfWorkSaveChanges();

                return RedirectToAction("Index");
            }

            return View(questionsResource);
        }

        // GET: Admin/Questions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Questions questions = await _questionService.GetByIdAsync(Convert.ToInt32(id));
            if (questions == null)
            {
                return HttpNotFound();
            }
            var questionsResource = Mapper.Map<Questions, QuestionsResource>(questions);
            return View(questionsResource);
        }

        // POST: Admin/Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(QuestionsResource questionsResource)
        {
            if (ModelState.IsValid)
            {
                var exstQuestions = await _questionService.GetByIdAsync(questionsResource.Id);
                exstQuestions.Question = questionsResource.Question;
                exstQuestions.OptionA = questionsResource.OptionA;
                exstQuestions.OptionB = questionsResource.OptionB;
                exstQuestions.OptionC = questionsResource.OptionC;
                exstQuestions.OptionD = questionsResource.OptionD;
                exstQuestions.Answer = questionsResource.Answer;
                exstQuestions.UpdatedDate = DateTime.Now;

                await _questionService.UpdateAsync(exstQuestions);
                _questionService.UnitOfWorkSaveChanges();
              
                return RedirectToAction("Index");
            }
            return View(questionsResource);
        }

        // GET: Admin/Questions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Questions questions = await _questionService.GetByIdAsync(Convert.ToInt32(id));
            if (questions == null)
            {
                return HttpNotFound();
            }
            var questionsResource = Mapper.Map<Questions, QuestionsResource>(questions);
            return View(questionsResource);
        }

        // POST: Admin/Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Questions questions = await _questionService.GetByIdAsync(id);
            await _questionService.DeleteAsync(questions);
            _questionService.UnitOfWorkSaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _questionService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
