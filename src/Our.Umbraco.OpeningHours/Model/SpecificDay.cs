using System;
using Newtonsoft.Json;

namespace Our.Umbraco.OpeningHours.Model
{
    public class SpecificDay : WeekDay
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("selectedDate")]
        public DateTime SelectedDate { get; set; }  
    }
}