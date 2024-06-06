using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RanchDuBonheur.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RanchDuBonheur.Controllers
{
    public class EventController : Controller
    {
        private readonly RanchDbContext _context;

        public EventController(RanchDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(bool showPast = false)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var meals = await _context.Meals
                .Include(m => m.MealDishes)
                .ThenInclude(md => md.Dish)
                .Include(m => m.MealArtists)
                .ThenInclude(ma => ma.Artist)
                .Where(m => showPast ? m.Date < today : m.Date >= today)
                .OrderBy(m => m.Date)  
                .ToListAsync();

            return View(new Tuple<List<RanchDuBonheur.Models.Pocos.Meals.Meal>, bool>(meals, showPast));
        }
    }
}