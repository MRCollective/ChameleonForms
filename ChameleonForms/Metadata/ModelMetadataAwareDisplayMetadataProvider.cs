using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System;
using System.Linq;

namespace ChameleonForms.Metadata
{
    internal class ModelMetadataAwareDisplayMetadataProvider<TAttribute> : IDisplayMetadataProvider
        where TAttribute : IModelMetadataAware
    {
        public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            context.Attributes
                .OfType<TAttribute>()
                .FirstOrDefault()
                ?.GetDisplayMetadata(context);
        }
    }
}
