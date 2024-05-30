using Microsoft.AspNetCore.Mvc;

namespace RanchDuBonheur.Controllers
{
    public class EventController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
