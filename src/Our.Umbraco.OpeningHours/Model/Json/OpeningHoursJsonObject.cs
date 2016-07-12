using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Our.Umbraco.OpeningHours.Model.Json {
    
    public class OpeningHoursJsonObject {

        #region Properties

        /// <summary>
        /// Gets a reference to the <see cref="JObject"/> the object was parsed from.
        /// </summary>
        [JsonIgnore]
        public JObject JObject { get; private set; }

        #endregion

        #region Constructors

        protected OpeningHoursJsonObject(JObject obj) {
            JObject = obj;
        }

        #endregion

    }

}