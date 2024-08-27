using RanchDuBonheur.Services.Interfaces;

namespace RanchDuBonheur.Services.Implementations;

public class LinkService : ILinkService
{
    public string BuildAbsoluteUri(HttpRequest request)
    {
        var scheme = "https"; // Force HTTPS
        var host = request.Host.Host;

        if (!host.StartsWith("www."))
        {
            host = "www." + host;
        }

        return string.Concat(
            scheme,
            "://",
            host,
            request.PathBase.ToUriComponent(),
            request.Path.ToUriComponent(),
            request.QueryString.ToUriComponent());
    }


    public string BuildFacebookShareUrl(string abloluteUri)
    {
        return $"https://www.facebook.com/sharer/sharer.php?u={Uri.EscapeDataString(abloluteUri)}";
    }
}
