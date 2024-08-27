using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RanchDuBonheur.Data;
using SimpleMvcSitemap;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Routing;
using RanchDuBonheur.Services.Interfaces.SiteMapServices;

public class EventSitemapService(
    RanchDbContext context,
    IUrlHelperFactory urlHelperFactory,
    IHttpContextAccessor httpContextAccessor)
    : ISitemapService
{
    public async Task<List<SitemapNode>> GetSitemapNodesAsync()
    {
        var nodes = new List<SitemapNode>();
        var urlHelper = urlHelperFactory.GetUrlHelper(new ActionContext
        {
            HttpContext = httpContextAccessor.HttpContext,
            RouteData = httpContextAccessor.HttpContext.GetRouteData(),
            ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor()
        });

        // Récupérer les repas
        var meals = await context.Meals.ToListAsync();

        // Générer les URLs pour chaque repas
        foreach (var meal in meals)
        {
            var eventUrl = urlHelper.Action("Event", "Event", new { idEvent = meal.Id }, "https");
            nodes.Add(new SitemapNode(eventUrl)
            {
                Priority = 0.7m,
                ChangeFrequency = ChangeFrequency.Never
            });
        }

        return nodes;
    }
}