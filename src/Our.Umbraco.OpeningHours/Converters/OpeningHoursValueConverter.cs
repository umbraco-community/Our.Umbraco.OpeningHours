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

        public bool IsConverter(IPublishedPropertyType propertyType) => propertyType.EditorAlias.Equals("OpeningHours");
        

        public bool? IsValue(object value, PropertyValueLevel level)
        {
            throw new NotImplementedException();
        }

        public Type GetPropertyValueType(IPublishedPropertyType propertyType)
        {
            throw new NotImplementedException();
        }

        public PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType)
        {
            throw new NotImplementedException();
        }

        public object ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object source,
            bool preview)
        {
            throw new NotImplementedException();
        }

        public object ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType,
            PropertyCacheLevel referenceCacheLevel, object inter, bool preview)
        {
            throw new NotImplementedException();
        }

        public object ConvertIntermediateToXPath(IPublishedElement owner, IPublishedPropertyType propertyType,
            PropertyCacheLevel referenceCacheLevel, object inter, bool preview)
        {
            throw new NotImplementedException();
        }


        //private readonly ILogger _logger;

        //public OpeningHoursValueConverter(ILogger logger)
        //{
        //    _logger = logger;
        //}

        //public bool IsConverter(PublishedPropertyType propertyType)
        //{
        //    
        //} 

        //public bool? IsValue(object value, PropertyValueLevel level)
        //{
        //    return true;
        //}

        //public Type GetPropertyValueType(PublishedPropertyType propertyType) => typeof(OpeningHoursModel);

        //public object ConvertSourceToIntermediate(IPublishedElement owner, PublishedPropertyType propertyType, object source, bool preview)
        //{
        //    return source;
        //}

        //public object ConvertIntermediateToObject(IPublishedElement owner, PublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object inter, bool preview)
        //{
        //    try
        //    {
        //        return OpeningHoursModel.Deserialize(inter as string);
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.Error<OpeningHoursValueConverter>("Error converting value", e);
        //    }

        //    // Create default model
        //    return new OpeningHoursModel();
        //}

        //public object ConvertIntermediateToXPath(IPublishedElement owner, PublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object inter, bool preview)
        //{
        //    return inter.ToString();
        //}

        //public PropertyCacheLevel GetPropertyCacheLevel(PublishedPropertyType propertyType) => PropertyCacheLevel.Element;

    }
}
