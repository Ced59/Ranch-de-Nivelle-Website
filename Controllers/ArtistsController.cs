using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RanchDuBonheur.Data;
using RanchDuBonheur.Models.Pocos.Artists;
using RanchDuBonheur.Models.ViewModels;
using RanchDuBonheur.Models.ViewModels.Shared;
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
                var today = DateOnly.FromDateTime(DateTime.Today);

                var artist = await context.Artists
                    .Include(a => a.Videos) 
                    .Include(a => a.MealArtists)
                    .ThenInclude(ma => ma.Meal)
                    .Where(a => a.Id == artistId)
                    .Select(a => new ArtistDetailsViewModel
                    {
                        Artist = a,
                        Meals = a.MealArtists
                            .Where(ma => ma.Meal.Date >= today)
                            .Select(ma => new MealInfo
                            {
                                Date = ma.Meal.Date,
                                Id = ma.Meal.Id
                            })
                            .ToList(),
                        Videos = a.Videos.Select(v => new VideoInfo 
                        {
                            Id = v.Id,
                            Title = v.Title,
                            Description = v.Description,
                            YtLink = v.YtLink
                        }).ToList()
                    })
                    .SingleOrDefaultAsync();

                if (artist == null)
                {
                    TempData["Error"] = "Artiste non trouvé";
                    return RedirectToAction("Index");
                }


                var absoluteUri = linkService.BuildAbsoluteUri(HttpContext.Request);
                ViewData["OG:Url"] = absoluteUri;
                ViewData["FbShareUrl"] = linkService.BuildFacebookShareUrl(absoluteUri);
                ViewData["OG:Image"] = "https://www.ranchdubonheur.fr" + artist.Artist.PhotoUrl;
                ViewData["OG:Description"] = artist.Artist.Name + " : Un artiste du Ranch du Bonheur";

                return View(artist);
            }

            TempData["Error"] = "Requête invalide";
            return RedirectToAction("Index");
        }


        [Route("view-video/{videoId}")]
        public async Task<IActionResult> ViewVideos(Guid videoId)
        {
            var videoWithArtist = await context.Videos
                .Include(v => v.Artist)
                .ThenInclude(a => a.Videos)
                .FirstOrDefaultAsync(v => v.Id == videoId);

            if (videoWithArtist == null || videoWithArtist.Artist == null)
            {
                TempData["Error"] = "Vidéo ou artiste non trouvé";
                return RedirectToAction("Index");
            }

            var viewModel = new ArtistVideosViewModel
            {
                Artist = videoWithArtist.Artist,
                Videos = videoWithArtist.Artist.Videos.Select(v => new VideoInfo
                {
                    Id = v.Id,
                    Title = v.Title,
                    Description = v.Description,
                    YtLink = v.YtLink
                }).ToList(),
                IdSelectedVideo = videoId
            };

            var absoluteUri = linkService.BuildAbsoluteUri(HttpContext.Request);
            ViewData["OG:Url"] = absoluteUri;
            ViewData["FbShareUrl"] = linkService.BuildFacebookShareUrl(absoluteUri);
            ViewData["OG:Image"] = "https://www.ranchdubonheur.fr" + viewModel.Artist.PhotoUrl;
            ViewData["OG:Description"] = viewModel.Artist.Name + " : Ses vidéos";

            return View(viewModel);
        }

    }
}
