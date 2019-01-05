using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ChameleonForms.ModelBinders
{
    /// <summary>
    /// Binds a flags enum in a model.
    /// </summary>
    public class FlagsEnumModelBinder : IModelBinder
    {
        /// <inheritdoc />
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var nullable = bindingContext.ModelType.IsGenericType && bindingContext.ModelType.GetGenericTypeDefinition() == typeof(Nullable<>);
            var underlyingType = Nullable.GetUnderlyingType(bindingContext.ModelType) ?? bindingContext.ModelType;
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var enumValues = value.Values;
            
            long? enumValueAsLong = null;

            foreach (var enumValue in enumValues.Where(x => !string.IsNullOrEmpty(x)))
            {
                if (Enum.IsDefined(underlyingType, enumValue))
                {
                    var valueAsEnum = Enum.Parse(underlyingType, enumValue, true);
                    enumValueAsLong = (enumValueAsLong ?? 0) | Convert.ToInt64(valueAsEnum);
                }
                else
                {
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName
                        , string.Format("The value '{0}' is not valid for {1}."
                            , enumValue
                            , bindingContext.ModelMetadata.DisplayName ?? bindingContext.ModelMetadata.PropertyName
                            ));
                }
            }
            
            if(enumValueAsLong == null)
            {
                if(nullable)
                {
                    bindingContext.Result = ModelBindingResult.Success(null);
                }
                else
                {
                    bindingContext.Result = ModelBindingResult.Failed();
                }
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Success(Enum.Parse(underlyingType, enumValueAsLong.ToString()));
            }

            return Task.CompletedTask;
        }
    }
}
