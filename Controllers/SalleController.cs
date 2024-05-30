using Microsoft.AspNetCore.Mvc;

namespace RanchDuBonheur.Controllers
{
    public class SalleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
