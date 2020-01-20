using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Our.Umbraco.OpeningHours.Converters;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace Our.Umbraco.OpeningHours.Composition
{
    public class OpeningHoursComposer : IUserComposer
    {
        public void Compose(global::Umbraco.Core.Composing.Composition composition)
        {
            // Appending property value converter
            composition.PropertyValueConverters().Append<OpeningHoursValueConverter>();
        }
    }
}