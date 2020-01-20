using System;
using System.Globalization;
using System.Linq;
using Our.Umbraco.OpeningHours.Extensions;
using Our.Umbraco.OpeningHours.Model.Items;
using Umbraco.Core;

namespace Our.Umbraco.OpeningHours.Model {

    /// <summary>
    /// Class representing the opening hours of a day at a specific date.
    /// </summary>
    public class OpeningHoursDay {

        #region Properties

        /// <summary>
        /// Gets a reference to the weekday this day is based on.
        /// </summary>
        public OpeningHoursWeekdayItem Weekday { get; private set; }

        /// <summary>
        /// Gets a reference to the holiday this day is based on, or <code>null</code> if not present.
        /// </summary>
        public OpeningHoursHolidayItem Holiday { get; private set; }

        /// <summary>
        /// Gets where the entity is open at least once during the day.
        /// </summary>
        public bool IsOpen { get; private set; }

        /// <summary>
        /// Gets whether the entity is closed during the entire the day.
        /// </summary>
        public bool IsClosed {
            get { return !IsOpen; }
        }

        /// <summary>
        /// Gets the date of the day.
        /// </summary>
        public DateTime Date { get; private set; }

        /// <summary>
        /// Gets the day of the week.
        /// </summary>
        public DayOfWeek DayOfWeek {
            get { return Date.DayOfWeek; }
        }

        /// <summary>
        /// Gets whether the day is today.
        /// </summary>
        public bool IsToday {
            get { return Date.IsToday(); }
        }

        /// <summary>
        /// Gets whether the day is tomorrow.
        /// </summary>
        public bool IsTomorrow {
            get { return Date.IsTomorrow(); }
        }

        /// <summary>
        /// Gets whether current time is today and within the opening hours.
        /// </summary>
        public bool IsCurrentlyOpen {
            get { return IsToday && Items.Any(x => x.Opens <= DateTime.Now && DateTime.Now <= x.Closes); }
        }

        /// <summary>
        /// Gets whether entity is currently closed (AKA different day or not within the opening hours of this day).
        /// </summary>
        public bool IsCurrentlyClosed {
            get { return !IsCurrentlyOpen; }
        }

        /// <summary>
        /// Gets the name of the weekday according to <see cref="CultureInfo.CurrentCulture"/>.
        /// </summary>
        public string WeekDayName {
            get { return Date.ToString("dddd", CultureInfo.CurrentCulture); }
        }

        /// <summary>
        /// Gets the name of the weekday according to <see cref="CultureInfo.CurrentCulture"/>. The first character of
        /// the name will always be uppercase.
        /// </summary>
        public string WeekDayNameFirstCharToUpper {
            get { return WeekDayName.ToFirstUpper(); }
        }

        /// <summary>
        /// Gets the label of this day. Use <see cref="HasLabel"/> to check whether the day has a label.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets whether this day has a label (AKA the <see cref="Label"/> has been specified).
        /// </summary>
        public bool HasLabel {
            get { return !String.IsNullOrEmpty(Label); }
        }

        /// <summary>
        /// Gets an array of the time slot of the day.
        /// </summary>
        public OpeningHoursDayTimeSlot[] Items { get; private set; }

        #endregion

        #region Constructors

        public OpeningHoursDay(DateTime date, OpeningHoursWeekdayItem weekday, OpeningHoursHolidayItem holiday) {
            Date = date;
            Weekday = weekday;
            Holiday = holiday;
            IsOpen = holiday == null ? weekday.IsOpen : holiday.IsOpen;
            Label = holiday == null ? weekday.Label : holiday.Label;
            Items = (holiday == null ? weekday.Items : holiday.Items).Select(x => new OpeningHoursDayTimeSlot(date, x)).ToArray();
        }

        #endregion

    }

}