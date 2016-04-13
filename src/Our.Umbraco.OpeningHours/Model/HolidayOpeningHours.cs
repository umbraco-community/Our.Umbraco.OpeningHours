using System;
using Newtonsoft.Json;

namespace Our.Umbraco.OpeningHours.Model
{
    public class HolidayOpeningHours : WeekdayOpeningHours
    {
        [JsonProperty("date")]
        public DateTime? Date { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; } 
    }
}