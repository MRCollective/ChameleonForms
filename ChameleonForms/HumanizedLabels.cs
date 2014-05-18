using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Humanizer;

namespace ChameleonForms
{
    // Of course it *should* be Humanised, but I'll keep consistency with the Humanizer library
    /// <summary>
    /// Data Annotations Model Metadata Provider that defaultly transforms camel-case view model property
    /// names to sentence case for their display name.
    /// </summary>
    public class HumanizedLabels : DataAnnotationsModelMetadataProvider
    {
        internal LetterCasing Casing { get; set; }

        /// <summary>
        /// Register the HumanizedLabels metadata provider as the current Model Metadata Provider.
        /// </summary>
        public static void Register(LetterCasing casing = LetterCasing.Sentence)
        {
            ModelMetadataProviders.Current = new HumanizedLabels() { Casing = casing };
        }

        /// <inheritdoc />
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType,
            Func<object> modelAccessor, Type modelType, string propertyName)
        {
            // Default metadata implementations
            var metadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);

            // Auto-sentence case for display name
            if (metadata.DisplayName == null && metadata.PropertyName != null)
            {
                metadata.DisplayName = metadata.PropertyName.Humanize(Casing);
            }

            return metadata;
        }
    }
}