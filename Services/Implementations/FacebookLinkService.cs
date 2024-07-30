using RanchDuBonheur.Services.Interfaces;

namespace RanchDuBonheur.Services.Implementations;

public class FacebookLinkService : IFacebookLinkService
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
}
