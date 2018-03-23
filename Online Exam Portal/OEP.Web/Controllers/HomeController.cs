using System.Web.Mvc;

namespace OEP.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}