using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RanchDuBonheur.Extensions
{
    public static class HtmlExtensions
    {
        public static HtmlString FormatWithLineBreaks(this IHtmlHelper htmlHelper, string text)
        {
            if (string.IsNullOrEmpty(text))
                return new HtmlString(text);

            var formatted = text.Replace("\r\n", "<br />").Replace("\n", "<br />");

            var encoded = htmlHelper.Encode(formatted);


            return new HtmlString(formatted);
        }
    }
}
