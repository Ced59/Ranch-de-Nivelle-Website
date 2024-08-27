using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using RanchDuBonheur.Data;
using RanchDuBonheur.Services.Interfaces.SiteMapServices;
using SimpleMvcSitemap;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ArtistsSitemapService(
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

        var indexUrl = urlHelper.Action("Index", "Artists", null, "https");
        nodes.Add(new SitemapNode(indexUrl)
        {
            Priority = 0.8m,
            ChangeFrequency = ChangeFrequency.Never
        });

        var artists = await context.Artists.ToListAsync();
        foreach (var artist in artists)
        {
            var artistUrl = urlHelper.Action("Artist", "Artists", new { id = artist.Id }, "https");
            nodes.Add(new SitemapNode(artistUrl)
            {
                Priority = 0.6m,
                ChangeFrequency = ChangeFrequency.Never
            });
        }

        var videos = await context.Videos.ToListAsync();
        foreach (var video in videos)
        {
            var videoUrl = urlHelper.Action("ViewVideos", "Artists", new { videoId = video.Id }, "https");
            nodes.Add(new SitemapNode(videoUrl)
            {
                Priority = 0.5m,
                ChangeFrequency = ChangeFrequency.Never
            });
        }

        return nodes;
    }
}
