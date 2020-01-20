using System;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;
using Our.Umbraco.OpeningHours.Model;

namespace Our.Umbraco.OpeningHours.Converters
{
    public class OpeningHoursValueConverter : IPropertyValueConverter
    {
        private readonly ILogger _logger;

        public OpeningHoursValueConverter(ILogger logger)
        {
            _logger = logger;
        }

        public bool IsConverter(PublishedPropertyType propertyType)
        {
            return propertyType.EditorAlias.Equals("OpeningHours");
        } 

        public bool? IsValue(object value, PropertyValueLevel level)
        {
            return true;
        }

        public Type GetPropertyValueType(PublishedPropertyType propertyType) => typeof(OpeningHoursModel);

        public object ConvertSourceToIntermediate(IPublishedElement owner, PublishedPropertyType propertyType, object source, bool preview)
        {
            return source;
        }

        public object ConvertIntermediateToObject(IPublishedElement owner, PublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object inter, bool preview)
        {
            try
            {
                return OpeningHoursModel.Deserialize(inter as string);
            }
            catch (Exception e)
            {
                _logger.Error<OpeningHoursValueConverter>("Error converting value", e);
            }

            // Create default model
            return new OpeningHoursModel();
        }

        public object ConvertIntermediateToXPath(IPublishedElement owner, PublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object inter, bool preview)
        {
            return inter.ToString();
        }

        public PropertyCacheLevel GetPropertyCacheLevel(PublishedPropertyType propertyType) => PropertyCacheLevel.Element;

    }
}
