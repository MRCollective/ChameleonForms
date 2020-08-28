using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChameleonForms.ModelBinders
{
    /// <summary>
    /// Binds a flags enum in a model.
    /// </summary>
    public class FlagsEnumModelBinder : IModelBinder
    {
        /// <summary>
        /// Called when binding a model using this model binder.
        /// </summary>
        /// <param name="bindingContext">The context within which binding occurs</param>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var nullable = bindingContext.ModelMetadata.IsNullableValueType;
            var underlyingType = bindingContext.ModelMetadata.UnderlyingOrModelType;
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var values = value.Values;
            
            long? enumValueAsLong = null;

            foreach (var enumValue in values.Where(x => !string.IsNullOrEmpty(x)))
            {
                if (Enum.IsDefined(underlyingType, enumValue))
                {
                    var valueAsEnum = Enum.Parse(underlyingType, enumValue, true);
                    enumValueAsLong = (enumValueAsLong ?? 0) | Convert.ToInt64(valueAsEnum);
                }
                else
                {
                    bindingContext.ModelState.AddModelError(
                        bindingContext.ModelName,
                        string.Format("The value '{0}' is not valid for {1}.",
                        enumValue,
                        bindingContext.ModelMetadata.DisplayName ?? bindingContext.ModelMetadata.Name)
                    );
                }
            }
            
            if (enumValueAsLong == null || enumValueAsLong == 0)
            {
                if (nullable)
                    bindingContext.Result = ModelBindingResult.Success(null);
                else
                {
                    if (!bindingContext.ModelState.ContainsKey(bindingContext.ModelName) ||
                        !bindingContext.ModelState[bindingContext.ModelName].Errors.Any())
                    {
                        // Patching up the in-built support for non-nullable flags enums - they are marked as IsRequired in their ModelMetadata, but don't have required validation
                        bindingContext.ModelState.AddModelError(
                            value == ValueProviderResult.None ? bindingContext.OriginalModelName : bindingContext.ModelName,
                            string.Format("The {0} field is required.", bindingContext.ModelMetadata.DisplayName ?? bindingContext.ModelMetadata.Name)
                        );
                    }
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
