using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ChameleonForms.Validators
{
    /// <summary>
    /// An implementation of <see cref="IClientModelValidatorProvider"/> which provides client validators
    /// for <see cref="DateTime"/> fields.
    /// </summary>
    public class DateTimeClientModelValidatorProvider : IClientModelValidatorProvider
    {
        /// <inheritdoc />
        public void CreateValidators(ClientValidatorProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var typeToValidate = context.ModelMetadata.UnderlyingOrModelType;

            // Check only the numeric types for which we set type='text'.
            if (typeToValidate == typeof(DateTime))
            {
                var results = context.Results;
                // Read interface .Count once rather than per iteration
                var resultsCount = results.Count;
                for (var i = 0; i < resultsCount; i++)
                {
                    var validator = results[i].Validator;
                    if (validator != null && validator is DateTimeClientModelValidator)
                    {
                        // A validator is already present. No need to add one.
                        return;
                    }
                }

                results.Add(new ClientValidatorItem
                {
                    Validator = new DateTimeClientModelValidator(),
                    IsReusable = true
                });
            }
        }
    }
    
    /// <summary>
    /// An implementation of <see cref="IClientModelValidator"/> that provides the rule for validating
    /// <see cref="DateTime"/> types.
    /// </summary>
    internal class DateTimeClientModelValidator : IClientModelValidator
    {
        /// <inheritdoc />
        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-date", GetErrorMessage(context.ModelMetadata));
        }

        private static void MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (!attributes.ContainsKey(key))
            {
                attributes.Add(key, value);
            }
        }

        private string GetErrorMessage(ModelMetadata modelMetadata)
        {
            if (modelMetadata == null)
            {
                throw new ArgumentNullException(nameof(modelMetadata));
            }

            var name = modelMetadata.DisplayName ?? modelMetadata.Name;
            var formatString = modelMetadata.DisplayFormatString;

            if (formatString == "g")
                formatString = string.Join(" ", CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern);

            var dateParseString = formatString?.Replace("{0:", "")?.Replace("}", "");
            var message = new StringBuilder();

            message.Append(name == null
                ? "The field must be a date"
                : $"The field {name} must be a date"
            );

            if (!string.IsNullOrEmpty(formatString))
                message.Append($" with format {dateParseString}");

            return message.Append(".").ToString();
        }
    }
}
