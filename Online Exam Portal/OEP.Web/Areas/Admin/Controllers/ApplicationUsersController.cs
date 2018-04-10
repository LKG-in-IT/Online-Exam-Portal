using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OEP.Core.DomainModels.Identity;
using OEP.Data;
using OEP.Core.Services;

namespace OEP.Web.Areas.Admin.Controllers
{
    public class ApplicationUsersController : Controller
    {
        private readonly IApplicationUserService _applicationUserService;

        public ApplicationUsersController(IApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
        }
        
        // GET: Admin/ApplicationUsers
        public  ActionResult Index()
        {
            return View( _applicationUserService.GetApplicationUsers());
        }
            

        
    }
}
