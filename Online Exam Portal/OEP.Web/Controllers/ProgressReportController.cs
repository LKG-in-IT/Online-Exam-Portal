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
using OEP.Resources.Common;
using OEP.Core.Services;
using Microsoft.AspNet.Identity;
using OEP.Web.Helpers;

namespace OEP.Web.Controllers
{
    [AuthorizeUser(Roles = "User,Faculty,Admin")]
    [ValidatePackageStatus]
    public class ProgressReportController : Controller
    {
        IResultService _resultService;
        public ProgressReportController(IResultService resultService)
        {
            _resultService = resultService;

        }

        // GET: ProgressReport
        public async Task<ActionResult> Index()
        {
            if(Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var progressreport =await _resultService.FindByAsync(i => i.UserId == userId);
                ProgressReportResource pr = new ProgressReportResource();
                pr.ExamCount = progressreport.Count;
                pr.Win = progressreport.Where(j => j.ResultStatus == "Win").Count();
                pr.Fail = progressreport.Where(j => j.ResultStatus == "Fail").Count();





                return View(pr);
            }
            else
            {

                return RedirectToAction("Login", "Account");

            }


        }

     
    }
}
