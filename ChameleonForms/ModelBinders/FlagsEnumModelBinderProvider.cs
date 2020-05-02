using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace ChameleonForms.ModelBinders
{
    class FlagsEnumModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (context.Metadata.IsFlagsEnum)
            {
                return new FlagsEnumModelBinder();
            }

            return null;
        }
    }
}
