using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Our.Umbraco.OpeningHours.Extensions.Json;
using Our.Umbraco.OpeningHours.Json;

namespace Our.Umbraco.OpeningHours.Model.Items {
    
    public class OpeningHoursHolidayItem : OpeningHoursWeekdayItem {

        #region Properties

        [JsonProperty("date", Order = 2)]
        [JsonConverter(typeof(OpeningHoursJsonConverter))]
        public DateTime Date { get; set; }

        [JsonIgnore]
        public bool HasDate {
            get { return Date != default(DateTime); }
        }

        #endregion

        #region Constructors

        protected OpeningHoursHolidayItem(JObject obj) : base(obj) {
            Date = obj.GetString("date", DateTime.Parse);
        }

        #endregion

        #region Static methods

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