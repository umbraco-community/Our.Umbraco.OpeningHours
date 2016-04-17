using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Our.Umbraco.OpeningHours.Model {
    
    /// <summary>
    /// Class representing the model of the <strong>Opening Hours</strong> property editor.
    /// </summary>
    public class OpeningHours {

        private readonly Dictionary<string, HolidayOpeningHours> _holidays;

        #region Properties

        /// <summary>
        /// Gets a dictionary of the standard weekdays as specified in the property editor.
        /// </summary>
        [JsonProperty("weekdays")]
        public Dictionary<DayOfWeek, WeekdayOpeningHours> Weekdays { get; private set; }

        /// <summary>
        /// Gets a list the holidays as specified in the property editor.
        /// </summary>
        [JsonProperty("holidays")]
        public List<HolidayOpeningHours> Holidays { get; private set; }

        /// <summary>
        /// Gets whether the entity (store, company or similar) is currently open.
        /// </summary>
        [JsonIgnore]
        public bool IsCurrentlyOpen {
            get { return GetDay(DateTime.Now).IsCurrentlyOpen; }
        }

        /// <summary>
        /// Gets whether the entity (store, company or similar) is currently closed.
        /// </summary>
        [JsonIgnore]
        public bool IsCurrentlyClosed {
            get { return GetDay(DateTime.Now).IsCurrentlyClosed; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance with default options.
        /// </summary>
        public OpeningHours() {
            Weekdays = new Dictionary<DayOfWeek, WeekdayOpeningHours>();
            for (int i = 0; i < 7; i++) {
                Weekdays.Add((DayOfWeek) i, new WeekdayOpeningHours());
            }
            Holidays = new List<HolidayOpeningHours>();
            _holidays = new Dictionary<string, HolidayOpeningHours>();
        }

        /// <summary>
        /// Initializes a new instance with the specified <code>weekdays</code> and <code>holidays</code>.
        /// </summary>
        /// <param name="weekdays">The weekdays that should make up the instance.</param>
        /// <param name="holidays">The holidays that should make up the model.</param>
        public OpeningHours(Dictionary<DayOfWeek, WeekdayOpeningHours> weekdays, List<HolidayOpeningHours> holidays) {
            Weekdays = weekdays ?? new Dictionary<DayOfWeek, WeekdayOpeningHours>();
            Holidays = holidays ?? new List<HolidayOpeningHours>();
            _holidays = Holidays.Where(x => x.Date != null).ToDictionary(x => x.Date.Value.ToString("yyyyMMdd"));
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Gets whether the entity is open at the specified <code>dayOfWeek</code>. Notice that since this is just the
        /// weekday, holidays wion't be taken into account.
        /// </summary>
        /// <param name="dayOfWeek">The day of the week.</param>
        /// <returns>Returns <code>true</code> if the entity is open at the specified <code>dayOfWeek</code>, otherwise
        /// <code>false</code>.</returns>
        public bool IsOpen(DayOfWeek dayOfWeek) {
            return Weekdays[dayOfWeek].IsOpen;
        }

        /// <summary>
        /// Gets whether the entity is open during the day of the specified <code>date</code>. If
        /// <strong>Require Holiday Dates</strong> has been checked in the pre-value editor, holidays will be taken
        /// into account as well.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>Returns <code>true</code> if the entity is open during the day of the specified <code>date</code>,
        /// otherwise <code>false</code>.</returns>
        public bool IsOpen(DateTime date) {
            HolidayOpeningHours holiday;
            return _holidays.TryGetValue(date.ToString("yyyyMMdd"), out holiday) ? holiday.IsOpen : Weekdays[date.DayOfWeek].IsOpen;
        }

        /// <summary>
        /// Gets whether the day at the specified <code>date</code> is a holiday. Notice that this check will only work
        /// if <strong>Require Holiday Dates</strong> has been checked in the pre-value editor.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>Returns <code>true</code> if the day at the specified <code>date</code> is a holiday, otherwise
        /// <code>false</code>.</returns>
        public bool IsHoliday(DateTime date) {
            return _holidays.ContainsKey(date.ToString("yyyyMMdd"));
        }

        /// <summary>
        /// Gets a reference to the holiday at the specified <code>date</code>, or <code>null</code> if the day doesn't
        /// represent a holiday.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>Returns an instance of <see cref="HolidayOpeningHours"/> representing the holiday, or
        /// <code>null</code> if the day doesn't represent a holiday.</returns>
        public HolidayOpeningHours GetHoliday(DateTime date) {
            HolidayOpeningHours holiday;
            return _holidays.TryGetValue(date.ToString("yyyyMMdd"), out holiday) ? holiday : null;
        }

        /// <summary>
        /// Gets a reference to the holiday at the specified <code>date</code>, or <code>null</code> if the day doesn't
        /// represent a holiday.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="holiday">A reference to the holiday.</param>
        /// <returns>Returns <code>true</code> if the day represents a holiday, otherwise <code>false</code>.</returns>
        public bool TryGetHoliday(DateTime date, out HolidayOpeningHours holiday) {
            return _holidays.TryGetValue(date.ToString("yyyyMMdd"), out holiday);
        }

        /// <summary>
        /// Gets an instance of <see cref="OpeningHoursDay"/> representing the day at the specified <code>date</code>. If <strong>Require Holiday Dates</strong> has
        /// been checked in the pre-value editor, holidays will be taken into account as well.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>Returns an instance of <see cref="OpeningHoursDay"/> representing the day at the specified <code>date</code>.</returns>
        public OpeningHoursDay GetDay(DateTime date) {

            // Get information about the weekday (and whether it is a holiday)
            WeekdayOpeningHours woh = Weekdays[date.DayOfWeek];
            HolidayOpeningHours hoh = GetHoliday(date);

            // Initialize the instance for the day
            return new OpeningHoursDay {
                IsOpen = hoh == null ? woh.IsOpen : hoh.IsOpen,
                Date = date,
                Label = hoh == null ? null : hoh.Label,
                Opens = date.Add(hoh == null ? woh.Opens : hoh.Opens),
                Closes = date.Add(hoh == null ? woh.Closes : hoh.Closes)
            };

        }

        /// <summary>
        /// Gets an array of the next <code>count</code> upcoming days. If <strong>Require Holiday Dates</strong> has
        /// been checked in the pre-value editor, holidays will be incorporated into the result.
        /// </summary>
        /// <param name="count">The amount of days to be returned (including the current day).</param>
        /// <returns>Returns an array of <see cref="OpeningHoursDay"/> representing the opening hours of the upcoming days.</returns>
        public OpeningHoursDay[] GetUpcomingDays(int count) {

            // Array containing the days
            OpeningHoursDay[] upcomingDays = new OpeningHoursDay[count];

            // Iterate through the days one by one
            for (int i = 0; i < count; i++) {
                upcomingDays[i] = GetDay(DateTime.Today.AddDays(i));
            }

            return upcomingDays;

        }

        #endregion

    }

}