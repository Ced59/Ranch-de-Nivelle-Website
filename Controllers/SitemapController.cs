using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SimpleMvcSitemap;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using RanchDuBonheur.Services.Interfaces.SiteMapServices;

namespace RanchDuBonheur.Controllers
{
    [Route("sitemap")]
    public class SitemapController(ISitemapGenerationService sitemapGenerationService)
        : Controller
    {
        [HttpGet]
        [Route("sitemap.xml")]
        public async Task<IActionResult> GenerateSitemap()
        {
            var sitemap = await sitemapGenerationService.GenerateSitemapAsync();

            return sitemap;
        }

    }
}