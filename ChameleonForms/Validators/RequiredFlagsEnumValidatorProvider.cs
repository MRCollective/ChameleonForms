using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ChameleonForms.Validators
{
    /// <summary>
    /// Provides a validator to validate [Required] non-nullable flags enum fields.
    /// They are flagged as `IsRequired` on <see cref="Microsoft.AspNetCore.Mvc.ModelBinding.ModelMetadata"/>, but no validation is done when they are empty if there isn't an explicit [Required].
    /// </summary>
    public class RequiredFlagsEnumValidatorProvider : IModelValidatorProvider
    {
        /// <inheritdoc />
        public void CreateValidators(ModelValidatorProviderContext context)
        {
            if (context.ModelMetadata.IsFlagsEnum && !context.ModelMetadata.IsNullableValueType && context.ModelMetadata.IsRequired)
                context.Results.Add(new ValidatorItem
                {
                    Validator = new RequiredFlagsEnumValidator(),
                    IsReusable = true
                });
        }
    }

    /// <summary>
    /// Validates [Required] flags enum fields.
    /// </summary>
    public class RequiredFlagsEnumValidator : IModelValidator
    {
        /// <inheritdoc />
        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            if (Convert.ToInt64(context.Model) == 0L)
            {
                return new[]
                {
                    new ModelValidationResult("", $"The {context.ModelMetadata.DisplayName ?? context.ModelMetadata.Name} field is required."), 
                };
            }

            return Enumerable.Empty<ModelValidationResult>();
        }
    }
}
