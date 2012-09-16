using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Humanizer;

namespace ChameleonForms
{
    // Of course it *should* be Humanised, but I'll keep consistency with the Humanizer library
    public class HumanizedLabels : DataAnnotationsModelMetadataProvider
    {
        public static void Register()
        {
            ModelMetadataProviders.Current = new HumanizedLabels();
        }

        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType,
            Func<object> modelAccessor, Type modelType, string propertyName)
        {
            // Default metadata implementations
            var metadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);

            // Auto-sentence case for display name
            if (metadata.DisplayName == null && metadata.PropertyName != null)
            {
                metadata.DisplayName = metadata.PropertyName.Humanize(LetterCasing.Sentence);
            }

            return metadata;
        }
    }
}