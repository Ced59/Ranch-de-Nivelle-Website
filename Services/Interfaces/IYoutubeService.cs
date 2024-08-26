namespace RanchDuBonheur.Services.Interfaces;

public interface IYoutubeService
{
    Task<bool> IsVideoAvailable(string url);
}