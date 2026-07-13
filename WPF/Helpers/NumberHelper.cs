using System.Globalization;

namespace WPF.Helpers
{
    public static class NumberHelper
    {
        public static string FormatDecimal(string value, string format = "N0")
        {
            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal val) ||
                decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out val))
            {
                return val.ToString(format, CultureInfo.CurrentCulture);
            }
            return value;
        }

        public static string FormatInt(string value, string format = "N0")
        {
            if (int.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out int val) ||
                int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out val))
            {
                return val.ToString(format, CultureInfo.CurrentCulture);
            }
            return value;
        }

        public static bool TryParseDecimal(string value, out decimal result)
        {
            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out result))
            {
                return true;
            }
            return decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
        }

        public static bool TryParseInt(string value, out int result)
        {
            if (int.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out result))
            {
                return true;
            }
            return int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
        }
    }
}
