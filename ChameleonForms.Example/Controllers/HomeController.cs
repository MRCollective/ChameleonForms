using Microsoft.AspNetCore.Mvc;

namespace ChameleonForms.Example.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
