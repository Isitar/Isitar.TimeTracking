namespace Isitar.TimeTracking.Frontend.Common
{
    using System;
    using System.Globalization;

    public class Formatter
    {
        public static string FromatDouble(double d, int decimals = 2)
        {
            return Math.Round(d, 2).ToString(CultureInfo.InvariantCulture);
        }
    }
}