using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RanchDuBonheur.Data;

namespace RanchDuBonheur.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly RanchDbContext _context;

        public ArtistsController(RanchDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var artists = await _context.Artists.ToListAsync();

            return View(artists);
        }

        public async Task<IActionResult> Artist()
        {
            return View();
        }
    }
}
