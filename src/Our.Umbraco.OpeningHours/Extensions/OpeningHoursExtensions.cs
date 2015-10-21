using System;
using System.Linq;
using Our.Umbraco.OpeningHours.Model;

namespace Our.Umbraco.OpeningHours.Extensions
{
    public static class OpeningHoursExtensions
    {
        public static Timeframe TodaysOpeningHours(this Model.OpeningHours model,
            DateTime? today = null)
        {
            if (model == null) return null;

            if (!today.HasValue)
            {
                today = DateTime.UtcNow;
            }

            // Find any holidays with todays date
            var holiday = model.Holidays.FirstOrDefault(x => x.Date.Date == today.Value.Date);
            if (holiday != null)
            {
                return holiday;
            }

            // No holidays, so check general weekday opening hours
            return model.Weekdays.ContainsKey(today.Value.DayOfWeek) 
                ? model.Weekdays[today.Value.DayOfWeek]
                : null;
        }

        public static int ClosesInHowManyMinutes(this Model.OpeningHours model, 
            DateTime? now = null)
        {
            if (model == null) return -1; // Default to store closed

            if (!now.HasValue)
            {
                now = DateTime.UtcNow;
            }

            // Check to see if store is open today at all
            var todaysOpeningHours = model.TodaysOpeningHours(now);
            if (todaysOpeningHours == null || !todaysOpeningHours.IsOpen)
            {
                return -1; // store closed 
            }

            // Check to see if store has opened yet
            var timeOfDay = now.Value.TimeOfDay;
            if (timeOfDay < todaysOpeningHours.Opens)
            {
                return -2; // store not opened yet
            }

            // Check to see when store closes
            var closesIn = (int)(todaysOpeningHours.Closes - timeOfDay).TotalMinutes;
            return closesIn > 0 ? closesIn : -3;
        }
    }
}