using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace ChameleonForms.ModelBinders
{
    class DateTimeModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.UnderlyingOrModelType == typeof(DateTime) && !string.IsNullOrEmpty(context.Metadata.DisplayFormatString))
            {
                return new DateTimeModelBinder();
            }

            return null;
        }
    }
}
