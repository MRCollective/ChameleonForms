using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

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

            if (context.Metadata.IsEnum && context.Metadata.UnderlyingOrModelType.GetCustomAttributes().OfType<FlagsAttribute>().Any())
            {
                return new FlagsEnumModelBinder();
            }

            return null;
        }
    }
}
