using System;
using System.Linq;
using System.Web.Mvc;

namespace ChameleonForms.ModelBinders
{
    /// <summary>
    /// Binds a flags enum in a model.
    /// </summary>
    public class FlagsEnumModelBinder : DefaultModelBinder
    {
        /// <inheritdoc />
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var underlyingType = Nullable.GetUnderlyingType(bindingContext.ModelType) ?? bindingContext.ModelType;
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var submittedValue = value == null ? null : value.AttemptedValue;

            if (
                !underlyingType.IsEnum
                ||
                !underlyingType.GetCustomAttributes(typeof(FlagsAttribute), false).Any()
                ||
                (string.IsNullOrEmpty(submittedValue))
            )
                return base.BindModel(controllerContext, bindingContext);
            
            var enumValueAsLong = 0L;
            var enumValues = submittedValue.Split(',');
            var error = false;

            foreach (var v in enumValues)
            {
                if (Enum.IsDefined(underlyingType, v))
                {
                    var valueAsEnum = Enum.Parse(underlyingType, v, true);
                    enumValueAsLong |= Convert.ToInt64(valueAsEnum);
                }
                else
                {
                    error = true;
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, string.Format("The value '{0}' is not valid for {1}.", v, bindingContext.ModelMetadata.DisplayName ?? bindingContext.ModelMetadata.PropertyName));
                }
            }

            if (error)
                return Activator.CreateInstance(bindingContext.ModelType);

            bindingContext.ModelMetadata.Model = Enum.Parse(underlyingType, enumValueAsLong.ToString());
            return bindingContext.ModelMetadata.Model;
        }
    }
}
