using System;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using OEP.Core.Services;
using OEP.Resources.Admin;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using OEP.Core.Data;
using OEP.Core.DomainModels.QuestionModel;
using OEP.Web.Helpers;

namespace OEP.Web.Areas.Admin.Controllers
{
    [AuthorizeUser(Roles = "Admin")]
    public class QuestionTypeController : Controller
    {
        private readonly IService<QuestionType> _questionTypeService;

        public QuestionTypeController(IService<QuestionType> questionTypeService)
        {
            _questionTypeService = questionTypeService;
        }


        // GET: Admin/QuestionTypes
        public ActionResult Index()
        {
            return View();
        }

        // POST: Admin/QuestionTypes/LoadQuestionTypes

        public async Task<ActionResult> LoadQuestionTypes()
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

                    var questionTypeList = await _questionTypeService.GetAllAsync(
                        skip,
                        pageSize,

                        //sorting

                        x => sortColumn == "Name" ? x.Name : null,

                        //filtering
                        x => searchValue != "" ? x.Name.Contains(searchValue) : x.Id != 0,

                        //sort by
                        (sortColumnDir == "desc" ? OrderBy.Descending : OrderBy.Ascending)
                     );

                    var resp = Mapper.Map<List<QuestionType>, List<QuestionTypeResource>>(questionTypeList);

                    //total number of rows count     
                    recordsTotal = questionTypeList.TotalCount;
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = resp });
                }
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
            return Content("Error");
        }

        // GET: Admin/QuestionTypes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionType questionType = await _questionTypeService.GetByIdAsync(Convert.ToInt32(id));
            if (questionType == null)
            {
                return HttpNotFound();
            }
            var questionTypeResource = Mapper.Map<QuestionType, QuestionTypeResource>(questionType);
            return View(questionTypeResource);
        }

        // GET: Admin/QuestionTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/QuestionTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(QuestionTypeResource questionTypeResource)
        {
            if (ModelState.IsValid)
            {
                var questionType = Mapper.Map<QuestionTypeResource, QuestionType>(questionTypeResource);
                questionType.CreatedDate = DateTime.Now;
                questionType.UpdatedDate = DateTime.Now;
                var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                questionType.UserId = userId;
                await _questionTypeService.AddAsync(questionType);
                _questionTypeService.UnitOfWorkSaveChanges();
                return RedirectToAction("Index");
            }

            return View(questionTypeResource);
        }

        // GET: Admin/QuestionTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionType questionType = await _questionTypeService.GetByIdAsync(Convert.ToInt32(id));
            if (questionType == null)
            {
                return HttpNotFound();
            }
            var questionTypeResource = Mapper.Map<QuestionType, QuestionTypeResource>(questionType);
            return View(questionTypeResource);
        }

        // POST: Admin/QuestionTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(QuestionTypeResource questionTypeResource)
        {
            if (ModelState.IsValid)
            {
                var exstQuestionType = await _questionTypeService.GetByIdAsync(Convert.ToInt32(questionTypeResource.Id));
                exstQuestionType.Name = questionTypeResource.Name;
                exstQuestionType.Status = questionTypeResource.Status;
                exstQuestionType.UpdatedDate = DateTime.Now;
                var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                exstQuestionType.UserId = userId;
                await _questionTypeService.UpdateAsync(exstQuestionType);
                _questionTypeService.UnitOfWorkSaveChanges();
                return RedirectToAction("Index");
            }
            return View(questionTypeResource);
        }

        // GET: Admin/QuestionTypes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionType questionType = await _questionTypeService.GetByIdAsync(Convert.ToInt32(id));
            if (questionType == null)
            {
                return HttpNotFound();
            }
            var questionTypeResource = Mapper.Map<QuestionType, QuestionTypeResource>(questionType);

            return View(questionTypeResource);
        }

        // POST: Admin/QuestionTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            QuestionType questionType = await _questionTypeService.GetByIdAsync(Convert.ToInt32(id));
            await _questionTypeService.DeleteAsync(questionType);
            _questionTypeService.UnitOfWorkSaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // db.Dispose();
                _questionTypeService.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
