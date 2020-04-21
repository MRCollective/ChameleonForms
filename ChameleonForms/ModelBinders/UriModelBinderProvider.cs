using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace ChameleonForms.ModelBinders
{
    class UriModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(Uri))
                return new UriModelBinder();

            return null;
        }
    }
}
