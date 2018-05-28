using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using OEP.Core.Data;
using OEP.Core.DomainModels.EducationModels;
using OEP.Core.Services;
using OEP.Data;
using OEP.Resources.Admin;
using OEP.Web.Helpers;

namespace OEP.Web.Areas.Admin.Controllers
{
    [AuthorizeUser(Roles = "Admin")]
    public class EducationTypeController : Controller
    {
        private readonly IService<EducationType> _educationTypeService;

        public EducationTypeController(IService<EducationType> educationTypeService )
        {
            _educationTypeService = educationTypeService;
        }

        // GET: Admin/EducationType
        public ActionResult Index()
        {
            return View();
        }

        // POST: Admin/EducationType/LoadEducationTypes

        public async Task<ActionResult> LoadEducationTypes()
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

                    var educationTypeList = await _educationTypeService.GetAllAsync(
                        skip,
                        pageSize,

                        //sorting
                        x => sortColumn == "Name" ? x.Name : null,

                        //filtering
                        x => searchValue != "" ? x.Name.Contains(searchValue) : x.Id != 0,

                        //sort by
                        (sortColumnDir == "desc" ? OrderBy.Descending : OrderBy.Ascending)
                     );

                    var resp = Mapper.Map<List<EducationType>, List<EducationTypeResource>>(educationTypeList);

                    //total number of rows count     
                    recordsTotal = educationTypeList.TotalCount;
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = resp });
                }
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
            return Content("Error");
        }

        // GET: Admin/EducationType/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EducationType educationType = await _educationTypeService.GetByIdAsync(Convert.ToInt32(id));
            if (educationType == null)
            {
                return HttpNotFound();
            }
            var educationTypeResource = Mapper.Map<EducationType, EducationTypeResource>(educationType);
            return View(educationTypeResource);
        }

        // GET: Admin/EducationType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/EducationType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EducationTypeResource EducationTypeResource)
        {
            if (ModelState.IsValid)
            {
                var educationType = Mapper.Map<EducationTypeResource, EducationType>(EducationTypeResource);
                educationType.CreatedDate = DateTime.Now;
                educationType.UpdatedDate = DateTime.Now;
                var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                educationType.UserId = userId;
                await _educationTypeService.AddAsync(educationType);
                _educationTypeService.UnitOfWorkSaveChanges();
                return RedirectToAction("Index");
            }

            return View(EducationTypeResource);
        }

        // GET: Admin/EducationType/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EducationType educationType = await _educationTypeService.GetByIdAsync(Convert.ToInt32(id));
            if (educationType == null)
            {
                return HttpNotFound();
            }
            var educationTypeResource = Mapper.Map<EducationType, EducationTypeResource>(educationType);
            return View(educationTypeResource);
        }

        // POST: Admin/EducationType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EducationTypeResource educationTypeResource)
        {
            if (ModelState.IsValid)
            {
                var exstEducationType = await _educationTypeService.GetByIdAsync(Convert.ToInt32(educationTypeResource.Id));
                exstEducationType.Name = educationTypeResource.Name;
                exstEducationType.Status = educationTypeResource.Status;
                exstEducationType.UpdatedDate = DateTime.Now;
                var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                exstEducationType.UserId = userId;

                await _educationTypeService.UpdateAsync(exstEducationType);
                _educationTypeService.UnitOfWorkSaveChanges();
                return RedirectToAction("Index");
            }
            return View(educationTypeResource);
        }

        // GET: Admin/EducationType/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EducationType educationType = await _educationTypeService.GetByIdAsync(Convert.ToInt32(id));
            if (educationType == null)
            {
                return HttpNotFound();
            }
            var educationTypeResource = Mapper.Map<EducationType, EducationTypeResource>(educationType);

            return View(educationTypeResource);
        }

        // POST: Admin/EducationType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            EducationType educationType = await _educationTypeService.GetByIdAsync(Convert.ToInt32(id));
            await _educationTypeService.DeleteAsync(educationType);
            _educationTypeService.UnitOfWorkSaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // db.Dispose();
                _educationTypeService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
