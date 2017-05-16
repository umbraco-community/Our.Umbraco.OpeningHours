using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Our.Umbraco.OpeningHours.Json;
using Skybrud.Essentials.Json.Extensions;

namespace Our.Umbraco.OpeningHours.Model.Items {
    
    public class OpeningHoursHolidayItem : OpeningHoursWeekdayItem {

        #region Properties

        /// <summary>
        /// Gets the date specified for this holiday item. If a date hasn't been specified,
        /// <see cref="DateTime.MinValue"/> will be returned instead.
        /// </summary>
        [JsonProperty("date", Order = 2)]
        [JsonConverter(typeof(OpeningHoursJsonConverter))]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets whether a date has been specified for this holiday item.
        /// </summary>
        [JsonIgnore]
        public bool HasDate {
            get { return Date != default(DateTime); }
        }

        /// <summary>
        /// Gets whether the holiday item is valid. Currently this property is just an alias of <see cref="HasDate"/>.
        /// </summary>
        [JsonIgnore]
        public bool IsValid {
            get { return HasDate; }
        }

        #endregion

        #region Constructors

        protected OpeningHoursHolidayItem(JObject obj) : base(obj, default(DayOfWeek)) {
            Date = obj.GetString("date", ParseDate);
            DayOfWeek = Date.DayOfWeek;
        }

        #endregion

        #region Static methods

        private DateTime ParseDate(string str) {
            DateTime date;
            return DateTime.TryParse(str ?? "", out date) ? date : default(DateTime);
        }

        /// <summary>
        /// Initializes an empty instance.
        /// </summary>
        /// <returns>Returns an instance of <see cref="OpeningHoursHolidayItem"/>.</returns>
        public static OpeningHoursHolidayItem GetEmptyModel() {
            return new OpeningHoursHolidayItem(JObject.Parse("{label:null,date:null,items:[]}"));
        }

        /// <summary>
        /// Gets an instance of <see cref="OpeningHoursHolidayItem"/> from the specified <see cref="JObject"/>.
        /// </summary>
        /// <param name="obj">The instance of <see cref="JObject"/> to parse.</param>
        public static new OpeningHoursHolidayItem Parse(JObject obj) {
            return obj == null ? null : new OpeningHoursHolidayItem(obj);
        }

        #endregion

    }

}