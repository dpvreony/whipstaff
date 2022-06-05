using System.Globalization;

namespace Whipstaff.NodaTime
{
    public static class LocalDateExtensions
    {
        public static string ToYearMonthDayString(this global::NodaTime.LocalDate instance)
        {
            return instance.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
        }
    }
}
