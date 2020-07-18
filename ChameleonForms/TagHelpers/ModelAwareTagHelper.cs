using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ChameleonForms.TagHelpers
{
    /// <summary>
    /// Tag helper that is aware of the current page model type (via a generic Process method).
    /// </summary>
    public abstract class ModelAwareTagHelper : TagHelper
    {

        /// <summary>
        /// Order in which this tag helper gets executed. Set higher than default so there is opportunity to extend this functionality.
        /// </summary>
        public override int Order => 10;

        /// <summary>
        /// The page's <see cref="Microsoft.AspNetCore.Mvc.Rendering.ViewContext"/>.
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// Called when the tag helper is being processed.
        /// </summary>
        /// <param name="context">The context within which the tag helper is processed</param>
        /// <param name="output">The output from the tag helper</param>
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            // ReSharper disable once PossibleNullReferenceException
            return GetType().GetMethod(nameof(ProcessWhileAwareOfModelTypeAsync)).MakeGenericMethod(ViewContext.ViewData.ModelMetadata.ModelType)
                .Invoke(this, new object[] { context, output }) as Task;
        }

        /// <summary>
        /// Asynchronously executes the <see cref="TagHelper"/> with the given <paramref name="context"/> and
        /// <paramref name="output"/> against a page model type.
        /// </summary>
        /// <param name="context">Contains information associated with the current HTML tag.</param>
        /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
        /// <returns>A <see cref="Task"/> that on completion updates the <paramref name="output"/>.</returns>
        public abstract Task ProcessWhileAwareOfModelTypeAsync<TModel>(TagHelperContext context, TagHelperOutput output);
    }
}
