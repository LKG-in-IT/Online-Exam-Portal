using System.Web.Mvc;

namespace OEP.Web.Controllers
{
    public class HomeController : Controller
    {



        public HomeController()
        {
        }

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}