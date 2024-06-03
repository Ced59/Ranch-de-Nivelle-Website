using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RanchDuBonheur.Extensions
{
    public static class HtmlExtensions
    {
        public static IHtmlContent FormatWithLineBreaks(this IHtmlHelper htmlHelper, string text)
        {
            if (string.IsNullOrEmpty(text))
                return new HtmlString(text);

            var encoded = htmlHelper.Encode(text);
            var formatted = encoded.Replace(htmlHelper.Encode("\r\n"), "<br />").Replace(htmlHelper.Encode("\n"), "<br />");

            return new HtmlString(formatted);
        }
    }
}
