using System;
using System.Collections.Generic;
using ChameleonForms.FieldGenerators;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ChameleonForms.Validators
{
    /// <summary>
    /// An implementation of <see cref="IClientModelValidatorProvider"/> which provides client validators
    /// for integral numeric types.
    /// </summary>
    public class IntegralNumericClientModelValidatorProvider : IClientModelValidatorProvider
    {
        /// <summary>
        /// Called when validators need to be created from this validator provider.
        /// </summary>
        /// <param name="context">The context within which validators need to be provided</param>
        public void CreateValidators(ClientValidatorProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var typeToValidate = context.ModelMetadata.UnderlyingOrModelType;

            // Check only the numeric types for which we set type='text'.
            if (FieldGeneratorExtensions.IntTypes.Contains(typeToValidate))
            {
                var results = context.Results;
                // Read interface .Count once rather than per iteration
                var resultsCount = results.Count;
                for (var i = 0; i < resultsCount; i++)
                {
                    var validator = results[i].Validator;
                    if (validator != null && validator is NumericIntegralClientModelValidator)
                    {
                        // A validator is already present. No need to add one.
                        return;
                    }
                }

                results.Add(new ClientValidatorItem
                {
                    Validator = new NumericIntegralClientModelValidator(),
                    IsReusable = true
                });
            }
        }
    }
    
    /// <summary>
    /// An implementation of <see cref="IClientModelValidator"/> that provides the rule for validating
    /// integral numeric types.
    /// </summary>
    internal class NumericIntegralClientModelValidator : IClientModelValidator
    {
        /// <inheritdoc />
        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-number", GetErrorMessage(context.ModelMetadata));
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

            var messageProvider = modelMetadata.ModelBindingMessageProvider;
            var name = modelMetadata.DisplayName ?? modelMetadata.Name;
            if (name == null)
            {
                return messageProvider.NonPropertyValueMustBeANumberAccessor();
            }

            return messageProvider.ValueMustBeANumberAccessor(name);
        }
    }
}
