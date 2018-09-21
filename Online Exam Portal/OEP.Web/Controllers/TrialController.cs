using AutoMapper;
using Microsoft.AspNet.Identity.Owin;
using OEP.Core.Data;
using OEP.Core.DomainModels.QuestionModel;
using OEP.Core.Services;
using OEP.Resources.Admin;
using OEP.Resources.Common;
using OEP.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace OEP.Web.Controllers
{
    [AuthorizeUser(Roles = "User,Faculty,Admin")]
    [ValidatePackageStatus]
    public class TrialController : Controller
    {

        private readonly IService<QuestionType> _questionTypeService;
        private readonly IQuestionService _questionService;

        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;

        public TrialController(IService<QuestionType> questionTypeService, IQuestionService questionService)
        {
            _questionTypeService = questionTypeService;
            _questionService = questionService;
        }

        public TrialController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;

        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Trial
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var questionTypeList = await _questionTypeService.GetAllAsync();
            var resp = Mapper.Map<List<QuestionType>, List<QuestionTypeResource>>(questionTypeList);
            ViewBag.QuestionType = new SelectList(questionTypeList.Where(i => i.Status == true), "Id", "Name", selectedValue: "Id");
            return View();
        }

        [HttpPost]
        public ActionResult Index(int? QuestionType,string NoOfQuestions)
        {
            if(QuestionType==0 || QuestionType == null)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Start", new { QuestionType = QuestionType, NoOfQuestions= NoOfQuestions });
        }

        [HttpGet]
        public async Task<ActionResult> Start(int? QuestionType,string NoOfQuestions)
        {
            if (QuestionType != null && QuestionType != 0)
            {
                if (string.IsNullOrEmpty(NoOfQuestions))
                {
                    NoOfQuestions = "10";
                }
                ViewBag.QuestionType = QuestionType;
                var QuestionTypeId = Convert.ToInt32(QuestionType);
                var QuestionCount = Convert.ToInt32(NoOfQuestions);
                //Get Total no of question in database
                var totalQuestionsCountInDatabase = _questionService.GetAllCount();
                //check the total
                QuestionCount = (QuestionCount > totalQuestionsCountInDatabase) ? totalQuestionsCountInDatabase : QuestionCount;
                Random rnd = new Random();
                int randomNumber = rnd.Next(0, (totalQuestionsCountInDatabase- QuestionCount));

                var questionlist = new List<QuestionsResource>();
                var Questions = await _questionService.GetAllAsync(randomNumber, QuestionCount, x => x.Question, x => x.QuestionTypeId == QuestionTypeId, OrderBy.Ascending, x => x.QuestionsLocalized);
                questionlist = Mapper.Map<List<Questions>, List<QuestionsResource>>(Questions);
                return View(questionlist);
            }
            return Redirect("/");
        }

        [HttpPost]
        public async Task<ActionResult> SubmitAnswers(TrialExamAnswersResourceCollection questWithAnswers)
        {
            var examResultResource = new List<ExamResultResource>();
            if (questWithAnswers.ExamAnswersResourceList.Any())
            {
                var questions = await _questionService.GetAllAsync(0, int.MaxValue, x => x.Question, x => x.QuestionTypeId == questWithAnswers.QuestionType, OrderBy.Ascending, x => x.QuestionsLocalized);

                foreach (var eq in questions)
                {
                    var currentQuestionExistInTheExam= questWithAnswers.ExamAnswersResourceList.FirstOrDefault(x => x.QuestionId == eq.Id);
                    if (eq != null && currentQuestionExistInTheExam != null)
                    {
                        examResultResource.Add(
                                new ExamResultResource()
                                {
                                    QuestionsResource = Mapper.Map<Questions, QuestionsResource>(eq),
                                    SeletecdAnswer = currentQuestionExistInTheExam != null ? currentQuestionExistInTheExam.Answer : 0
                                }
                            );
                    }
                }

                var TotalQuestions = examResultResource.Count;
                var TotalQuestionsAttended = examResultResource.Count(x => x.SeletecdAnswer != 0);
                var TotalCorrectAnswered = 0;
                var TotalInCorrectAnswers = 0;
                //Status of Exam
                foreach (var result in examResultResource)
                {
                    if (result.QuestionsResource.Answer == result.SeletecdAnswer)
                    {
                        TotalCorrectAnswered++;
                    }
                    else
                    {
                        TotalInCorrectAnswers++;
                    }
                }

                ViewBag.TotalQuestions = TotalQuestions;
                ViewBag.TotalQuestionsAttended = TotalQuestionsAttended;
                ViewBag.TotalQuestionsUnAttended = TotalQuestions - TotalQuestionsAttended;
                ViewBag.TotalCorrectAnswered = TotalCorrectAnswered;
                ViewBag.TotalInCorrectAnswers = TotalInCorrectAnswers- (TotalQuestions - TotalQuestionsAttended);

                return Json(new
                {
                    success = true,
                    result = ViewExtensions.RenderToString(PartialView("_ViewResults", examResultResource))
                });
            }


            return Json(new
            {
                success = false
            });
        }
    }
}