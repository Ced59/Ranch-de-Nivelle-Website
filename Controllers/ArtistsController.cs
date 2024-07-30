using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RanchDuBonheur.Data;
using RanchDuBonheur.Services.Interfaces;

namespace RanchDuBonheur.Controllers
{
    [Route("artistes")]
    public class ArtistsController : Controller
    {
        private readonly RanchDbContext _context;
        private readonly IFacebookLinkService _facebookLinkService;

        public ArtistsController(RanchDbContext context, IFacebookLinkService facebookLinkService)
        {
            _context = context;
            _facebookLinkService = facebookLinkService;
        }

        [Route("accueil")]
        public async Task<IActionResult> Index()
        {
            var artists = await _context.Artists.ToListAsync();

            return View(artists);
        }


        [Route("detail")]
        public async Task<IActionResult> Artist(string id)
        {
            if (Guid.TryParse(id, out Guid artistId))
            {
                var artist = await _context.Artists.SingleOrDefaultAsync(artist => artist.Id == artistId);
                if (artist == null)
                {
                    TempData["Error"] = "Artiste non trouvé";
                    return RedirectToAction("Index");
                }

                var absoluteUri = _facebookLinkService.BuildAbsoluteUri(HttpContext.Request);
                ViewData["OG:Url"] = absoluteUri;
                ViewData["OG:Image"] = "https://www.ranchdubonheur.fr" + artist.PhotoUrl;
                ViewData["OG:Description"] = artist.Name + " : Un artiste du Ranch du Bonheur";

                return View(artist);
            }
            else
            {
                TempData["Error"] = "Requête invalide";
                return RedirectToAction("Index");
            }
        }

    }
}
