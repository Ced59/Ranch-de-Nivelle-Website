using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using RanchDuBonheur.Services.Interfaces.SiteMapServices;
using SimpleMvcSitemap;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public class SitemapGenerationService(
    IWebHostEnvironment env,
    IEnumerable<ISitemapService> sitemapServices,
    IUrlHelperFactory urlHelperFactory,
    IHttpContextAccessor httpContextAccessor)
    : ISitemapGenerationService
{
    public async Task<IActionResult> GenerateSitemapAsync()
    {
        var nodes = new List<SitemapNode>();

        // Créer IUrlHelper à partir du contexte actuel
        var urlHelper = urlHelperFactory.GetUrlHelper(new ActionContext
        {
            HttpContext = httpContextAccessor.HttpContext,
            RouteData = httpContextAccessor.HttpContext.GetRouteData(),
            ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor()
        });

        // Ajouter la page d'accueil avec la priorité maximale
        nodes.Add(new SitemapNode(urlHelper.Action("Index", "Home", null, "https"))
        {
            Priority = 1.0m,
            ChangeFrequency = ChangeFrequency.Never
        });

        // Récupérer les URLs de tous les services avec leur priorité
        foreach (var service in sitemapServices)
        {
            var serviceNodes = await service.GetSitemapNodesAsync();
            nodes.AddRange(serviceNodes);
        }

        // Générer le sitemap
        return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));

    }
}
