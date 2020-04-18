using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Humanizer;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace ChameleonForms
{
    // Of course it *should* be Humanised, but I'll keep consistency with the Humanizer library :P
    /// <summary>
    /// Data Annotations Model Metadata Provider that transforms camel-case view model property
    /// names to sentence case for their display name unless the display name has already been overriden.
    /// </summary>
    public class HumanizedLabelsDisplayMetadataProvider : IDisplayMetadataProvider
    {
        /// <summary>
        /// Creates the display metadata for a property that results in humanized labels.
        /// </summary>
        /// <param name="context">The display metadata provider context for the property</param>
        public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
        {
            var propertyAttributes = context.Attributes;
            var modelMetadata = context.DisplayMetadata;
            var propertyName = context.Key.Name;

            if (IsTransformRequired(propertyName, modelMetadata, propertyAttributes))
            {
                modelMetadata.DisplayName = () => propertyName.Humanize().Transform(To.SentenceCase);
            }
        }

        private static bool IsTransformRequired(string propertyName, DisplayMetadata modelMetadata, IReadOnlyList<object> propertyAttributes)
        {
            if (!string.IsNullOrEmpty(modelMetadata.SimpleDisplayProperty))
                return false;

            if (propertyAttributes.OfType<DisplayNameAttribute>().Any())
                return false;

            if (propertyAttributes.OfType<DisplayAttribute>().Any())
                return false;

            if (string.IsNullOrEmpty(propertyName))
                return false;

            return true;
        }
    }
}