using System;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;
using Our.Umbraco.OpeningHours.Model;

namespace Our.Umbraco.OpeningHours.Converters
{
    [PropertyValueType(typeof(OpeningHours))]
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
                return Model.OpeningHours.Deserialize(source as string);
            }
            catch (Exception e)
            {
                LogHelper.Error<OpeningHoursValueConverter>("Error converting value", e);
            }

            // Create default model
            return new Model.OpeningHours();
        }
    }
}
