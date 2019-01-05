using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace ChameleonForms.ModelBinders
{
    /// <summary>
    /// Binds a datetime in a model using the display format string.
    /// </summary>
    public class DateTimeModelBinder : IModelBinder
    {
        /// <inheritdoc />
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var formatString = bindingContext.ModelMetadata.DisplayFormatString;

            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var submittedValue = value == default(ValueProviderResult) ? null : value.FirstValue;
            var isNullable = bindingContext.ModelType.IsGenericType && bindingContext.ModelType.GetGenericTypeDefinition() == typeof(Nullable<>);

            if(isNullable && string.IsNullOrEmpty(submittedValue))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
            }
            else if (string.IsNullOrEmpty(submittedValue) || !DateTime.TryParseExact(submittedValue
                , formatString.Replace("{0:", "").Replace("}", "")
                , CultureInfo.CurrentCulture.DateTimeFormat
                , DateTimeStyles.None
                , out DateTime parsedDate
                ))
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName
                    , string.Format("The value '{0}' is not valid for {1}. Format of date is {2}."
                        , submittedValue ?? ""
                        , bindingContext.ModelMetadata.DisplayName ?? bindingContext.ModelMetadata.PropertyName
                        , formatString
                    ));
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Success(parsedDate);
            }

            return Task.CompletedTask;
        }
    }
}
