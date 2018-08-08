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
using OEP.Web.Helpers;
using OEP.Core.DomainModels;

namespace OEP.Web.Areas.Admin.Controllers
{
    [AuthorizeUser(Roles = "Admin,Faculty")]
    public class QuestionsController : Controller
    {
        private readonly IQuestionService _questionService;
        private readonly IService<QuestionType> _questionTypeService;

        private readonly IService<QuestionsLocalized> _questionsLocalizedService;

        private readonly IService<Language> _languageService;

        public QuestionsController(IQuestionService questionService, IService<QuestionType> questionTypeService,
            IService<QuestionsLocalized> questionsLocalizedService, IService<Language> languageService)
        {
            _questionService = questionService;
            _questionTypeService = questionTypeService;

            _questionsLocalizedService=questionsLocalizedService;
            _languageService = languageService;

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
                        x => sortColumn == "Question" ? x.Question : (sortColumn == "QuestionType" ? x.QuestionType.Name : null),

                        //filtering
                        x => searchValue != "" ? x.Question.Contains(searchValue) || x.OptionA.Contains(searchValue) || x.OptionB.Contains(searchValue) || x.OptionC.Contains(searchValue) || x.OptionD.Contains(searchValue) : x.Id != 0,

                        //sort by
                        (sortColumnDir == "desc" ? OrderBy.Descending : OrderBy.Ascending),

                        x=>x.QuestionType
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
            Questions questions = await _questionService.GetSingleIncludingAsync(Convert.ToInt32(id), x => x.QuestionType);
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
            var questionTypeList = _questionTypeService.FindByAsync(x => x.Status);
            ViewBag.QuestionTypeId = new SelectList(questionTypeList.Result.Where(i => i.Status == true), "Id", "Name");
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

                //Get Current available languages 
                var languages =await _languageService.FindByAsync(x => x.Status && !x.Default);
                if (languages != null)
                {
                    // inserting default values to all langauges
                    foreach(var lang in languages)
                    {
                        var QuestionsLocalized = new QuestionsLocalized() {
                            Question= questions.Question,
                            OptionA = questions.OptionA,
                            OptionB = questions.OptionB,
                            OptionC = questions.OptionC,
                            OptionD = questions.OptionD,
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            LanguageId=lang.Id,
                            QuestionsId=questions.Id,
                            UserId =User.Identity.GetUserId()
                        };
                        await _questionsLocalizedService.AddAsync(QuestionsLocalized);
                        _questionsLocalizedService.UnitOfWorkSaveChanges();
                    }
                }
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
            Questions questions = await _questionService.GetSingleIncludingAsync(Convert.ToInt32(id),x=>x.QuestionType , x=>x.QuestionsLocalized);
            if (questions == null)
            {
                return HttpNotFound();
            }
            var questionsResource = Mapper.Map<Questions, QuestionsResource>(questions);

            var questionTypeList = _questionTypeService.FindByAsync(x=>x.Status);
            ViewBag.QuestionTypeId = new SelectList(questionTypeList.Result.Where(i => i.Status == true), "Id", "Name", questions.QuestionTypeId);

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
                exstQuestions.QuestionTypeId = questionsResource.QuestionTypeId;
                exstQuestions.UpdatedDate = DateTime.Now;

                await _questionService.UpdateAsync(exstQuestions);
                _questionService.UnitOfWorkSaveChanges();

                //Update Localized questions
                if (questionsResource.QuestionsLocalized != null)
                {
                    foreach (var item in questionsResource.QuestionsLocalized)
                    {
                        var questionsLocalized = await _questionsLocalizedService.GetByIdAsync(item.Id);
                        questionsLocalized.Question = item.Question;
                        questionsLocalized.OptionA = questionsResource.OptionA;
                        questionsLocalized.OptionB = questionsResource.OptionB;
                        questionsLocalized.OptionC = questionsResource.OptionC;
                        questionsLocalized.OptionD = questionsResource.OptionD;
                        questionsLocalized.UserId = User.Identity.GetUserId();                        
                        questionsLocalized.UpdatedDate = DateTime.Now;

                        await _questionsLocalizedService.UpdateAsync(questionsLocalized);
                        _questionsLocalizedService.UnitOfWorkSaveChanges();
                    }
                }

              
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
            Questions questions = await _questionService.GetSingleIncludingAsync(Convert.ToInt32(id), x => x.QuestionType);
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
