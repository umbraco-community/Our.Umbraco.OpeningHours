using System;
using System.Linq;
using Our.Umbraco.OpeningHours.Model;

namespace Our.Umbraco.OpeningHours.Extensions
{
    public static class OpeningHoursExtensions
    {
        public static WeekdayOpeningHours TodaysOpeningHours(this Model.OpeningHours model,
            DateTime? today = null)
        {
            if (model == null) return null;

            if (!today.HasValue)
            {
                today = DateTime.Now;
            }

            // Find any holidays with todays date
            if (model.Holidays != null)
            {
                var holiday = model.Holidays.FirstOrDefault(x => x.Date.HasValue 
                    && x.Date.Value.Date == today.Value.Date);
                if (holiday != null)
                {
                    return holiday;
                }
            }

            // No holidays, so check general weekday opening hours
            return model.Weekdays != null && model.Weekdays.ContainsKey(today.Value.DayOfWeek) 
                ? model.Weekdays[today.Value.DayOfWeek]
                : null;
        }

        public static bool IsOpen24Hours(this WeekdayOpeningHours model)
        {
            if (model == null) return false;
            if (!model.IsOpen) return false;

            var midnight = new TimeSpan(0,0,0);
            return model.Opens == midnight && model.Closes == midnight;
        }

        public static int ClosesInHowManyMinutes(this Model.OpeningHours model, 
            DateTime? now = null)
        {
            if (model == null) return -1; // Default to store closed

            if (!now.HasValue)
            {
                now = DateTime.Now;
            }

            // Check to see if store is open today at all
            var todaysOpeningHours = model.TodaysOpeningHours(now);
            if (todaysOpeningHours == null || !todaysOpeningHours.IsOpen)
            {
                return -1; // store closed 
            }

            // Check to see if store is 24 hour
            if (todaysOpeningHours.IsOpen24Hours())
            {
                return 1440; // 24 hours
            }

            // Check to see if store has opened yet
            var timeOfDay = now.Value.TimeOfDay;
            if (timeOfDay < todaysOpeningHours.Opens)
            {
                return -2; // store not opened yet
            }

            // Check to see when store closes
            var midnight = new TimeSpan(0, 0, 0);
            var closingTime = todaysOpeningHours.Closes;
            if(closingTime == midnight) closingTime = new TimeSpan(23,59,59);

            var closesIn = (int)(closingTime - timeOfDay).TotalMinutes;
            return closesIn > 0 ? closesIn : -3;
        }
    }
}