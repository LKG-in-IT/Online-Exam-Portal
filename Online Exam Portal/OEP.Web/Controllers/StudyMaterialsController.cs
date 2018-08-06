using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OEP.Data;
using OEP.Resources.Admin;
using OEP.Core.Services;
using AutoMapper;
using OEP.Core.DomainModels.StudyMaterialsModel;
using OEP.Web.Helpers;

namespace OEP.Web.Controllers
{
    [AuthorizeUser(Roles = "User,Faculty,Admin")]
    public class StudyMaterialsController : Controller
    {
        private readonly IStudyMaterial _studyMaterial;
        private readonly ISubCategoryService _subCategoryService;
        public StudyMaterialsController(IStudyMaterial studyMaterial,ISubCategoryService  subCategoryService)
        {
            _studyMaterial = studyMaterial;
            _subCategoryService = subCategoryService;

        }


        // GET: StudyMaterials
        public ActionResult Index()
        {
            var subcategorylist = _subCategoryService.GetAll();
            ViewBag.subcategory = new SelectList(subcategorylist.Where(i => i.Status == true), "Id", "Name", selectedValue: "Id");
            return View();
        }
        public async Task<PartialViewResult> LoadStudyMaterials(int subcategoryid)
        {
            var StudyMaterialsList = await _studyMaterial.FindByAsync(i=>i.SubcategoryID==subcategoryid);
            var StudyMaterialsResource = Mapper.Map<List<StudyMaterial>, List<StudyMaterialsResources>>(StudyMaterialsList);

            return PartialView(StudyMaterialsResource);
        }

    }
}
