using Microsoft.AspNetCore.Mvc;

namespace RanchDuBonheur.Controllers
{
    public class ArtistsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
