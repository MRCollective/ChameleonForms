using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChameleonForms.ModelBinders
{
    /// <summary>
    /// Binds a <see cref="Uri"/> in a model.
    /// </summary>
    public class UriModelBinder : IModelBinder
    {
        /// <inheritdoc />
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var submittedValue = value.FirstValue;
            if (string.IsNullOrWhiteSpace(submittedValue))
                return Task.CompletedTask;

            if (!Uri.TryCreate(submittedValue, UriKind.Absolute, out var uri))
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName,
                    $"The value '{submittedValue ?? ""}' is not a valid URL for {bindingContext.ModelMetadata.DisplayName ?? bindingContext.ModelMetadata.Name}.");
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;

            }

            if (!new[] {Uri.UriSchemeHttp, Uri.UriSchemeHttps}.Contains(uri.Scheme))
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName,
                    $"The value '{submittedValue ?? ""}' is not a valid HTTP(S) URL for {bindingContext.ModelMetadata.DisplayName ?? bindingContext.ModelMetadata.Name}.");
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }
            
            bindingContext.Result = ModelBindingResult.Success(uri);
            return Task.CompletedTask;
        }
    }
}