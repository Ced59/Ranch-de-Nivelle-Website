using Microsoft.AspNetCore.Mvc;
using RanchDuBonheur.Models;
using System.Diagnostics;
using RanchDuBonheur.Services.Interfaces;

namespace RanchDuBonheur.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFacebookLinkService _facebookLinkService;

        public HomeController(ILogger<HomeController> logger, IFacebookLinkService facebookLinkService)
        {
            _logger = logger;
            _facebookLinkService = facebookLinkService;
        }

        public IActionResult Index()
        {
            var absoluteUri = _facebookLinkService.BuildAbsoluteUri(HttpContext.Request);
            ViewData["OG:Url"] = absoluteUri;
            ViewData["OG:Image"] = "https://www.ranchdubonheur.fr/images/home/PHOTO-LA-LOUVIERE-FRANCIS-FROISART.jpg";
            ViewData["OG:Description"] = "Accueil du site du Ranch du Bonheur";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
