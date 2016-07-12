using System;

namespace Our.Umbraco.OpeningHours.Model.Items {
    
    public class OpeningHoursDayTimeItem {

        #region Properties

        /// <summary>
        /// Gets the opening time of this item.
        /// </summary>
        public DateTime Opens { get; private set; }

        /// <summary>
        /// Gets the closing time of this item.
        /// </summary>
        public DateTime Closes { get; private set; }

        #endregion

        #region Constructors

        public OpeningHoursDayTimeItem(DateTime date, OpeningHoursTimeItem time) {
            Opens = new DateTime(date.Year, date.Month, date.Day, time.Opens.Hours, time.Opens.Minutes, time.Opens.Seconds);
            Closes = new DateTime(date.Year, date.Month, date.Day, time.Closes.Hours, time.Closes.Minutes, time.Closes.Seconds);
        }

        #endregion

    }

}