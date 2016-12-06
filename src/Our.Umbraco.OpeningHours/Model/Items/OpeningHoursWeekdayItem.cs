using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Our.Umbraco.OpeningHours.Model.Json;
using Skybrud.Essentials.Json.Extensions;

namespace Our.Umbraco.OpeningHours.Model.Items {
    
    public class OpeningHoursWeekdayItem : OpeningHoursJsonObject {

        #region Properties

        /// <summary>
        /// Gets the label of the day.
        /// </summary>
        [JsonProperty("label", Order = 1)]
        public string Label { get; private set; }

        /// <summary>
        /// Gets an array of the time slots of the day.
        /// </summary>
        [JsonProperty("items", Order = 3)]
        public OpeningHoursTimeSlot[] Items { get; private set; }

        /// <summary>
        /// Gets where the entity has at least one open time slot throughout the day.
        /// </summary>
        [JsonIgnore]
        public bool IsOpen {
            get { return Items != null && Items.Length > 0; }
        }

        /// <summary>
        /// Gets whether the entity is closed throughout the entire day.
        /// </summary>
        [JsonIgnore]
        public bool IsClosed {
            get { return !IsOpen; }
        }

        /// <summary>
        /// Gets whether the entity is during multiple periods throughout the day.
        /// </summary>
        [JsonIgnore]
        public bool HasMultiple {
            get { return Items != null && Items.Length > 1; }
        }

        #endregion

        #region Constructors

        protected OpeningHoursWeekdayItem(JObject obj) : base(obj) {
            Label = obj.GetString("label") ?? "";
            Items = obj.GetArray("items", OpeningHoursTimeSlot.Parse);
            ParseLegacyValues();
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Parses legacy values from <code>beta1</code>.
        /// </summary>
        private void ParseLegacyValues() {
            if (Items != null) return;
            if (JObject.GetBoolean("isOpen")) {
                TimeSpan opens = JObject.GetString("opens", TimeSpan.Parse);
                TimeSpan closes = JObject.GetString("closes", TimeSpan.Parse);
                Items = new[] {
                    new OpeningHoursTimeSlot(opens, closes)
                };
            } else {
                Items = new OpeningHoursTimeSlot[0];
            }
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Initializes an empty instance for the specified <see cref="DayOfWeek"/>.
        /// </summary>
        /// <param name="day">The day of the week.</param>
        /// <returns>Returns an instance of <see cref="OpeningHoursWeekdayItem"/>.</returns>
        public static OpeningHoursWeekdayItem GetEmptyModel(DayOfWeek day) {
            return new OpeningHoursWeekdayItem(JObject.Parse("{label:\"" + day + "\",items:[]}"));
        }

        /// <summary>
        /// Gets an instance of <see cref="OpeningHoursTimeSlot"/> from the specified <see cref="JObject"/>.
        /// </summary>
        /// <param name="obj">The instance of <see cref="JObject"/> to parse.</param>
        public static OpeningHoursWeekdayItem Parse(JObject obj) {
            return obj == null ? null : new OpeningHoursWeekdayItem(obj);
        }

        #endregion

    }

}