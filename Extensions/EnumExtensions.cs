using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace RanchDuBonheur.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes is { Length: > 0 } ? attributes[0].Description : value.ToString();
        }

        public static IEnumerable<SelectListItem> GetSelectList<TEnum>() where TEnum : struct, IConvertible
        {
            return Enum.GetValues(typeof(TEnum))
                .Cast<Enum>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.GetDescription()
                });
        }
    }

}
