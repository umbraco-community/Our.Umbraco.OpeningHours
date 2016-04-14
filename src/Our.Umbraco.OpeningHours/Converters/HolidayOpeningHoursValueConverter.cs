using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Our.Umbraco.OpeningHours.Model;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;

namespace Our.Umbraco.OpeningHours.Converters
{
    [PropertyValueCache(PropertyCacheValue.All, PropertyCacheLevel.Content)]
    public class HolidayOpeningHoursValueConverter : PropertyValueConverterBase
    {
        public override bool IsConverter(PublishedPropertyType propertyType)
        {
            return propertyType.PropertyEditorAlias.InvariantEquals("HolidayOpeningHours");
        }

        public override object ConvertDataToSource(PublishedPropertyType propertyType, object source, bool preview)
        {
            try
            {
                if (source != null && !source.ToString().IsNullOrWhiteSpace())
                {
                    var oh = JsonConvert.DeserializeObject<List<HolidayOpeningHours>>(source.ToString());
                    return oh;
                }
            }
            catch (Exception e)
            {
                LogHelper.Error<OpeningHoursValueConverter>("Error converting value", e);
            }

            // Create default model
            return new List<HolidayOpeningHours>();
        }
    }
}