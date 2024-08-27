using SimpleMvcSitemap;

namespace RanchDuBonheur.Services.Interfaces.SiteMapServices;

public interface ISitemapService
{
    Task<List<SitemapNode>> GetSitemapNodesAsync();
}