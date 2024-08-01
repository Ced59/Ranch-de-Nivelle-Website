using RanchDuBonheur.Services.Interfaces;

namespace RanchDuBonheur.Services.Implementations;

public class LinkService : ILinkService
{
    public string BuildAbsoluteUri(HttpRequest request)
    {
        return string.Concat(
            request.Scheme,
            "://",
            request.Host.ToUriComponent(),
            request.PathBase.ToUriComponent(),
            request.Path.ToUriComponent(),
            request.QueryString.ToUriComponent());
    }

    public string BuildFacebookShareUrl(string abloluteUri)
    {
        return $"https://www.facebook.com/sharer/sharer.php?u={Uri.EscapeDataString(abloluteUri)}";
    }
}
