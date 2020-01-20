using System;
using System.Linq;
using Our.Umbraco.OpeningHours.Model;
using Our.Umbraco.OpeningHours.Model.Items;
using Our.Umbraco.OpeningHours.Model.Offset;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Our.Umbraco.OpeningHours.Extensions {

    public static class OpeningHoursExtensions {

        /// <summary>
        /// Gets opening hours from the property with the <code>openingHours</code> alias. If the property value
        /// doesn't match a valid instance of <see cref="OpeningHoursModel"/>, an instance with empty values will be
        /// returned instead.
        /// </summary>
        /// <param name="content">The instance of <see cref="IPublishedContent"/> holding the opening hours.</param>
        /// <returns>Returns an instance of <see cref="OpeningHoursModel"/>.</returns>
        public static OpeningHoursModel GetOpeningHours(this IPublishedContent content) {
            return GetOpeningHours(content, "openingHours");
        }

        /// <summary>
        /// Gets opening hours from the property with the specified <code>propertyAlias</code>. If the property value
        /// doesn't match a valid instance of <see cref="OpeningHoursModel"/>, an instance with empty values will be
        /// returned instead.
        /// </summary>
        /// <param name="content">The instance of <see cref="IPublishedContent"/> holding the opening hours.</param>
        /// <param name="propertyAlias">The alias of the property.</param>
        /// <returns>Returns an instance of <see cref="OpeningHoursModel"/>.</returns>
        public static OpeningHoursModel GetOpeningHours(this IPublishedContent content, string propertyAlias) {
            OpeningHoursModel openingHours = content == null ? null : content.Value<OpeningHoursModel>(propertyAlias);
            return openingHours ?? new OpeningHoursModel();
        }

        public static OpeningHoursOffsetModel GetOpeningHours(this IPublishedContent content, TimeZoneInfo timeZone) {
            return GetOpeningHours(content, "openingHours", timeZone);
        }

        public static OpeningHoursOffsetModel GetOpeningHours(this IPublishedContent content, string propertyAlias, TimeZoneInfo timeZone) {
            OpeningHoursModel model = GetOpeningHours(content, propertyAlias);
            return new OpeningHoursOffsetModel(model, timeZone);
        }

        public static OpeningHoursWeekdayItem TodaysOpeningHours(this OpeningHoursModel model) {
            return TodaysOpeningHours(model, DateTime.Now);
        }

        public static OpeningHoursWeekdayItem TodaysOpeningHours(this OpeningHoursModel model, DateTime today) {
            
            if (model == null) return null;

            // Find any holidays with todays date
            if (model.Holidays != null) {
                OpeningHoursHolidayItem holiday = model.Holidays.FirstOrDefault(x => x.Date.Date == today.Date);
                if (holiday != null) return holiday;
            }

            // No holidays, so check general weekday opening hours
            return model.Weekdays != null && model.Weekdays.ContainsKey(today.DayOfWeek) ? model.Weekdays[today.DayOfWeek] : null;
        
        }

        public static bool IsOpen24Hours(this OpeningHoursWeekdayItem model) {
            throw new NotImplementedException();
            //if (model == null) return false;
            //if (!model.IsOpen) return false;

            //var midnight = new TimeSpan(0,0,0);
            //return model.Opens == midnight && model.Closes == midnight;
        }

        public static int ClosesInHowManyMinutes(this OpeningHoursModel model, DateTime? now = null) {
            
            throw new NotImplementedException();
            
            //if (model == null) return -1; // Default to store closed

            //if (!now.HasValue)
            //{
            //    now = DateTime.Now;
            //}

            //// Check to see if store is open today at all
            //var todaysOpeningHours = model.TodaysOpeningHours(now);
            //if (todaysOpeningHours == null || !todaysOpeningHours.IsOpen)
            //{
            //    return -1; // store closed 
            //}

            //// Check to see if store is 24 hour
            //if (todaysOpeningHours.IsOpen24Hours())
            //{
            //    return 1440; // 24 hours
            //}

            //// Check to see if store has opened yet
            //var timeOfDay = now.Value.TimeOfDay;
            //if (timeOfDay < todaysOpeningHours.Opens)
            //{
            //    return -2; // store not opened yet
            //}

            //// Check to see when store closes
            //var midnight = new TimeSpan(0, 0, 0);
            //var closingTime = todaysOpeningHours.Closes;
            //if(closingTime == midnight) closingTime = new TimeSpan(23,59,59);

            //var closesIn = (int)(closingTime - timeOfDay).TotalMinutes;
            //return closesIn > 0 ? closesIn : -3;
        }

    }

}