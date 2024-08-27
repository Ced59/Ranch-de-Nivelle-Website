using Microsoft.AspNetCore.Mvc;

namespace RanchDuBonheur.Services.Interfaces.SiteMapServices;

public interface ISitemapGenerationService
{
    Task<IActionResult> GenerateSitemapAsync();
}