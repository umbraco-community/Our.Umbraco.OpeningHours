using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json;

namespace Our.Umbraco.OpeningHours.Model.Json {
    
    public class OpeningHoursJsonObject : JsonObjectBase {

        #region Constructors

        protected OpeningHoursJsonObject(JObject obj) : base(obj) { }

        #endregion

    }

}