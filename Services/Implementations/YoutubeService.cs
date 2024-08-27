using RanchDuBonheur.Services.Interfaces;

namespace RanchDuBonheur.Services.Implementations;

public class YoutubeService(HttpClient httpClient) : IYoutubeService
{
    public async Task<bool> IsVideoAvailable(string url)
    {
        try
        {
            var response = await httpClient.GetAsync(url);
            return response.IsSuccessStatusCode
                   && (response.RequestMessage.RequestUri.Host.Contains("youtube.com")
                       || response.RequestMessage.RequestUri.Host.Contains("youtu.be"));
        }
        catch
        {
            return false;
        }
    }
}