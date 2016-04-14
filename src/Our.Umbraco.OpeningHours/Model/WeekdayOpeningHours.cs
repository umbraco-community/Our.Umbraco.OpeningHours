using System;
using Newtonsoft.Json;

namespace Our.Umbraco.OpeningHours.Model
{
    public class WeekdayOpeningHours
    {
        [JsonProperty("opens")]
        public TimeSpan Opens { get; set; }

        [JsonProperty("closes")]
        public TimeSpan Closes { get; set; }

        [JsonProperty("isOpen")]
        public bool IsOpen { get; set; }
    }
}