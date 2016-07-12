using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Our.Umbraco.OpeningHours.Model;
using Our.Umbraco.OpeningHours.Model.Items;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;

namespace Our.Umbraco.OpeningHours.Converters
{
    [PropertyValueCache(PropertyCacheValue.All, PropertyCacheLevel.Content)]
    public class WeekdayOpeningHoursValueConverter : PropertyValueConverterBase
    {
        public override bool IsConverter(PublishedPropertyType propertyType)
        {
            return propertyType.PropertyEditorAlias.InvariantEquals("WeekdayOpeningHours");
        }

        public override object ConvertDataToSource(PublishedPropertyType propertyType, object source, bool preview)
        {
            try
            {
                if (source != null && !source.ToString().IsNullOrWhiteSpace())
                {
                    var oh = JsonConvert.DeserializeObject<Dictionary<DayOfWeek, OpeningHoursWeekdayItem>>(source.ToString());
                    return oh;
                }
            }
            catch (Exception e)
            {
                LogHelper.Error<OpeningHoursValueConverter>("Error converting value", e);
            }

            // Create default model
            var dict = new Dictionary<DayOfWeek, OpeningHoursWeekdayItem>();

            for (var i = 0; i < 7; i++)
            {
                dict.Add((DayOfWeek) i, OpeningHoursWeekdayItem.GetEmptyModel((DayOfWeek) i));
            }

            return dict;
        }
    }
}