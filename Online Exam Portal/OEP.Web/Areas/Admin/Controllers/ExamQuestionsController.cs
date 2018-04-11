using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;
using OEP.Core.DomainModels.ExamModels;
using OEP.Data;
using OEP.Core.Services;
using OEP.Resources.Admin;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using OEP.Core.Data;
using OEP.Core.DomainModels.QuestionModel;
using OEP.Web.Helpers;

namespace OEP.Web.Areas.Admin.Controllers
{
    [AuthorizeUser(Roles = "Admin,Faculty")]
    public class ExamQuestionsController : Controller
    {
        private readonly IExamQuestionService _examQuestionService;
        private readonly IQuestionService _questionService;
        private readonly IExamservice _examservice;

        public ExamQuestionsController(IExamQuestionService examQuestionService, IQuestionService questionService, IExamservice examservice)
        {
            _examQuestionService = examQuestionService;
            _questionService = questionService;
            _examservice = examservice;
        }


        /// <summary>
        /// Add Questions for Exam. We are adding questions for each exam
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Admin/ExamQuestions/AddQuestions/5
        public async Task<ActionResult> AddQuestions(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var exam = await _examservice.GetByIdAsync(Convert.ToInt32(id));
            if (exam == null)
            {
                return RedirectToRoute(new { controller = "Exams", action = "Index", area = "Admin" });
            }
            ExamQuestionViewModel examQuestionViewModel = new ExamQuestionViewModel();
            examQuestionViewModel.ExamId = exam.Id;
            examQuestionViewModel.ExamName = exam.Name;
            return View(examQuestionViewModel);
        }

        // POST: Admin/ExamQuestions/LoadQuestions/5

        public async Task<ActionResult> LoadQuestions(int id)
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

                    var examQuestion = await _examQuestionService.GetAllAsync(
                        skip,
                        pageSize,

                        //sorting
                        x => sortColumn == "Question" ? x.Questions.Question : null,

                        //filtering
                        x => x.ExamId == id &&
                        searchValue != "" ? x.Questions.Question.Contains(searchValue) : x.Id != 0,

                        //sort by
                        (sortColumnDir == "desc" ? OrderBy.Descending : OrderBy.Ascending),

                        //include properties
                        x => x.Questions
                     );

                    var resp = Mapper.Map<List<ExamQuestion>, List<ExamQuestionResource>>(examQuestion);

                    //total number of rows count     
                    recordsTotal = examQuestion.TotalCount;
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = resp });
                }
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
            return Content("Error");
        }


        /// <summary>
        /// get questions for auto complete textbox.
        /// </summary>
        /// <param name="phrase">text for search</param>
        /// <returns></returns>
        [HttpGet]
        // GET: Admin/ExamQuestions/GetQuestions
        public async Task<ActionResult> GetQuestions(string phrase)
        {
            var questions = await _questionService.FindByAsync(x => x.Question.Contains(phrase) && x.Status);
            var questionRes = Mapper.Map<List<Questions>, List<QuestionAutoCompleteResource>>(questions);
            if (questionRes.Any())
            {
                return Json(questionRes, JsonRequestBehavior.AllowGet);
            }
            var questionAutoCompleteResourceList = new List<QuestionAutoCompleteResource>();
            QuestionAutoCompleteResource questionAutoCompleteResource = new QuestionAutoCompleteResource() { Id = 0, Question = "No Results Found" };
            questionAutoCompleteResourceList.Add(questionAutoCompleteResource);
            return Json(questionAutoCompleteResourceList, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// add question after user choose question from autocomplete and click add button
        /// </summary>
        /// <param name="examQuestionResource"></param>
        /// <returns></returns>
        // POST: Admin/ExamQuestions/AddQuestions
        [HttpPost]
        public async Task<string> AddQuestions(ExamQuestionResource examQuestionResource)
        {
            if (ModelState.IsValid)
            {
                var examQuestionExist =
                    _examQuestionService.FindByAsync(
                        x => x.ExamId == examQuestionResource.ExamId && x.QuestionId == examQuestionResource.QuestionId);

                if (examQuestionExist.Result == null || !examQuestionExist.Result.Any())
                {
                    var examQuestion = Mapper.Map<ExamQuestionResource, ExamQuestion>(examQuestionResource);
                    examQuestion.CreatedDate = DateTime.Now;
                    examQuestion.UpdatedDate = DateTime.Now;
                    var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                    examQuestion.UserId = userId;
                    examQuestion.Status = true;
                    await _examQuestionService.AddAsync(examQuestion);
                    _examQuestionService.UnitOfWorkSaveChanges();

                    var response = JsonConvert.SerializeObject(new ResponseContent<string>() { Status = "Success", Message = "", Result = "" });
                    return response;
                }
                else
                {
                    return JsonConvert.SerializeObject(new ResponseContent<string>() { Status = "Exist", Message = "The Question is already added!", Result = "" });
                }
            }
            return JsonConvert.SerializeObject(new ResponseContent<string>() { Status = "Error", Message = "The enter valid details!", Result = "" });
        }


        /// <summary>
        /// Delete Question from list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizeUser(Roles = "Admin")]
        [HttpPost]
        public async Task<string> DeleteQuestion(int? id)
        {
            if (id != null)
            {
                var examQuestionExist = await _examQuestionService.GetByIdAsync(Convert.ToInt32(id));
                if (examQuestionExist != null)
                {
                    await _examQuestionService.DeleteAsync(examQuestionExist);
                    return JsonConvert.SerializeObject(new ResponseContent<string>() { Status = "Success", Message = "", Result = "" });
                }
                return JsonConvert.SerializeObject(new ResponseContent<string>() { Status = "NotExist", Message = "The Item doesn't exist!", Result = "" });
            }
            return JsonConvert.SerializeObject(new ResponseContent<string>() { Status = "Error", Message = "The enter valid details!", Result = "" });
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
