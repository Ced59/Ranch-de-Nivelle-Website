namespace RanchDuBonheur.Extensions;

public static class DateExtensions
{
    public static string GetCapitalizedDate(this DateOnly date)
    {
        var dateTime = date.ToDateTime(new TimeOnly(0));
        var formattedDate = dateTime.ToString("dddd d MMMM yyyy", new System.Globalization.CultureInfo("fr-FR"));
        return char.ToUpper(formattedDate[0]) + formattedDate.Substring(1);
    }
}