using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ChameleonForms.ModelBinders
{
    class EnumListModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (context.Metadata.IsEnumerableType && context.Metadata.ElementMetadata.IsEnum)
            {
                var elementType = context.Metadata.ElementMetadata.ModelType;
                var binderType = typeof(EnumListModelBinder<>).MakeGenericType(elementType);
                var elementBinder = context.CreateBinder(context.Metadata.ElementMetadata);

                var loggerFactory = context.Services.GetRequiredService<ILoggerFactory>();
                var mvcOptions = context.Services.GetRequiredService<IOptions<MvcOptions>>().Value;
                return (IModelBinder)Activator.CreateInstance(
                    binderType,
                    elementBinder,
                    loggerFactory,
                    true /* allowValidatingTopLevelNodes */,
                    mvcOptions);
            }

            return null;
        }
    }
}
