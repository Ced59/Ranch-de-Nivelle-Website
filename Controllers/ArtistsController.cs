using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RanchDuBonheur.Data;

namespace RanchDuBonheur.Controllers
{
    [Route("artistes")]
    public class ArtistsController : Controller
    {
        private readonly RanchDbContext _context;

        public ArtistsController(RanchDbContext context)
        {
            _context = context;
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
