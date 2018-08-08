﻿using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using OEP.Core.Data;
using OEP.Core.DomainModels.ExamModels;
using OEP.Core.DomainModels.PackageModel;
using OEP.Core.DomainModels.QuestionModel;
using OEP.Core.DomainModels.ResultModel;
using OEP.Core.Services;
using OEP.Resources.Admin;
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
    public class ExamController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ISubCategoryService _subCategoryService;
        private readonly IExamTypeService _examTypeService;
        private readonly IExamQuestionService _examQuestionService;
        private readonly IExamservice _examservice;
        private readonly IQuestionService _questionService;
        private readonly IPackageService _packageService;
        private readonly IResultService _resultService;

        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;
        ExamResource examresource;

        public ExamController(ICategoryService categoryService, IResultService resultService, ISubCategoryService subCategoryService, IQuestionService questionService,
            IExamTypeService examTypeService, IExamQuestionService examQuestionService, IExamservice examservice, IPackageService packageService)
        {
            _resultService = resultService;
            _categoryService = categoryService;
            _subCategoryService = subCategoryService;
            _examTypeService = examTypeService;
            _examservice = examservice;
            _packageService = packageService;
            _examQuestionService = examQuestionService;
            _questionService = questionService;

        }
        public ExamController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
        // GET: Exam
        public ActionResult Index()
        {
            var categorylist = _categoryService.GetAll();
            ViewBag.CategoryId = new SelectList(categorylist.Where(i => i.Status == true), "Id", "Name", selectedValue: "Id");

            var examtypelist = _examTypeService.GetAll();

            ViewBag.ExamTypeId = new SelectList(examtypelist.Where(i => i.Status == true), "Id", "Name");


            return View();
        }





        //POST: SubCategory

        public JsonResult SubCategory(int categoryId)
        {

            var subcategorylist = _subCategoryService.GetAll().Where(i => i.CategoryId == categoryId).ToList();

            return Json(subcategorylist);

        }

        //POST: Search

        public async Task<JsonResult> SearchExam(ExamList examList)
        {

            int subCategoryId = 0, examtypeid = 0, categoryId=0;
            if (examList.CategoryId != null && examList.CategoryId != "0")
            {
                categoryId = Convert.ToInt32(examList.CategoryId);
            }
            if (examList.SubcategoryId != null && examList.SubcategoryId != "0")
            {
                subCategoryId = Convert.ToInt32(examList.SubcategoryId);
            }
            if (examList.Examtypeid != null)
            {
                examtypeid = Convert.ToInt32(examList.Examtypeid);

            }

            var examlist = await _examservice.GetAllAsync(

                        examList.skip,
                        examList.pageSize,

                        //sorting

                        x => x.Name,

                        //filtering
                        x => (categoryId!=0?x.SubCategory.CategoryId==categoryId:x.Status)&&
                             (subCategoryId != 0 ? x.SubcategoryId == subCategoryId : x.Status) &&
                             (examtypeid != 0 ? x.ExamtypeId == examtypeid : x.Status) &&
                             (examList.KeyWord != null ? x.Name.Contains(examList.KeyWord) : x.Status),

                        //sort by
                        OrderBy.Descending


                );            

            var recordsTotal = examlist.TotalCount;
            var totalItem = examlist.Count();
            var resp = Mapper.Map<List<Exam>, List<ExamResource>>(examlist);
            return Json(new { exam = resp, total = recordsTotal, totalItem = totalItem });

        }        

        // GET: Start Exam
        public ActionResult ViewExam(int? ExamId)
        {
            if (ExamId != null)
            {
                Exam examlist = _examservice.GetById(Convert.ToInt32(ExamId));
                if (examlist != null)
                {
                    examresource = Mapper.Map<Exam, ExamResource>(examlist);
                    ViewBag.count = _examQuestionService.FindBy(i => i.ExamId == ExamId).Count();
                    ViewBag.NotFound = false;
                    return View(examresource);
                }   
            }
            ViewBag.NotFound = true;
            return View();

        }

        public ActionResult StartExam(int ExamId)
        {
            return View();
        }

        //  /Exam/GetQuestions
        public JsonResult GetQuestions(int ExamId)
        {

            var questiolist = _examQuestionService.FindBy(i => i.ExamId == ExamId).Select(x => x.Questions).ToList();
            var examresource = Mapper.Map<List<Questions>, List<QuestionsViewResource>>(questiolist);
            var jsonObj = JsonConvert.SerializeObject(examresource);
            return Json(jsonObj, JsonRequestBehavior.AllowGet);

        }
        // get /Exam/GetAnswer
        public JsonResult GetAnswer(int Qid)
        {
            var answer = _questionService.GetById(Qid).Answer;
            return Json(answer, JsonRequestBehavior.AllowGet);

        }
        // get /Exam/PassMark
        public JsonResult GetPassMark(int ExamId)
        {
            var Passmark = _examservice.GetById(ExamId).Passmark;
            return Json(Passmark, JsonRequestBehavior.AllowGet);

        }
        // post /Exam/AddResult
        public JsonResult AddResult(ResultResource resultresource)
        {

            var result = Mapper.Map<ResultResource, Result>(resultresource);
            result.CreatedDate = DateTime.Now;
            result.UpdatedDate = DateTime.Now;
            var userId = User.Identity.GetUserId();
            result.UserId = userId;
            var passmark = _examservice.GetById(resultresource.ExamId).Passmark;
            result.ResultStatus = passmark <= resultresource.Mark ? "Win" : "Fail";
            result.Status = true;
            _resultService.Add(result);
            _resultService.UnitOfWorkSaveChanges();
            return Json(result.ResultStatus);


        }

    }
}