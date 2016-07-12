using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Our.Umbraco.OpeningHours.Extensions.Json;
using Our.Umbraco.OpeningHours.Json;
using Our.Umbraco.OpeningHours.Model.Json;

namespace Our.Umbraco.OpeningHours.Model.Items {

    /// <summary>
    /// Class representing a time item (or time slot really) of a day.
    /// </summary>
    public class OpeningHoursTimeItem : OpeningHoursJsonObject {

        #region Properties

        /// <summary>
        /// Gets the opening time of this item.
        /// </summary>
        [JsonProperty("opens")]
        [JsonConverter(typeof(OpeningHoursJsonConverter))]
        public TimeSpan Opens { get; private set; }

        /// <summary>
        /// Gets the closing time of this item.
        /// </summary>
        [JsonProperty("closes")]
        [JsonConverter(typeof(OpeningHoursJsonConverter))]
        public TimeSpan Closes { get; private set; }

        #endregion

        #region Constructors

        private OpeningHoursTimeItem(JObject obj) : base(obj) {
            Opens = obj.GetString("opens", TimeSpan.Parse);
            Closes = obj.GetString("closes", TimeSpan.Parse);
        }

        internal OpeningHoursTimeItem(TimeSpan opens, TimeSpan closes) : base(null) {
            Opens = opens;
            Closes = closes;
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Gets an instance of <see cref="OpeningHoursTimeItem"/> from the specified <see cref="JObject"/>.
        /// </summary>
        /// <param name="obj">The instance of <see cref="JObject"/> to parse.</param>
        public static OpeningHoursTimeItem Parse(JObject obj) {
            return obj == null ? null : new OpeningHoursTimeItem(obj);
        }

        #endregion

    }

}