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
using OEP.Core.DomainModels.ResultModel;
using Microsoft.AspNet.Identity;
using OEP.Web.Helpers;

namespace OEP.Web.Controllers
{
    [AuthorizeUser(Roles = "User,Faculty,Admin")]
    public class ResultController : Controller
    {
        private readonly IResultService _resultService;

        public ResultController(IResultService resultService)
        {
            _resultService = resultService;
        }

        // GET: Result
        public ActionResult Index()
        {

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var resultlist = _resultService.FindBy(i => i.UserId == userId).ToList();
            var resultResourcelist = Mapper.Map<List< Result>,List< ResultResource>>(resultlist);
            return View(resultResourcelist);
            }
            else

            {
                return RedirectToAction("Login", "Account");
            }
        }

     
    }
}
