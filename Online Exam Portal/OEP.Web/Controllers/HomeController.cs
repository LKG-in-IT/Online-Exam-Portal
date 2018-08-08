using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using OEP.Core.DomainModels.PackageModel;
using OEP.Core.Services;
using OEP.Resources.Admin;
using OEP.Resources.Common;
using OEP.Web.Helpers;

namespace OEP.Web.Controllers
{
    [AuthorizeUser(Roles = "User,Faculty,Admin")]
    [ValidatePackageStatus]
    public class HomeController : Controller
    {
        private readonly IPackageService _packageService;
        public HomeController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        public HomeController()
        {
        }

        public ActionResult Index()
        {
            var resp = Mapper.Map<List<Package>, List<PackageResource>>(_packageService.GetAll());
            HomePageResource homePageResource = new HomePageResource() {Packages = resp };
            return View(homePageResource);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}