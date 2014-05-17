using System;
using System.Globalization;
using System.Web.Mvc;

namespace ChameleonForms.ModelBinders
{
    /// <summary>
    /// Binds a datetime in a model using the display format string.
    /// </summary>
    public class DateTimeModelBinder : DefaultModelBinder
    {
        /// <inheritdoc />
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var underlyingType = Nullable.GetUnderlyingType(bindingContext.ModelType) ?? bindingContext.ModelType;
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var submittedValue = value == null ? null : value.AttemptedValue;
            var formatString = bindingContext.ModelMetadata.DisplayFormatString;

            if (
                underlyingType != typeof(DateTime)
                ||
                string.IsNullOrEmpty(formatString)
                ||
                string.IsNullOrEmpty(submittedValue)
            )
                return base.BindModel(controllerContext, bindingContext);

            DateTime parsedDate;
            if (!DateTime.TryParseExact(submittedValue, formatString.Replace("{0:", "").Replace("}", ""), new DateTimeFormatInfo(), DateTimeStyles.None, out parsedDate))
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, string.Format("The value '{0}' is not valid for {1}.", submittedValue, bindingContext.ModelMetadata.DisplayName ?? bindingContext.ModelMetadata.PropertyName));
                bindingContext.ModelMetadata.Model = bindingContext.ModelType.IsValueType
                    ? Activator.CreateInstance(bindingContext.ModelType)
                    : null;
                return bindingContext.ModelMetadata.Model;
            }

            bindingContext.ModelMetadata.Model = parsedDate;
            return parsedDate;
        }
    }
}
