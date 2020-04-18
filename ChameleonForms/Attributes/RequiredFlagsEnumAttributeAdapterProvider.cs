using System;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ChameleonForms.Attributes
{
    /// <summary>
    /// Provides [Required] validation for flag enums.
    /// </summary>
    public class RequiredFlagsEnumAttributeAdapterProvider : ValidationAttributeAdapterProvider, IValidationAttributeAdapterProvider
    {
        IAttributeAdapter IValidationAttributeAdapterProvider.GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
        {
            var ret = base.GetAttributeAdapter(attribute, stringLocalizer);
            if (ret == null)
            {
                var type = attribute.GetType();
                if (type == typeof(RequiredFlagsEnumAttribute))
                {
                    ret = new RequiredAttributeAdapter((RequiredAttribute)attribute, stringLocalizer);
                }
            }

            return ret;
        }

        private class RequiredAttributeAdapter : AttributeAdapterBase<RequiredAttribute>
        {
            public RequiredAttributeAdapter(RequiredAttribute attribute, IStringLocalizer stringLocalizer)
                : base(attribute, stringLocalizer)
            {
            }

            public override void AddValidation(ClientModelValidationContext context)
            {
                if (context == null)
                {
                    throw new ArgumentNullException(nameof(context));
                }

                MergeAttribute(context.Attributes, "data-val", "true");
                MergeAttribute(context.Attributes, "data-val-required", GetErrorMessage(context));
            }

            /// <inheritdoc />
            public override string GetErrorMessage(ModelValidationContextBase validationContext)
            {
                if (validationContext == null)
                {
                    throw new ArgumentNullException(nameof(validationContext));
                }

                return GetErrorMessage(validationContext.ModelMetadata, validationContext.ModelMetadata.GetDisplayName());
            }
        }
    }
}
