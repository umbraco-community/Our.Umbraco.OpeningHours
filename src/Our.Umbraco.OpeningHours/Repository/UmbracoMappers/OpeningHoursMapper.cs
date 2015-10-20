using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Our.Umbraco.OpeningHours.Model;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Our.Umbraco.OpeningHours.Repository.UmbracoMappers
{
    public static class OpeningHoursMapper
    {
        /// <summary>
        /// Maps opening hours property JSON to a strongnly typed object
        /// </summary>
        /// <param name="content">IPublishedContent</param>
        /// <param name="openingHoursPropertyAlias">string</param>
        /// <returns>OpeningHours. Returns null if property JSON couldn't be deserialized.</returns>        
        public static Our.Umbraco.OpeningHours.Model.OpeningHours MapOpeningHours(IPublishedContent content, string openingHoursPropertyAlias)
        {
            // TODO: needs to be refactored so we're able to simply do: myContent.GetPropertyValue<OpeningHours>("openingHoursPropertyAlias")

            // Check if this node has a property with the given alias and if it has a value
            if (!content.HasProperty(openingHoursPropertyAlias) || !content.HasValue(openingHoursPropertyAlias))
                return new Our.Umbraco.OpeningHours.Model.OpeningHours();

            try
            {
                // Get opening hours as dynamic object
                dynamic json = JsonConvert.DeserializeObject(content.GetPropertyValue<string>(openingHoursPropertyAlias));

                var openingHours = new Our.Umbraco.OpeningHours.Model.OpeningHours();

                // Week days
                if (json.weekDays != null)
                {
                    JArray weekDays = json.weekDays;

                    foreach (dynamic day in weekDays)
                    {
                        WeekDay weekDay = new WeekDay()
                        {
                            Label = day.Label,
                            DayOfWeek = day.DayOfWeek
                        };

                        if (day.Open != null)
                        {
                            string openVal = day.Open.value;
                            DateTime open;
                            if (DateTime.TryParse(openVal, out open))
                                weekDay.Open = new TimeSpan(open.Hour, open.Minute, 0);
                        }

                        if (day.Closed != null)
                        {
                            string closedVal = day.Closed.value;
                            DateTime closed;
                            if (DateTime.TryParse(closedVal, out closed))
                                weekDay.Closed = new TimeSpan(closed.Hour, closed.Minute, 0);
                        }

                        weekDay.ClosedThisDay = day.ClosedThisDay;

                        openingHours.WeekDays.Add(weekDay);
                    }
                }

                // Last sunday in month
                if (json.lastSundayInMonth != null)
                {
                    openingHours.LastSundayInMonth.IsActive = json.lastSundayInMonth.IsActive;

                    if (json.lastSundayInMonth.Open != null)
                    {
                        string openVal = json.lastSundayInMonth.Open.value;
                        DateTime open;
                        if (DateTime.TryParse(openVal, out open))
                            openingHours.LastSundayInMonth.Open = new TimeSpan(open.Hour, open.Minute, 0);
                    }

                    if (json.lastSundayInMonth.Closed != null)
                    {
                        string closedVal = json.lastSundayInMonth.Closed.value;
                        DateTime closed;
                        if (DateTime.TryParse(closedVal, out closed))
                            openingHours.LastSundayInMonth.Closed = new TimeSpan(closed.Hour, closed.Minute, 0);
                    }
                }

                // First sunday in month
                if (json.firstSundayInMonth != null)
                {
                    openingHours.FirstSunDayInMonth.IsActive = json.firstSundayInMonth.IsActive;

                    if (json.firstSundayInMonth.Open != null)
                    {
                        string openVal = json.firstSundayInMonth.Open.value;
                        DateTime open;
                        if (DateTime.TryParse(openVal, out open))
                            openingHours.FirstSunDayInMonth.Open = new TimeSpan(open.Hour, open.Minute, 0);
                    }

                    if (json.firstSundayInMonth.Closed != null)
                    {
                        string closedVal = json.firstSundayInMonth.Closed.value;
                        DateTime closed;
                        if (DateTime.TryParse(closedVal, out closed))
                            openingHours.FirstSunDayInMonth.Closed = new TimeSpan(closed.Hour, closed.Minute, 0);
                    }
                }

                // Specific days
                if (json.specificDays != null)
                {
                    JArray specificDays = json.specificDays;

                    foreach (dynamic day in specificDays)
                    {
                        SpecificDay specificDay = new SpecificDay();

                        specificDay.Text = day.Text;

                        if (day.SelectedDate != null)
                        {
                            string selectedDateVal = day.SelectedDate.value;
                            DateTime selectedDate;
                            if (DateTime.TryParse(selectedDateVal, out selectedDate))
                            {
                                specificDay.SelectedDate = selectedDate;

                                if (day.Open != null)
                                {
                                    string openVal = day.Open.value;
                                    DateTime open;
                                    if (DateTime.TryParse(openVal, out open))
                                        specificDay.Open = new TimeSpan(open.Hour, open.Minute, 0);
                                }

                                if (day.Closed != null)
                                {
                                    string closedVal = day.Closed.value;
                                    DateTime closed;
                                    if (DateTime.TryParse(closedVal, out closed))
                                        specificDay.Closed = new TimeSpan(closed.Hour, closed.Minute, 0);
                                }

                                specificDay.ClosedThisDay = day.ClosedThisDay;

                                openingHours.SpecificDays.Add(specificDay);
                            }
                        }
                    }
                }

                return openingHours;
            }
            catch
            {
                return null;
            }
        }
    }
}