using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RanchDuBonheur.Data;
using RanchDuBonheur.Models.Pocos.Artists;
using RanchDuBonheur.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RanchDuBonheur.Extensions;
using RanchDuBonheur.Services.Interfaces;

namespace RanchDuBonheur.Controllers
{
    public class EventController(RanchDbContext context, ILinkService linkService) : Controller
    {
        public async Task<IActionResult> Index(bool showPast = false)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var meals = await context.Meals
                .Include(m => m.MealDishes)
                .ThenInclude(md => md.Dish)
                .Include(m => m.MealArtists)
                .ThenInclude(ma => ma.Artist)
                .Where(m => showPast ? m.Date < today : m.Date >= today)
                .OrderBy(m => m.Date)
                .ToListAsync();

            return View(new Tuple<List<RanchDuBonheur.Models.Pocos.Meals.Meal>, bool>(meals, showPast));
        }

        public async Task<IActionResult> Event(string idEvent)
        {
            if (Guid.TryParse(idEvent, out var idEventGuid))
            {
                var meal = await context.Meals
                    .Include(md => md.MealDishes)
                    .ThenInclude(md => md.Dish)
                    .Include(m => m.MealArtists)
                    .ThenInclude(ma => ma.Artist)
                .Where(m => m.Id == idEventGuid)
                    .FirstOrDefaultAsync();

                var absoluteUri = linkService.BuildAbsoluteUri(HttpContext.Request);
                ViewData["OG:Url"] = absoluteUri;
                ViewData["FbShareUrl"] = linkService.BuildFacebookShareUrl(absoluteUri);
                ViewData["OG:Image"] = "https://www.ranchdubonheur.fr" + meal.MealArtists.ToList()[0].Artist.PhotoUrl;
                ViewData["OG:Description"] = "Voir le repas du " + meal.Date.GetCapitalizedDate() + " au Ranch du bonheur à Nivelle";

                return View(meal);
            }

            TempData["Error"] = "Requête invalide";
            return RedirectToAction("Index");

        }
    }
}