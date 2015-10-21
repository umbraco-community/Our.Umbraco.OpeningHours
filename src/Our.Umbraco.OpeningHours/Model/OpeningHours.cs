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

        /// <summary>
        ///     Method for getting the general opening hours
        /// </summary>
        /// <param name="includeMinutes">bool (defaults to true)</param>
        /// <returns>Dictionary where key is a string of day(s) and value is a string of opening hours</returns>
        //public Dictionary<string, string> GetGeneralOpeningHoursGrouped(bool includeMinutes = true)
        //{
        //    Dictionary<string, string> result = new Dictionary<string, string>();
        //    Dictionary<string, string> tmp = new Dictionary<string, string>();

        //    string timeFormat = includeMinutes ? "{0:hh\\:mm}-{1:hh\\:mm}" : "{0:hh}-{1:hh}";

        //    foreach (Workday day in this.WeekDays)
        //    {
        //        if (day.ClosedThisDay)
        //        {
        //            if (!day.DayOfWeek.ToLower().Equals("sunday"))
        //            {
        //                tmp.Add(day.Label, "Closed");
        //            }
        //            else
        //            {
        //                if (!this.FirstSunDayInMonth.IsActive && !this.LastSundayInMonth.IsActive)
        //                {
        //                    tmp.Add(day.Label, "Closed");
        //                }
        //            }
        //        }
        //        else
        //        {
        //            tmp.Add(day.Label, string.Format(timeFormat, day.Open, day.Closed));
        //        }
        //    }

        //    // Make the general opening hours compact, i.e.: "Mon. - Wed." if they have the same opening hours
        //    var compact = tmp.Compact(p => p.Value,
        //                   (key, values) => new
        //                   {
        //                       Hours = key,
        //                       Start = values.First().Key,
        //                       End = values.Last().Key
        //                   });

        //    foreach (var grp in compact)
        //    {
        //        if (!grp.Start.Equals(grp.End))
        //            result.Add(grp.Start + " - " + grp.End, grp.Hours);
        //        else
        //            result.Add(grp.Start, grp.Hours);
        //    }

        //    // First and last sunday of month
        //    if ((this.FirstSunDayInMonth.IsActive && this.LastSundayInMonth.IsActive))
        //    {
        //        if (this.FirstSunDayInMonth.ClosedThisDay && this.LastSundayInMonth.ClosedThisDay)
        //            result.Add("First and last sunday in month", "Closed");
        //        else
        //        {
        //            if (this.FirstSunDayInMonth.Open == this.LastSundayInMonth.Open && this.FirstSunDayInMonth.Closed == this.LastSundayInMonth.Closed)
        //            {
        //                result.Add("First and last sunday in month", string.Format(timeFormat, this.FirstSunDayInMonth.Open, this.FirstSunDayInMonth.Closed));

        //            }
        //            else
        //            {
        //                result.Add("First sunday in month", string.Format(timeFormat, this.FirstSunDayInMonth.Open, this.FirstSunDayInMonth.Closed));
        //                result.Add("Last sunday in month", string.Format(timeFormat, this.LastSundayInMonth.Open, this.LastSundayInMonth.Closed));
        //            }
        //        }

        //    }
        //    else
        //    {
        //        // First sunday of month
        //        if (this.FirstSunDayInMonth.IsActive)
        //            if (this.FirstSunDayInMonth.ClosedThisDay)
        //                result.Add("First sunday in month", "Closed");
        //            else
        //            {
        //                result.Add("First sunday in month", string.Format(timeFormat, this.FirstSunDayInMonth.Open, this.FirstSunDayInMonth.Closed));
        //            }

        //        // Last sunday of month
        //        if (this.LastSundayInMonth.IsActive)
        //        {
        //            if (this.LastSundayInMonth.ClosedThisDay)
        //                result.Add("Last sunday in month", "Closed");
        //            else
        //            {
        //                result.Add("Last sunday in month", string.Format(timeFormat, this.LastSundayInMonth.Open, this.LastSundayInMonth.Closed));
        //            }
        //        }
        //    }

        //    return result;
        //}

        /// <summary>
        ///     Method for getting the opening hours of today as a string
        /// </summary>
        /// <param name="includeMinutes">bool (defaults to true)</param>
        /// <returns>string</returns>
        //public string GetTodaysOpeningHours(bool includeMinutes = true)
        //{
        //    DateTime now = DateTime.Now;

        //    string timeFormat = includeMinutes ? "{0:hh\\:mm}-{1:hh\\:mm}" : "{0:hh}-{1:hh}";

        //    // Get current day name
        //    string dayName = now.ToString("dddd");

        //    // First, check to make sure there's no specific opening hours today
        //    Holiday specificOpeningHoursToday = this.SpecificDays.FirstOrDefault(x => x.SelectedDate.Date == now.Date);

        //    if (specificOpeningHoursToday != null)
        //    {
        //        // Check if this specific opening hour is closed for the day
        //        if (specificOpeningHoursToday.ClosedThisDay)
        //            return string.Empty;

        //        // If today is not closed, use the selected open and closing hours
        //        if (now.TimeOfDay >= specificOpeningHoursToday.Open && now.TimeOfDay <= specificOpeningHoursToday.Closed)
        //        {
        //            return string.Format(timeFormat, specificOpeningHoursToday.Open, specificOpeningHoursToday.Closed);
        //        }
        //    }
        //    else
        //    {
        //        // First sunday of month check
        //        // Start by getting the first sunday of this month
        //        DateTime firstSundayOfMonth = now.GetFirstSundayOfMonth();

        //        // Check if today is the first sunday of the month
        //        if (firstSundayOfMonth.Date == now.Date && this.FirstSunDayInMonth.IsActive)
        //        {
        //            if (this.FirstSunDayInMonth.ClosedThisDay)
        //                return string.Empty;

        //            if (now.TimeOfDay >= this.FirstSunDayInMonth.Open && now.TimeOfDay <= this.FirstSunDayInMonth.Closed)
        //            {
        //                return string.Format(timeFormat, this.FirstSunDayInMonth.Open, this.FirstSunDayInMonth.Closed);
        //            }
        //        }

        //        // Last sunday of month check
        //        DateTime lastSundayInMonth = now.GetLastWeekDayOfMonth(DayOfWeek.Sunday);
        //        if (lastSundayInMonth.Date == now.Date && this.LastSundayInMonth.IsActive)
        //        {
        //            if (this.LastSundayInMonth.ClosedThisDay)
        //                return string.Empty;

        //            if (now.TimeOfDay >= this.LastSundayInMonth.Open && now.TimeOfDay <= this.LastSundayInMonth.Closed)
        //            {
        //                return string.Format(timeFormat, this.LastSundayInMonth.Open, this.LastSundayInMonth.Closed);
        //            }
        //        }

        //        // If no specific opening hour was found for todays date, use opening hours from the week days

        //        // Find opening hours for today
        //        var openingHour = this.WeekDays.FirstOrDefault(x => x.DayOfWeek.ToLower().Equals(dayName.ToLower()));

        //        if (openingHour != null)
        //        {
        //            // Check if today is closed
        //            if (openingHour.ClosedThisDay)
        //                return string.Empty;

        //            // If today is not closed, compare the open and closed times with current time
        //            if (now.TimeOfDay >= openingHour.Open && now.TimeOfDay <= openingHour.Closed)
        //            {
        //                return string.Format(timeFormat, openingHour.Open, openingHour.Closed);
        //            }
        //        }
        //    }

        //    return string.Empty;
        //}

        /// <summary>
        ///     Method for getting the opening hours for a shop when it's open next.
        ///     Should be called if a shop is currently closed.
        /// </summary>
        /// <returns>string</returns>
        //public string GetOpeningHoursWhenNextOpen()
        //{
        //    string resultFormat = "{0} at {1:hh\\:mm}";
        //    DateTime now = DateTime.Now;
        //    DateTime tomorrow = now.AddDays(1);

        //    // Check specific opening days
        //    Holiday day = this.SpecificDays.FirstOrDefault(x => x.SelectedDate.Date == tomorrow.Date);

        //    if (day != null)
        //    {
        //        if (!day.ClosedThisDay)
        //            return string.Format(resultFormat, day.Label.ToLower(), day.Open);
        //    }

        //    // If today is saturday, we have to check if either first or last sunday in month is tomorrow
        //    if (now.DayOfWeek == DayOfWeek.Saturday)
        //    {
        //        // Check if tomorrow is first or last sunday in month
        //        DateTime firstSundayInMonth = now.GetFirstSundayOfMonth();
        //        DateTime lastSundayInMonth = now.GetLastWeekDayOfMonth(DayOfWeek.Sunday);

        //        if (tomorrow.Date == firstSundayInMonth.Date && this.FirstSunDayInMonth.IsActive)
        //            return string.Format(resultFormat, this.FirstSunDayInMonth.Label.ToLower(), this.FirstSunDayInMonth.Open);

        //        if (tomorrow.Date == lastSundayInMonth.Date)
        //            return string.Format(resultFormat, this.LastSundayInMonth.Label.ToLower(), this.LastSundayInMonth.Open);
        //    }
        //    else
        //    {
        //        // Get week day
        //        Workday currentWeekDay = this.WeekDays.FirstOrDefault(x => x.DayOfWeek == now.DayOfWeek.ToString());

        //        // Search forward in the array
        //        for (int i = this.WeekDays.IndexOf(currentWeekDay) + 1; i < this.WeekDays.Count() - 1; i++)
        //        {
        //            Workday nextWeekDay = this.WeekDays[i];
        //            if (!nextWeekDay.ClosedThisDay)
        //                return string.Format(resultFormat, nextWeekDay.Label.ToLower(), nextWeekDay.Open);
        //        }

        //        // Search from the start of the week days array and to the current week day
        //        for (int i = 0; i < this.WeekDays.IndexOf(currentWeekDay); i++)
        //        {
        //            Workday nextWeekDay = this.WeekDays[i];
        //            if (!nextWeekDay.ClosedThisDay)
        //                return string.Format(resultFormat, nextWeekDay.Label.ToLower(), nextWeekDay.Open);
        //        }
        //    }

        //    return string.Empty;
        //}

        /// <summary>
        ///     Method for getting opening hours if a shop opens later today
        /// </summary>
        /// <returns>string</returns>
        //public string GetLaterOpeningHoursForToday()
        //{
        //    string resultFormat = "{0:hh\\:mm}";
        //    DateTime now = DateTime.Now;

        //    // Check if any specific opening hour is today
        //    Holiday today = this.SpecificDays.FirstOrDefault(x => x.SelectedDate.Date == now.Date);

        //    if (today != null)
        //    {
        //        if (!today.ClosedThisDay && today.Open > now.TimeOfDay)
        //        {
        //            return string.Format(resultFormat, today.Open);
        //        }
        //    }
        //    else
        //    {
        //        // Check first/last sunday in month                
        //        DateTime firstSundayInMonth = now.GetFirstSundayOfMonth();
        //        DateTime lastSundayInMonth = now.GetLastWeekDayOfMonth(DayOfWeek.Sunday);

        //        if (now.Date == firstSundayInMonth.Date && this.FirstSunDayInMonth.IsActive && this.FirstSunDayInMonth.Open > now.TimeOfDay && !this.FirstSunDayInMonth.ClosedThisDay)
        //            return string.Format(resultFormat, this.FirstSunDayInMonth.Open);

        //        if (now.Date == lastSundayInMonth.Date && this.LastSundayInMonth.IsActive && this.LastSundayInMonth.Open > now.TimeOfDay && !this.LastSundayInMonth.ClosedThisDay)
        //            return string.Format(resultFormat, this.LastSundayInMonth.Open);
        //    }

        //    // Normal week days
        //    Workday weekDay = this.WeekDays.FirstOrDefault(x => x.DayOfWeek == now.DayOfWeek.ToString());

        //    if (weekDay != null)
        //    {
        //        if (weekDay.Open > now.TimeOfDay && !weekDay.ClosedThisDay)
        //            return string.Format(resultFormat, weekDay.Open);
        //    }

        //    return string.Empty;
        //}

        /// <summary>
        ///     Property for checking if a shop is currently open
        /// </summary>
        public bool IsOpen
        {
            get { return false; }
        }
    }
}