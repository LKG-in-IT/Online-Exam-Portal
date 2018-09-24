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
    [ValidatePackageStatus]
    public class ResultController : Controller
    {
        private readonly IResultService _resultService;

        public ResultController(IResultService resultService)
        {
            _resultService = resultService;
        }

        // GET: Result
        public async Task<ActionResult> Index(int? page)
        {

            if (User.Identity.IsAuthenticated)
            {
                var pageSize = 3;
                page = page != null && page != 0 ? (page-1)*pageSize: 0;
                ResultListResource _resultListResource = new ResultListResource();
                var userId = User.Identity.GetUserId();
                var resultlist = await _resultService.GetAllAsync(Convert.ToInt32(page), pageSize, x => x.CreatedDate.ToString(), x => x.UserId == userId, Core.Data.OrderBy.Descending, x=>x.Exam);
                _resultListResource.ResultResourceList = Mapper.Map<List<Result>, List<ResultResource>>(resultlist);
                _resultListResource.TotalPages = resultlist.TotalPageCount;
                _resultListResource.PageIndex = resultlist.PageIndex;
                _resultListResource.PageSize = resultlist.PageSize;
                return View(_resultListResource);
            }
            else

            {
                return RedirectToAction("Login", "Account");
            }
        }


    }
}
