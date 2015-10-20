using System;

namespace Our.Umbraco.OpeningHours.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        ///     Extension method for getting the last week day in month
        /// </summary>
        /// <param name="date">DateTime</param>
        /// <param name="day">DayOfWeek</param>
        /// <returns>DateTime</returns>
        public static DateTime GetLastWeekDayOfMonth(this DateTime date, DayOfWeek day)
        {
            DateTime lastDayOfMonth = new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);

            int wantedDay = (int)day;
            int lastDay = (int)lastDayOfMonth.DayOfWeek;

            return lastDayOfMonth.AddDays(lastDay >= wantedDay ? wantedDay - lastDay : wantedDay - lastDay - 7);
        }

        /// <summary>
        ///     Extension method for getting the first sunday of a month
        /// </summary>
        /// <param name="date">DateTime</param>
        /// <returns>DateTime</returns>
        public static DateTime GetFirstSundayOfMonth(this DateTime date)
        {
            DateTime startDate = new DateTime(date.Year, date.Month, 1);

            while (startDate.DayOfWeek != DayOfWeek.Sunday)
            {
                startDate = startDate.AddDays(1);
            }

            return startDate;
        }
    }
}