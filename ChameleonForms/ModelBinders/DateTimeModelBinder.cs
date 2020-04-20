using Microsoft.AspNetCore.Mvc.ModelBinding;
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
            var formatString = bindingContext.ModelMetadata.EditFormatString;
            var dateParseString = formatString.Replace("{0:", "").Replace("}", "");

            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var submittedValue = value == default(ValueProviderResult) ? null : value.FirstValue;
            var isNullable = bindingContext.ModelType.IsGenericType &&
                             bindingContext.ModelType.GetGenericTypeDefinition() == typeof(Nullable<>);

            if (isNullable && string.IsNullOrEmpty(submittedValue))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
            }
            else if (string.IsNullOrEmpty(submittedValue) ||
                !DateTime.TryParseExact(submittedValue, dateParseString, CultureInfo.CurrentCulture.DateTimeFormat, DateTimeStyles.None, out DateTime parsedDate))
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName,
                    $"The value '{submittedValue ?? ""}' is not valid for {bindingContext.ModelMetadata.DisplayName ?? bindingContext.ModelMetadata.PropertyName}. Format of date is {dateParseString}.");
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Success(parsedDate);
            }

            return Task.CompletedTask;
        }
    }
}