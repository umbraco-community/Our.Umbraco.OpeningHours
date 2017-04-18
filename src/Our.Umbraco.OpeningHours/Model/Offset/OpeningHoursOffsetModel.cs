using System;
using Our.Umbraco.OpeningHours.Model.Items;

namespace Our.Umbraco.OpeningHours.Model.Offset {
    
    /// <summary>
    /// Class representing the model of the <strong>Opening Hours</strong> property editor.
    /// </summary>
    public class OpeningHoursOffsetModel {

        #region Properties

        /// <summary>
        /// Gets a reference to the time zone.
        /// </summary>
        public TimeZoneInfo TimeZone { get; private set; }

        /// <summary>
        /// Gets a reference to the underlying <see cref="OpeningHoursModel"/>.
        /// </summary>
        public OpeningHoursModel Model { get; private set; }
        
        /// <summary>
        /// Gets whether the entity (store, company or similar) is currently open.
        /// </summary>
        public bool IsCurrentlyOpen {
            get { return GetDay(DateTime.Now).IsCurrentlyOpen; }
        }

        /// <summary>
        /// Gets whether the entity (store, company or similar) is currently closed.
        /// </summary>
        public bool IsCurrentlyClosed {
            get { return GetDay(DateTime.Now).IsCurrentlyClosed; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance from the specified <paramref name="model"/>.
        /// </summary>
        /// <param name="model">The instance of <see cref="OpeningHoursModel"/> representing the model.</param>
        /// <param name="timeZone">The time zone the new model should be based on.</param>
        public OpeningHoursOffsetModel(OpeningHoursModel model, TimeZoneInfo timeZone) {

            if (model == null) throw new ArgumentNullException("model");

            TimeZone = timeZone;
            Model = model;

        }

        #endregion

        #region Member methods
        
        /// <summary>
        /// Gets whether the day at the specified <paramref name="date"/> is a holiday. Notice that this check will
        /// only work if <strong>Require Holiday Dates</strong> has been checked in the pre-value editor.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>Returns <code>true</code> if the day at the specified <code>date</code> is a holiday, otherwise
        /// <code>false</code>.</returns>
        public bool IsHoliday(DateTimeOffset date) {
            return Model.IsHoliday(date);
        }

        /// <summary>
        /// Gets a reference to the holiday at the specified <paramref name="date"/>, or <code>null</code> if the day
        /// doesn't represent a holiday.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>An instance of <see cref="OpeningHoursHolidayItem"/> representing the holiday, or
        /// <code>null</code> if the day doesn't represent a holiday.</returns>
        public OpeningHoursHolidayItem GetHoliday(DateTimeOffset date) {
            return Model.GetHoliday(date);
        }

        /// <summary>
        /// Gets a reference to the holiday at the specified <paramref name="date"/>, or <code>null</code> if the day
        /// doesn't represent a holiday.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="holiday">A reference to the holiday.</param>
        /// <returns><code>true</code> if the day represents a holiday, otherwise <code>false</code>.</returns>
        public bool TryGetHoliday(DateTimeOffset date, out OpeningHoursHolidayItem holiday) {
            return Model.TryGetHoliday(date, out holiday);
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
            return Model.GetDay(date);
        }

        /// <summary>
        /// Gets an array of the next <paramref name="count"/> upcoming days. If <strong>Require Holiday Dates</strong>
        /// has been checked in the pre-value editor, holidays will be incorporated into the result.
        /// </summary>
        /// <param name="count">The amount of days to be returned (including the current day).</param>
        /// <returns>An array of <see cref="OpeningHoursDayOffset"/> representing the opening hours of the upcoming
        /// days.</returns>
        public OpeningHoursDayOffset[] GetUpcomingDays(int count) {
            return Model.GetUpcomingDays(count, TimeZone);
        }
        
        #endregion

    }

}