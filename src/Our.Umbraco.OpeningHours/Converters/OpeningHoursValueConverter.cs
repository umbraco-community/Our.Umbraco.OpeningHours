using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;

namespace Our.Umbraco.OpeningHours.Converters
{
    [PropertyValueCache(PropertyCacheValue.All, PropertyCacheLevel.Content)]
    public class OpeningHoursValueConverter : PropertyValueConverterBase
    {
        public override bool IsConverter(PublishedPropertyType propertyType)
        {
            return propertyType.PropertyEditorAlias.InvariantEquals("OpeningHours");
        }

        public override object ConvertDataToSource(PublishedPropertyType propertyType, object source, bool preview)
        {
            try
            {
                if (source != null && !source.ToString().IsNullOrWhiteSpace())
                {
                    //TODO: Handle timespan conversion
                    var oh = JsonConvert.DeserializeObject<Model.OpeningHours>(source.ToString());
                    return oh;
                }
            }
            catch (Exception e)
            {
                LogHelper.Error<OpeningHoursValueConverter>("Error converting value", e);
            }

            return null;
        }
    }
}