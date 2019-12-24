using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabSanat.Helpers
{
    public static class Translator
    {
        public static string DayName(DayOfWeek day)
        {
            var culture = new System.Globalization.CultureInfo("tr-TR");
            var dayName = culture.DateTimeFormat.GetDayName(day);

            return dayName;
        }
    }
}
