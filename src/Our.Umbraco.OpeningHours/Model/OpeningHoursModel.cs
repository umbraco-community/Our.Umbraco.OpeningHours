using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Our.Umbraco.OpeningHours.Json;
using Our.Umbraco.OpeningHours.Model.Items;
using Our.Umbraco.OpeningHours.Model.Json;
using Our.Umbraco.OpeningHours.Model.Offset;
using Skybrud.Essentials.Json.Extensions;
using Umbraco.Core;

namespace Our.Umbraco.OpeningHours.Model {
    
    /// <summary>
    /// Class representing the model of the <strong>Opening Hours</strong> property editor.
    /// </summary>
    public class OpeningHoursModel : OpeningHoursJsonObject {

        private readonly Dictionary<string, OpeningHoursHolidayItem> _holidays;

        #region Properties

        /// <summary>
        /// Gets a dictionary of the standard weekdays as specified in the property editor.
        /// </summary>
        [JsonProperty("weekdays")]
        [JsonConverter(typeof(OpeningHoursJsonConverter))]
        public Dictionary<DayOfWeek, OpeningHoursWeekdayItem> Weekdays { get; private set; }

        /// <summary>
        /// Gets a list the holidays as specified in the property editor.
        /// </summary>
        [JsonProperty("holidays")]
        public OpeningHoursHolidayItem[] Holidays { get; private set; }

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
        public OpeningHoursModel() : base(null) {
            Weekdays = new Dictionary<DayOfWeek, OpeningHoursWeekdayItem>();
            for (int i = 0; i < 7; i++) {
                Weekdays.Add((DayOfWeek) i, OpeningHoursWeekdayItem.GetEmptyModel((DayOfWeek) i));
            }
            Holidays = new OpeningHoursHolidayItem[0];
            _holidays = new Dictionary<string, OpeningHoursHolidayItem>();
        }

        ///// <summary>
        ///// Initializes a new instance with the specified <code>weekdays</code> and <code>holidays</code>.
        ///// </summary>
        ///// <param name="weekdays">The weekdays that should make up the instance.</param>
        ///// <param name="holidays">The holidays that should make up the model.</param>
        //public OpeningHoursModel(Dictionary<DayOfWeek, OpeningHoursWeekdayItem> weekdays, OpeningHoursHolidayItem[] holidays) : base(null) {
        //    Weekdays = weekdays ?? new Dictionary<DayOfWeek, OpeningHoursWeekdayItem>();
        //    Holidays = holidays ?? new OpeningHoursHolidayItem[0];
        //    _holidays = Holidays.Where(x => x.Date != null).ToDictionary(x => x.Date.ToString("yyyyMMdd"));
        //}

        /// <summary>
        /// Initializes a new instance from the specified <see cref="JObject"/>.
        /// </summary>
        /// <param name="obj">The instance of <see cref="JObject"/> representing the model.</param>
        private OpeningHoursModel(JObject obj) : base(obj) {

            // Parse the weekdays
            Weekdays = new Dictionary<DayOfWeek, OpeningHoursWeekdayItem>();
            for (int i = 0; i < 7; i++) {
                DayOfWeek dayOfWeek = (DayOfWeek) i;
                Weekdays.Add(dayOfWeek, obj.GetObject("weekdays." + i, x => OpeningHoursWeekdayItem.Parse(x, dayOfWeek)));
            }

            // Parse holidays
            Holidays = obj.GetArrayItems("holidays", OpeningHoursHolidayItem.Parse);

            // Create a dictionary with the holidays - for O(1) lookups
            _holidays = Holidays.Where(x => x.IsValid).DistinctBy(x => x.Date.ToString("yyyyMMdd")).ToDictionary(x => x.Date.ToString("yyyyMMdd"));

        }

        #endregion

        #region Member methods

        ///// <summary>
        ///// Gets whether the entity is open at the specified <code>dayOfWeek</code>. Notice that since this is just the
        ///// weekday, holidays wion't be taken into account.
        ///// </summary>
        ///// <param name="dayOfWeek">The day of the week.</param>
        ///// <returns>Returns <code>true</code> if the entity is open at the specified <code>dayOfWeek</code>, otherwise
        ///// <code>false</code>.</returns>
        //public bool IsOpen(DayOfWeek dayOfWeek) {
        //    return Weekdays[dayOfWeek].IsOpen;
        //}

        ///// <summary>
        ///// Gets whether the entity is open during the day of the specified <code>date</code>. If
        ///// <strong>Require Holiday Dates</strong> has been checked in the pre-value editor, holidays will be taken
        ///// into account as well.
        ///// </summary>
        ///// <param name="date">The date.</param>
        ///// <returns>Returns <code>true</code> if the entity is open during the day of the specified <code>date</code>,
        ///// otherwise <code>false</code>.</returns>
        //public bool IsOpen(DateTime date) {
        //    HolidayOpeningHours holiday;
        //    return _holidays.TryGetValue(date.ToString("yyyyMMdd"), out holiday) ? holiday.IsOpen : Weekdays[date.DayOfWeek].IsOpen;
        //}

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
        /// Gets whether the day at the specified <paramref name="date"/> is a holiday. Notice that this check will
        /// only work if <strong>Require Holiday Dates</strong> has been checked in the pre-value editor.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>Returns <code>true</code> if the day at the specified <code>date</code> is a holiday, otherwise
        /// <code>false</code>.</returns>
        public bool IsHoliday(DateTimeOffset date) {
            return _holidays.ContainsKey(date.ToString("yyyyMMdd"));
        }

