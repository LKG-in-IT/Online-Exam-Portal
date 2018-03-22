using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OEP.Core.DomainModels.Test;
using OEP.Core.Services;
using OEP.Core.DomainModels.Test2;

namespace OEP.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IService<Test> _objService;
        private readonly IService<Test2> _objService2;



        public HomeController(IService<Test> objService,IService<Test2> objService2)
        {
            _objService = objService;
            _objService2 = objService2;
        }

        // GET: Home
        public ActionResult Index()
        {
            var testObj=new Test() {Name = "ee"};
            _objService.Add(testObj);
            var obj = new Test2() { Name = "dddddddddddddddd" };
            _objService2.Add(obj);
            _objService2.UnitOfWorkSaveChanges();
            return View();
        }
    }
}