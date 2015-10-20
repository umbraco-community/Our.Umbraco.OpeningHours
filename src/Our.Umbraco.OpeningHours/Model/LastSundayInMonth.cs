using Newtonsoft.Json;

namespace Our.Umbraco.OpeningHours.Model
{
    public class LastSundayInMonth : WeekDay
    {
        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
    }
}