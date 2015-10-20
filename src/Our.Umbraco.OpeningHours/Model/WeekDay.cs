using System;
using Newtonsoft.Json;

namespace Our.Umbraco.OpeningHours.Model
{
    public class WeekDay
    {
        [JsonProperty("dayOfWeek")]
        public string DayOfWeek { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("open")]
        public TimeSpan Open { get; set; }

        [JsonProperty("closed")]
        public TimeSpan Closed { get; set; }

        [JsonProperty("closedThisDay")]
        public bool ClosedThisDay { get; set; }
    }
}