using System;

namespace Our.Umbraco.OpeningHours.Extensions {

    /// <summary>
    /// Various extension methods for <see cref="DateTime"/> and <see cref="DateTimeOffset"/> used throughout the package.
    /// </summary>
    public static class DateTimeExtensions {

        /// <summary>
        /// Gets whether <code>first</code> and <code>second</code> represents the same day.
        /// </summary>
        /// <param name="first">The first date.</param>
        /// <param name="second">The second date.</param>
        /// <returns>Returns <code>true</code> if <code>first</code> and <code>second</code> represents the same day, otherwise <code>false</code>.</returns>
        public static bool IsSameDay(this DateTime first, DateTime second) {
            return first.Year == second.Year && first.Month == second.Month && first.Day == second.Day;
        }

        /// <summary>
        /// Gets whether <code>first</code> and <code>second</code> represents the same day.
        /// </summary>
        /// <param name="first">The first date.</param>
        /// <param name="second">The second date.</param>
        /// <returns>Returns <code>true</code> if <code>first</code> and <code>second</code> represents the same day, otherwise <code>false</code>.</returns>
        public static bool IsSameDay(this DateTimeOffset first, DateTimeOffset second) {
            return first.Year == second.Year && first.Month == second.Month && first.Day == second.Day;
        }

        /// <summary>
        /// Gets whether the specified <code>date</code> is today.
        /// </summary>
        /// <param name="date">The date date.</param>
        /// <returns>Returns <code>true</code> if <code>date</code> is today, otherwise <code>false</code>.</returns>
        public static bool IsToday(this DateTime date) {
            return IsSameDay(date, DateTime.Today);
        }

        /// <summary>
        /// Gets whether the specified <code>date</code> is today.
        /// </summary>
        /// <param name="date">The date date.</param>
        /// <returns>Returns <code>true</code> if <code>date</code> is today, otherwise <code>false</code>.</returns>
        public static bool IsToday(this DateTimeOffset date) {
            return IsSameDay(date, DateTimeOffset.Now);
        }

        /// <summary>
        /// Gets whether the specified <code>date</code> is tomorrow.
        /// </summary>
        /// <param name="date">The date date.</param>
        /// <returns>Returns <code>true</code> if <code>date</code> is tomorrow, otherwise <code>false</code>.</returns>
        public static bool IsTomorrow(this DateTime date) {
            return IsSameDay(date, DateTime.Today.AddDays(1));
        }

        /// <summary>
        /// Gets whether the specified <code>date</code> is tomorrow.
        /// </summary>
        /// <param name="date">The date date.</param>
        /// <returns>Returns <code>true</code> if <code>date</code> is tomorrow, otherwise <code>false</code>.</returns>
        public static bool IsTomorrow(this DateTimeOffset date) {
            return IsSameDay(date, DateTimeOffset.Now.AddDays(1));
        }

    }

}