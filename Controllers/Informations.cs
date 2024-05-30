using Microsoft.AspNetCore.Mvc;

namespace RanchDuBonheur.Controllers
{
    public class Informations : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
