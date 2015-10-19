using Newtonsoft.Json;

namespace OpeningHours.Umbraco.Source.Model
{
    public class LastSundayInMonth : WeekDay
    {
        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
    }
}