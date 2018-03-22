using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OEP.Web.Areas.Admin.Controllers
{
    [RouteArea("Admin")]
    public class HomeController : Controller
    {
        // GET: Admin/Home
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }
    }
}