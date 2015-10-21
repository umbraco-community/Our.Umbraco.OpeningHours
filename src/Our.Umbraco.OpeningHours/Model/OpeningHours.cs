using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Our.Umbraco.OpeningHours.Model
{
    public class OpeningHours
    {
        [JsonProperty("weekdays")]
        public Dictionary<DayOfWeek, Timeframe> Weekdays { get; set; }

        [JsonProperty("holidays")]
        public List<Holiday> Holidays { get; set; }

        public OpeningHours()
        {
            this.Weekdays = new Dictionary<DayOfWeek, Timeframe>();
            this.Holidays = new List<Holiday>();
        }
    }
}