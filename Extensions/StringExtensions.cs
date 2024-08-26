using System.Web;

namespace RanchDuBonheur.Extensions
{
    public static class StringExtensions
    {
        public static string ExtractYouTubeVideoId(this string url)
        {
            try
            {
                var uri = new Uri(url);
                var query = HttpUtility.ParseQueryString(uri.Query);
                if (query.AllKeys.Contains("v"))
                {
                    return query["v"];
                }
                else if (uri.Segments.Length > 1 && uri.Host.Contains("youtu.be"))
                {
                    // pour les liens raccourcis youtu.be
                    return uri.Segments.Last().TrimEnd('/');
                }
            }
            catch (Exception)
            {
                // Log or handle the exception as needed
                return string.Empty;
            }
            return string.Empty;
        }
    }
}
