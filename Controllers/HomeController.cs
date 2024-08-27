using Microsoft.AspNetCore.Mvc;
using RanchDuBonheur.Models;
using RanchDuBonheur.Services.Interfaces;
using System.Diagnostics;

namespace RanchDuBonheur.Controllers
{
    public class HomeController(ILogger<HomeController> logger, ILinkService linkService) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;

        public IActionResult Index()
        {
            var absoluteUri = linkService.BuildAbsoluteUri(HttpContext.Request);
            ViewData["OG:Url"] = absoluteUri;
            ViewData["FbShareUrl"] = linkService.BuildFacebookShareUrl(absoluteUri);
            ViewData["OG:Image"] = "https://www.ranchdubonheur.fr/images/home/PHOTO-LA-LOUVIERE-FRANCIS-FROISART.jpg";
            ViewData["OG:Description"] = "Accueil du site du Ranch du Bonheur";
            ViewData["MetaDescription"] =
                "Bienvenue au Ranch du Bonheur de Nivelle, un lieu o� le bonheur se savoure � toute heure.";
            ViewData["CanonicalUrl"] = absoluteUri;

            return View();
        }

        public IActionResult Privacy()
        {
            var absoluteUri = linkService.BuildAbsoluteUri(HttpContext.Request);
            ViewData["MetaDescription"] =
                "Charte de confidentialit� du site www.ranchdubonheur.fr";
            ViewData["CanonicalUrl"] = absoluteUri;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
