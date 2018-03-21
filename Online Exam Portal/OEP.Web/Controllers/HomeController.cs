using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OEP.Core.DomainModels.Test;
using OEP.Core.Services;

namespace OEP.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IService<Test> _objService;

        public HomeController(IService<Test> objService)
        {
            _objService = objService;
        }

        // GET: Home
        public ActionResult Index()
        {
            var testObj=new Test() {Name = "Abdul"};
            _objService.Add(testObj);
            return View();
        }
    }
}