using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Our.Umbraco.OpeningHours.Model.Items;

namespace Our.Umbraco.OpeningHours.Json {

    public class OpeningHoursJsonConverter : JsonConverter {

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            
            Dictionary<DayOfWeek, OpeningHoursWeekdayItem> obj = value as Dictionary<DayOfWeek, OpeningHoursWeekdayItem>;
            
            if (obj != null) {
                serializer.Serialize(writer, obj.ToDictionary(pair => (int) pair.Key + "", pair => pair.Value));
                return;
            }

            if (value is DateTime) {
                serializer.Serialize(writer, ((DateTime) value).ToString("yyyy-MM-dd"));
                return;
            }

            if (value is TimeSpan) {
                serializer.Serialize(writer, ((TimeSpan) value).ToString().Substring(0, 5));
                return;
            }
            
            serializer.Serialize(writer, value);
        
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
        }

        public override bool CanRead {
            get { return false; }
        }

        public override bool CanConvert(Type type) {
            return false;
        }
    
    }

}