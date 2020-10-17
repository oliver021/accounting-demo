using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace OliWorkshop.AccountingSys.Helpers
{
    public static class MiscHelpers
    {
        public static IEnumerable<string> GetDaysOfWeekFromNow(CultureInfo culture, bool full = false)
        {
            if (culture is null)
            {
                culture = CultureInfo.CurrentCulture;
            }

            for (int i = 6; i > -1; i--)
            {
                yield return DateTime.Now.AddDays(-i).ToString(full ? "dddd": "ddd", culture);
            }
        }

        public static IEnumerable<string> GetDaysOfWeekFromDate(CultureInfo culture, DateTime date, bool full = false)
        {
            if (culture is null)
            {
                culture = CultureInfo.CurrentCulture;
            }

            for (int i = 6; i > -1; i--)
            {
                yield return date.AddDays(-i).ToString(full ? "dddd" : "ddd", culture);
            }
        }

        public static IEnumerable<DateTime> GetDateTimesFromBack()
        {
           
            for (int i = 6; i > -1; i--)
            {
                yield return DateTime.Now.AddDays(-i);
            }
        }

        public static IEnumerable<DateTime> GetDateTimesFromBack(DateTime date)
        {
            for (int i = 6; i > -1; i--)
            {
                yield return date.AddDays(-i);
            }
        }
    }
}
