namespace RanchDuBonheur.Services.Interfaces;

public interface ILinkService
{
    string BuildAbsoluteUri(HttpRequest request);
    string BuildFacebookShareUrl(string abloluteUri);
}
