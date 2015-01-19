using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CaldavConnector.Converter
{

    /// <summary>
    /// This helper class converts a String into a DateTime.
    /// </summary>
    public static class StringToDateTimeConverter
    {
        private static Regex rxDate = new Regex(@"(\d{4})(\d{2})(\d{2})T?(\d{2}?)(\d{2}?)(\d{2}?)(Z?)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Does the convertion from String to DateTime.
        /// </summary>
        /// <param name="value">String to convert.</param>
        /// <returns>Converted DateTime.</returns>
        public static DateTime? Convert(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            if (!value.Contains("T"))
                value += "T000000";
            DateTime ret;
            var match = rxDate.Match(value);
            if (match.Success)
                return new DateTime(
                    match.Groups[1].Value.ToInt() ?? 0,
                    match.Groups[2].Value.ToInt() ?? 0,
                    match.Groups[3].Value.ToInt() ?? 0,
                    match.Groups[4].Value.ToInt() ?? 0,
                    match.Groups[5].Value.ToInt() ?? 0,
                    match.Groups[6].Value.ToInt() ?? 0,
                 match.Groups[match.Groups.Count - 1].Value.Is("Z") ? DateTimeKind.Utc : DateTimeKind.Unspecified);
            else if (DateTime.TryParse(value, out ret))
                return ret;

            return (DateTime?)null;
        }
        private static int? ToInt(this string input)
        {
            int ret;
            if (int.TryParse(input, out ret))
                return ret;
            else return (int?)null;
        }
        private static bool Is(this string input, string other)
        {
            return string.Equals(input ?? string.Empty, other ?? string.Empty, StringComparison.OrdinalIgnoreCase);
        }
    }
}
