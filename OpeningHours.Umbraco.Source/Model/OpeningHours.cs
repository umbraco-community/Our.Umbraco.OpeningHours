using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using OpeningHours.Umbraco.Source.Extensions;

namespace OpeningHours.Umbraco.Source.Model
{
    public class OpeningHours
    {
        [JsonProperty("weekDays")]
        public List<WeekDay> WeekDays { get; set; }

        [JsonProperty("firstSundayInMonth")]
        public FirstSundayInMonth FirstSunDayInMonth { get; set; }

        [JsonProperty("lastSundayInMonth")]
        public LastSundayInMonth LastSundayInMonth { get; set; }

        [JsonProperty("specificDays")]
        public List<SpecificDay> SpecificDays { get; set; }


        public OpeningHours()
        {
            WeekDays = new List<WeekDay>();
            SpecificDays = new List<SpecificDay>();
            FirstSunDayInMonth = new FirstSundayInMonth();
            LastSundayInMonth = new LastSundayInMonth();
        }

        /// <summary>
        ///     Method for getting the general opening hours
        /// </summary>
        /// <param name="includeMinutes">bool (defaults to true)</param>
        /// <returns>Dictionary where key is a string of day(s) and value is a string of opening hours</returns>
        public Dictionary<string, string> GetGeneralOpeningHoursGrouped(bool includeMinutes = true)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            Dictionary<string, string> tmp = new Dictionary<string, string>();
            foreach (WeekDay day in WeekDays)
            {
                if (day.ClosedThisDay)
                {
                    if (!day.DayOfWeek.ToLower().Equals("sunday"))
                    {
                        tmp.Add(day.Label, "Closed");
                    }
                    else
                    {
                        if (!FirstSunDayInMonth.IsActive && !LastSundayInMonth.IsActive)
                        {
                            tmp.Add(day.Label, "Closed");
                        }
                    }
                }
                else
                {
                    if (includeMinutes)
                        tmp.Add(day.Label, string.Format("{0:hh\\:mm}-{1:hh\\:mm}", day.Open, day.Closed));
                    else
                        tmp.Add(day.Label, string.Format("{0:hh}-{1:hh}", day.Open, day.Closed));
                }
            }

            // Make the general opening hours compact, i.e.: "Mon. - Wed." if they have the same opening hours
            var compact = tmp.Compact(p => p.Value,
                           (key, values) => new
                           {
                               Hours = key,
                               Start = values.First().Key,
                               End = values.Last().Key
                           });

            foreach (var grp in compact)
            {
                if (!grp.Start.Equals(grp.End))
                    result.Add(grp.Start + " - " + grp.End, grp.Hours);
                else
                    result.Add(grp.Start, grp.Hours);
            }

