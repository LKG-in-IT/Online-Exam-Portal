using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OEP.Web.Helpers;

namespace OEP.Web.Areas.Admin.Controllers
{

    [AuthorizeUser(Roles = "Admin,Faculty")]
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}