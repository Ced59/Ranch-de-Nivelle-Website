using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RanchDuBonheur.Controllers
{
    [Authorize]
    [Route("console-administration")]
    public class AdminController : Controller
    {
        [Route("accueil")]
        public IActionResult AdminIndex()
        {
            return View();
        }

        [Route("utilisateurs")]
        public IActionResult Users()
        {
            return View();
        }

        [Route("artistes")]
        public IActionResult Artists()
        {
            return View();
        }

        [Route("repas")]
        public IActionResult Meals()
        {
            return View();
        }

        [Route("infos-salle")]
        public IActionResult RoomInfo()
        {
            return View();
        }
    }
}