        /// <summary>
        /// Gets a reference to the holiday at the specified <code>date</code>, or <code>null</code> if the day doesn't
        /// represent a holiday.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>Returns an instance of <see cref="OpeningHoursHolidayItem"/> representing the holiday, or
        /// <code>null</code> if the day doesn't represent a holiday.</returns>
        public OpeningHoursHolidayItem GetHoliday(DateTime date) {
            OpeningHoursHolidayItem holiday;
            return _holidays.TryGetValue(date.ToString("yyyyMMdd"), out holiday) ? holiday : null;
        }

        /// <summary>
        /// Gets a reference to the holiday at the specified <code>date</code>, or <code>null</code> if the day doesn't
        /// represent a holiday.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="holiday">A reference to the holiday.</param>
        /// <returns>Returns <code>true</code> if the day represents a holiday, otherwise <code>false</code>.</returns>
        public bool TryGetHoliday(DateTime date, out OpeningHoursHolidayItem holiday) {
            return _holidays.TryGetValue(date.ToString("yyyyMMdd"), out holiday);
        }

        /// <summary>
        /// Gets a reference to the holiday at the specified <paramref name="date"/>, or <code>null</code> if the day
        /// doesn't represent a holiday.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>An instance of <see cref="OpeningHoursHolidayItem"/> representing the holiday, or
        /// <code>null</code> if the day doesn't represent a holiday.</returns>
        public OpeningHoursHolidayItem GetHoliday(DateTimeOffset date) {
            OpeningHoursHolidayItem holiday;
            return _holidays.TryGetValue(date.ToString("yyyyMMdd"), out holiday) ? holiday : null;
        }

        /// <summary>
        /// Gets a reference to the holiday at the specified <paramref name="date"/>, or <code>null</code> if the day
        /// doesn't represent a holiday.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="holiday">A reference to the holiday.</param>
        /// <returns><code>true</code> if the day represents a holiday, otherwise <code>false</code>.</returns>
        public bool TryGetHoliday(DateTimeOffset date, out OpeningHoursHolidayItem holiday) {
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
            OpeningHoursWeekdayItem woh = Weekdays[date.DayOfWeek];
            OpeningHoursHolidayItem hoh = GetHoliday(date);

            // Initialize the instance for the day
            return new OpeningHoursDay(date, woh, hoh);

        }

        /// <summary>
        /// Gets an instance of <see cref="OpeningHoursDayOffset"/> representing the day at the specified
        /// <paramref name="date"/>. If <strong>Require Holiday Dates</strong> has been checked in the pre-value
        /// editor, holidays will be taken into account as well.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>An instance of <see cref="OpeningHoursDayOffset"/> representing the day at the specified
        /// <paramref name="date"/>.</returns>
        public OpeningHoursDayOffset GetDay(DateTimeOffset date) {

            // Get information about the weekday (and whether it is a holiday)
            OpeningHoursWeekdayItem woh = Weekdays[date.DayOfWeek];
            OpeningHoursHolidayItem hoh = GetHoliday(date);

            // Initialize the instance for the day
            return new OpeningHoursDayOffset(date, woh, hoh);

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

        /// <summary>
        /// Gets an array of the next <paramref name="count"/> upcoming days. If <strong>Require Holiday Dates</strong>
        /// has been checked in the pre-value editor, holidays will be incorporated into the result.
        /// </summary>
        /// <param name="count">The amount of days to be returned (including the current day).</param>
        /// <param name="timeZone">The <see cref="TimeZoneInfo"/> that should be used.</param>
        /// <returns>An array of <see cref="OpeningHoursDay"/> representing the opening hours of the upcoming days.</returns>
        public OpeningHoursDayOffset[] GetUpcomingDays(int count, TimeZoneInfo timeZone) {

            // Array containing the days
            OpeningHoursDayOffset[] upcomingDays = new OpeningHoursDayOffset[count];

            // Iterate through the days one by one
            for (int i = 0; i < 14; i++) {

                // Get the current timestamp (according to the specified time zone)
                DateTimeOffset timeZoneNow = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, timeZone);

                // Get the timestamp for the day
                DateTimeOffset dt = timeZoneNow.AddDays(i);

                OpeningHoursWeekdayItem weekday = Weekdays[dt.DayOfWeek];
                OpeningHoursHolidayItem holiday = GetHoliday(dt);

                upcomingDays[i] = new OpeningHoursDayOffset(dt, weekday, holiday);

            }

            // Return the array
            return upcomingDays;

        }

        #endregion

        #region Static methods

        public static OpeningHoursModel Deserialize(string str) {

            // Return an empty model if the JSON string is empty
            if (String.IsNullOrWhiteSpace(str)) return new OpeningHoursModel();
            
            // Parse the JSON and initialize the model through the correct constructor
            return new OpeningHoursModel(JsonConvert.DeserializeObject<JObject>(str));

        }

        /// <summary>
        /// Gets an instance of <see cref="OpeningHoursModel"/> from the specified <see cref="JObject"/>.
        /// </summary>
        /// <param name="obj">The instance of <see cref="JObject"/> to parse.</param>
        public static OpeningHoursModel Parse(JObject obj) {
            return obj == null ? null : new OpeningHoursModel(obj);
        }

        #endregion

    }

}