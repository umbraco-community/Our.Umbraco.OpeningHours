using Newtonsoft.Json.Linq;
using Our.Umbraco.OpeningHours.Json;

namespace Our.Umbraco.OpeningHours.Model.Json {
    
    public class OpeningHoursJsonObject : JsonObjectBase {

        #region Constructors

        protected OpeningHoursJsonObject(JObject obj) : base(obj) { }

        #endregion

    }

}