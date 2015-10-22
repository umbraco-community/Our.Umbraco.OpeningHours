using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Our.Umbraco.OpeningHours.Model
{
    public class OpeningHours
    {
        [JsonProperty("weekdays")]
        public Dictionary<DayOfWeek, WeekdayOpeningHours> Weekdays { get; set; }

        [JsonProperty("holidays")]
        public List<HolidayOpeningHours> Holidays { get; set; }

        public OpeningHours()
        {
            this.Weekdays = new Dictionary<DayOfWeek, WeekdayOpeningHours>();

            for (var i = 0; i < 7; i++)
            {
                this.Weekdays.Add((DayOfWeek)i, new WeekdayOpeningHours());
            }

            this.Holidays = new List<HolidayOpeningHours>();
        }
    }
}