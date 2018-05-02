using AutoMapper;
using OEP.Core.Data;
using OEP.Core.DomainModels.ExamModels;
using OEP.Core.Services;
using OEP.Resources.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace OEP.Web.Controllers
{
    public class ExamController : Controller
    {

        private readonly ICategoryService _categoryService;
        private readonly ISubCategoryService _subCategoryService;
        private readonly IExamTypeService _examTypeService;

        private readonly IExamservice _examservice;



        public ExamController(ICategoryService categoryService, ISubCategoryService subCategoryService, IExamTypeService examTypeService, IExamservice examservice)
        {
            _categoryService = categoryService;
            _subCategoryService = subCategoryService;
            _examTypeService = examTypeService;
            _examservice = examservice;
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

        public async Task<JsonResult> SearchExam(ExamList examList )
        {

            int sid=0, eid=0;
            if (examList.SubcategoryId != null&& examList.SubcategoryId != "0")
            {
                sid = Convert.ToInt32(examList.SubcategoryId);
            }
            if(examList.Examtypeid != null)
            {
                eid = Convert.ToInt32(examList.Examtypeid);

            }

    
       

            var examlist = await _examservice.GetAllAsync(

            examList.skip,
                       examList.pageSize,

                        //sorting

                        x => x.Name ,

                        //filtering
                        x => examList.KeyWord != null ? x.Name.Contains(examList.KeyWord) : (examList.SubcategoryId !="0" && examList.SubcategoryId !=null) ?x.SubcategoryId==sid :(examList.Examtypeid != "0" && examList.Examtypeid !=null?x.ExamtypeId==eid:x.Id!=0),




                        //sort by
                        OrderBy.Descending 





                );


            var  recordsTotal = examlist.TotalCount;
            var totalItem = examlist.Count();
            var resp = Mapper.Map<List<Exam>, List<ExamResource>>(examlist);
            return Json(new { exam= resp, total=recordsTotal, totalItem= totalItem });

        }


    }
}