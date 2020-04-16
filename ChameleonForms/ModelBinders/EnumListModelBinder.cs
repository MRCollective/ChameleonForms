using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging;

namespace ChameleonForms.ModelBinders
{
    /// <summary>
    /// Binds a list of enums in a model.
    /// </summary>
    public class EnumListModelBinder<T> : ArrayModelBinder<T>
    {
        /// <inheritdoc />
        public override async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            await base.BindModelAsync(bindingContext);
            
            var boundModel = (((IEnumerable<T>)bindingContext.Result.Model) ?? new T[0]).ToArray();
            var model = boundModel.Where(x => x != null).ToArray();
            bindingContext.Result = ModelBindingResult.Success(model);

            // Add error if a [Required] enum list didn't have any values
            if (model.Length == 0 && bindingContext.ModelMetadata.IsRequired && (!bindingContext.ModelState.ContainsKey(bindingContext.ModelName) || bindingContext.ModelState[bindingContext.ModelName].Errors.Count == 0))
            {
                bindingContext.ModelState.AddModelError(
                    string.IsNullOrEmpty(bindingContext.ModelName)
                        ? bindingContext.ModelMetadata.Name
                        : bindingContext.ModelName,
                    $"The {bindingContext.ModelMetadata.DisplayName ?? bindingContext.ModelMetadata.Name} field is required.");
            }

            // Remove error if non-required, non-nullable enum list with null value(s)
            if (!bindingContext.ModelMetadata.IsRequired &&
                !bindingContext.ModelMetadata.ElementMetadata.IsNullableValueType)
            {
                var submittedValues = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
                var allItemsEmptyOrValid = submittedValues.All(v =>
                    string.IsNullOrEmpty(v)
                    || Enum.TryParse(bindingContext.ModelMetadata.ElementMetadata.UnderlyingOrModelType, v, true, out var x));

                if (allItemsEmptyOrValid && bindingContext.ModelState.ContainsKey(bindingContext.ModelName))
                {
                    bindingContext.ModelState[bindingContext.ModelName].Errors.Clear();
                    bindingContext.ModelState[bindingContext.ModelName].ValidationState = ModelValidationState.Valid;
                }
            }
        }

        public EnumListModelBinder(IModelBinder elementBinder, ILoggerFactory loggerFactory, bool allowValidatingTopLevelNodes, MvcOptions mvcOptions)
            : base(elementBinder, loggerFactory, allowValidatingTopLevelNodes, mvcOptions)
        {}
    }
}
