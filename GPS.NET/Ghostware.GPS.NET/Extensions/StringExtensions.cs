using System.Globalization;

namespace Ghostware.GPS.NET.Extensions
{
    public static class StringExtensions
    {
        public static double? GetDouble(this string value)
        {
            double result;

            //Try parsing in the current culture
            if (!double.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out result) &&
                //Then try in US english
                !double.TryParse(value, NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out result) &&
                //Then in neutral language
                !double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                return null;
            }

            return result;
        }
    }
}
