using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RanchDuBonheur.Data;
using RanchDuBonheur.Services.Interfaces;

namespace RanchDuBonheur.Controllers
{
    [Route("artistes")]
    public class ArtistsController(RanchDbContext context, ILinkService linkService) : Controller
    {
        [Route("accueil")]
        public async Task<IActionResult> Index()
        {
            var artists = await context.Artists.ToListAsync();

            return View(artists);
        }


        [Route("detail")]
        public async Task<IActionResult> Artist(string id)
        {
            if (Guid.TryParse(id, out var artistId))
            {
                var artist = await context.Artists.SingleOrDefaultAsync(artist => artist.Id == artistId);
                if (artist == null)
                {
                    TempData["Error"] = "Artiste non trouvé";
                    return RedirectToAction("Index");
                }

                var absoluteUri = linkService.BuildAbsoluteUri(HttpContext.Request);
                ViewData["OG:Url"] = absoluteUri;
                ViewData["FbShareUrl"] = linkService.BuildFacebookShareUrl(absoluteUri);
                ViewData["OG:Image"] = "https://www.ranchdubonheur.fr" + artist.PhotoUrl;
                ViewData["OG:Description"] = artist.Name + " : Un artiste du Ranch du Bonheur";

                return View(artist);
            }

            TempData["Error"] = "Requête invalide";
            return RedirectToAction("Index");
        }

    }
}
