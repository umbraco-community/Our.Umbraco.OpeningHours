using Newtonsoft.Json;

namespace Our.Umbraco.OpeningHours.Model
{
    public class FirstSundayInMonth : WeekDay
    {
        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
    }
}