            // First and last sunday of month
            if ((FirstSunDayInMonth.IsActive && LastSundayInMonth.IsActive))
            {
                if (FirstSunDayInMonth.ClosedThisDay && LastSundayInMonth.ClosedThisDay)
                    result.Add("First and last sunday in month", "Closed");
                else
                {
                    if (FirstSunDayInMonth.Open == LastSundayInMonth.Open && FirstSunDayInMonth.Closed == LastSundayInMonth.Closed)
                    {
                        if (includeMinutes)
                            result.Add("First and last sunday in month", string.Format("{0:hh\\:mm}-{1:hh\\:mm}", FirstSunDayInMonth.Open, FirstSunDayInMonth.Closed));
                        else
                            result.Add("First and last sunday in month", string.Format("{0:hh}-{1:hh}", FirstSunDayInMonth.Open, FirstSunDayInMonth.Closed));
                    }
                    else
                    {
                        result.Add("Første søndag i mdr.", string.Format("{0:hh\\:mm}-{1:hh\\:mm}", FirstSunDayInMonth.Open, FirstSunDayInMonth.Closed));
                        result.Add("Sidste søndag i mdr.", string.Format("{0:hh\\:mm}-{1:hh\\:mm}", LastSundayInMonth.Open, LastSundayInMonth.Closed));
                    }
                }

            }
            else
            {
                // First sunday of month
                if (FirstSunDayInMonth.IsActive)
                    if (FirstSunDayInMonth.ClosedThisDay)
                        result.Add("First sunday in month", "Closed");
                    else
                    {
                        if (includeMinutes)
                            result.Add("First sunday in month",
                                string.Format("{0:hh\\:mm}-{1:hh\\:mm}", FirstSunDayInMonth.Open, FirstSunDayInMonth.Closed));
                        else
                            result.Add("First sunday in month",
                                string.Format("{0:hh}-{1:hh}", FirstSunDayInMonth.Open, FirstSunDayInMonth.Closed));
                    }

                // Last sunday of month
                if (LastSundayInMonth.IsActive)
                {
                    if (LastSundayInMonth.ClosedThisDay)
                        result.Add("Last sunday in month", "Closed");
                    else
                    {
                        if (includeMinutes)
                            result.Add("Last sunday in month",
                                string.Format("{0:hh\\:mm}-{1:hh\\:mm}", LastSundayInMonth.Open, LastSundayInMonth.Closed));
                        else
                            result.Add("Last sunday in month",
                                string.Format("{0:hh}-{1:hh}", LastSundayInMonth.Open, LastSundayInMonth.Closed));
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///     Method for getting the opening hours of today as a string
        /// </summary>
        /// <param name="includeMinutes">bool (defaults to true)</param>
        /// <returns>string</returns>
        public string GetTodaysOpeningHours(bool includeMinutes = true)
        {
            DateTime now = DateTime.Now;

            // Get current day name
            string dayName = now.ToString("dddd");

            // First, check to make sure there's no specific opening hours today
            SpecificDay specificOpeningHoursToday = SpecificDays.FirstOrDefault(x => x.SelectedDate.Date == now.Date);

            if (specificOpeningHoursToday != null)
            {
                // Check if this specific opening hour is closed for the day
                if (specificOpeningHoursToday.ClosedThisDay)
                    return string.Empty;

                // If today is not closed, use the selected open and closing hours
                if (now.TimeOfDay >= specificOpeningHoursToday.Open && now.TimeOfDay <= specificOpeningHoursToday.Closed)
                {
                    if (includeMinutes)
                        return string.Format("{0:hh\\:mm}-{1:hh\\:mm}", specificOpeningHoursToday.Open, specificOpeningHoursToday.Closed);

                    return string.Format("{0:hh}-{1:hh}", specificOpeningHoursToday.Open, specificOpeningHoursToday.Closed);
                }
            }
            else
            {
                // First sunday of month check
                // Start by getting the first sunday of this month
                DateTime firstSundayOfMonth = now.GetFirstSundayOfMonth();

                // Check if today is the first sunday of the month
                if (firstSundayOfMonth.Date == now.Date && FirstSunDayInMonth.IsActive)
                {
                    if (FirstSunDayInMonth.ClosedThisDay)
                        return string.Empty;

                    if (now.TimeOfDay >= FirstSunDayInMonth.Open && now.TimeOfDay <= FirstSunDayInMonth.Closed)
                    {
                        if (includeMinutes)
                            return string.Format("{0:hh\\:mm}-{1:hh\\:mm}", FirstSunDayInMonth.Open, FirstSunDayInMonth.Closed);

                        return string.Format("{0:hh}-{1:hh}", FirstSunDayInMonth.Open, FirstSunDayInMonth.Closed);
                    }
                }

                // Last sunday of month check
                DateTime lastSundayInMonth = now.GetLastWeekDayOfMonth(DayOfWeek.Sunday);
                if (lastSundayInMonth.Date == now.Date && LastSundayInMonth.IsActive)
                {
                    if (LastSundayInMonth.ClosedThisDay)
                        return string.Empty;

                    if (now.TimeOfDay >= LastSundayInMonth.Open && now.TimeOfDay <= LastSundayInMonth.Closed)
                    {
                        if (includeMinutes)
                            return string.Format("{0:hh\\:mm}-{1:hh\\:mm}", LastSundayInMonth.Open, LastSundayInMonth.Closed);

                        return string.Format("{0:hh}-{1:hh}", LastSundayInMonth.Open, LastSundayInMonth.Closed);
                    }
                }

                // If no specific opening hour was found for todays date, use opening hours from the week days

                // Find opening hours for today
                var openingHour = WeekDays.FirstOrDefault(x => x.DayOfWeek.ToLower().Equals(dayName.ToLower()));

                if (openingHour != null)
                {
                    // Check if today is closed
                    if (openingHour.ClosedThisDay)
                        return string.Empty;

                    // If today is not closed, compare the open and closed times with current time
                    if (now.TimeOfDay >= openingHour.Open && now.TimeOfDay <= openingHour.Closed)
                    {
                        if (includeMinutes)
                            return string.Format("{0:hh\\:mm}-{1:hh\\:mm}", openingHour.Open, openingHour.Closed);

                        return string.Format("{0:hh}-{1:hh}", openingHour.Open, openingHour.Closed);
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        ///     Method for getting the opening hours for a shop when it's open next.
        ///     Should be called if a shop is currently closed.
        /// </summary>
        /// <returns>string</returns>
        public string GetOpeningHoursWhenNextOpen()
        {
            string resultFormat = "{0} at {1:hh\\:mm}";
            DateTime now = DateTime.Now;
            DateTime tomorrow = now.AddDays(1);

            // Check specific opening days
            SpecificDay day = SpecificDays.FirstOrDefault(x => x.SelectedDate.Date == tomorrow.Date);

            if (day != null)
            {
                if (!day.ClosedThisDay)
                    return string.Format(resultFormat, day.Label.ToLower(), day.Open);
            }

            // If today is saturday, we have to check if either first or last sunday in month is tomorrow
            if (now.DayOfWeek == DayOfWeek.Saturday)
            {
                // Check if tomorrow is first or last sunday in month
                DateTime firstSundayInMonth = now.GetFirstSundayOfMonth();
                DateTime lastSundayInMonth = now.GetLastWeekDayOfMonth(DayOfWeek.Sunday);

                if (tomorrow.Date == firstSundayInMonth.Date && FirstSunDayInMonth.IsActive)
                    return string.Format(resultFormat, FirstSunDayInMonth.Label.ToLower(), FirstSunDayInMonth.Open);

                if (tomorrow.Date == lastSundayInMonth.Date)
                    return string.Format(resultFormat, LastSundayInMonth.Label.ToLower(), LastSundayInMonth.Open);
            }
            else
            {
                // Get week day
                WeekDay currentWeekDay = WeekDays.FirstOrDefault(x => x.DayOfWeek == now.DayOfWeek.ToString());

                // Search forward in the array
                for (int i = WeekDays.IndexOf(currentWeekDay) + 1; i < WeekDays.Count() - 1; i++)
                {
                    WeekDay nextWeekDay = WeekDays[i];
                    if (!nextWeekDay.ClosedThisDay)
                        return string.Format(resultFormat, nextWeekDay.Label.ToLower(), nextWeekDay.Open);
                }

                // Search from the start of the week days array and to the current week day
                for (int i = 0; i < WeekDays.IndexOf(currentWeekDay); i++)
                {
                    WeekDay nextWeekDay = WeekDays[i];
                    if (!nextWeekDay.ClosedThisDay)
                        return string.Format(resultFormat, nextWeekDay.Label.ToLower(), nextWeekDay.Open);
                }
            }

            return string.Empty;
        }

        /// <summary>
        ///     Method for getting opening hours if a shop opens later today
        /// </summary>
        /// <returns>string</returns>
        public string GetLaterOpeningHoursForToday()
        {
            string resultFormat = "{0:hh\\:mm}";
            DateTime now = DateTime.Now;

            // Check if any specific opening hour is today
            SpecificDay today = SpecificDays.FirstOrDefault(x => x.SelectedDate.Date == now.Date);

            if (today != null)
            {
                if (!today.ClosedThisDay && today.Open > now.TimeOfDay)
                {
                    return string.Format(resultFormat, today.Open);
                }
            }
            else
            {
                // Check first/last sunday in month                
                DateTime firstSundayInMonth = now.GetFirstSundayOfMonth();
                DateTime lastSundayInMonth = now.GetLastWeekDayOfMonth(DayOfWeek.Sunday);

                if (now.Date == firstSundayInMonth.Date && FirstSunDayInMonth.IsActive && FirstSunDayInMonth.Open > now.TimeOfDay && !FirstSunDayInMonth.ClosedThisDay)
                    return string.Format(resultFormat, FirstSunDayInMonth.Open);

                if (now.Date == lastSundayInMonth.Date && LastSundayInMonth.IsActive && LastSundayInMonth.Open > now.TimeOfDay && !LastSundayInMonth.ClosedThisDay)
                    return string.Format(resultFormat, LastSundayInMonth.Open);
            }

            // Normal week days
            WeekDay weekDay = WeekDays.FirstOrDefault(x => x.DayOfWeek == now.DayOfWeek.ToString());

            if (weekDay != null)
            {
                if (weekDay.Open > now.TimeOfDay && !weekDay.ClosedThisDay)
                    return string.Format(resultFormat, weekDay.Open);
            }

            return string.Empty;
        }

        /// <summary>
        ///     Property for checking if a shop is currently open
        /// </summary>
        public bool IsOpen
        {
            get { return !string.IsNullOrEmpty(GetTodaysOpeningHours()); }
        }
    }
}