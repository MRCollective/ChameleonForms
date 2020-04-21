using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

            if (dateParseString == "g")
                dateParseString = string.Join(" ", CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern);

            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            // Patching issue where no value submitted results in a valid state for a DateTime (i.e. [Required] gets skipped)
            if (value == ValueProviderResult.None && !bindingContext.ModelMetadata.IsReferenceOrNullableType)
            {
                bindingContext.ModelState.AddModelError(bindingContext.OriginalModelName,
                    bindingContext.ModelMetadata.ModelBindingMessageProvider.ValueMustNotBeNullAccessor(null));
                return Task.CompletedTask;
            }

            var submittedValue = value.FirstValue;
            if (string.IsNullOrWhiteSpace(submittedValue))
            {
                return new SimpleTypeModelBinder(typeof(DateTime), bindingContext.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>())
                    .BindModelAsync(bindingContext);
            }

            if (!DateTime.TryParseExact(submittedValue, dateParseString, CultureInfo.CurrentCulture.DateTimeFormat, DateTimeStyles.None, out DateTime parsedDate))
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName,
                    $"The value '{submittedValue ?? ""}' is not valid for {bindingContext.ModelMetadata.DisplayName ?? bindingContext.ModelMetadata.Name}. Format of date is {dateParseString}.");
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Success(parsedDate);
            }

            return Task.CompletedTask;
        }
    }
}