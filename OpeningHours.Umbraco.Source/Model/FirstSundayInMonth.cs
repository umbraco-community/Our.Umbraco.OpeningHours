using Newtonsoft.Json;

namespace OpeningHours.Umbraco.Source.Model
{
    public class FirstSundayInMonth : WeekDay
    {
        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
    }
}