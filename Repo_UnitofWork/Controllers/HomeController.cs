using Microsoft.AspNetCore.Mvc;

namespace Repo_UnitofWork.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()

        {
            ViewBag.Message = TempData["Message"];
            return View();
        }
    }
}
