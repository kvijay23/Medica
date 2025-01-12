using System;
using System.Globalization;

namespace Medica.Employment.API.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime? TryParseExactOrNull(this string input, string format = "dd/MM/yyyy")
        {
            if (DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                return parsedDate;
            }

            return null;
        }
    }
}
