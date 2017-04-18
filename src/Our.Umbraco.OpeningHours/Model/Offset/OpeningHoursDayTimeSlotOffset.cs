using System;
using Our.Umbraco.OpeningHours.Model.Items;

namespace Our.Umbraco.OpeningHours.Model.Offset {

    /// <summary>
    /// Class representing a time slot of a day at a specific date.
    /// </summary>
    public class OpeningHoursDayTimeSlotOffset {

        #region Properties

        /// <summary>
        /// Gets the opening time of this item.
        /// </summary>
        public DateTimeOffset Opens { get; private set; }

        /// <summary>
        /// Gets the closing time of this item.
        /// </summary>
        public DateTimeOffset Closes { get; private set; }

        #endregion

        #region Constructors

        public OpeningHoursDayTimeSlotOffset(DateTimeOffset date, OpeningHoursTimeSlot time) {
            Opens = new DateTimeOffset(date.Year, date.Month, date.Day, time.Opens.Hours, time.Opens.Minutes, time.Opens.Seconds, date.Offset);
            Closes = new DateTimeOffset(date.Year, date.Month, date.Day, time.Closes.Hours, time.Closes.Minutes, time.Closes.Seconds, date.Offset);
        }

        #endregion

    }

}