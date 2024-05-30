using Microsoft.AspNetCore.Mvc;

namespace RanchDuBonheur.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
