using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ChameleonForms.TagHelpers
{
    /// <summary>
    /// Tag helper that acts within the context of a property of the page model.
    /// </summary>
    public abstract class ModelPropertyTagHelper : TagHelper
    {
        /// <summary>
        /// The page's <see cref="Microsoft.AspNetCore.Mvc.Rendering.ViewContext"/>.
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// A property (single or nested) within the page model.
        /// </summary>
        public ModelExpression For { get; set; }

        /// <inheritdoc />
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            // ReSharper disable once PossibleNullReferenceException
            return GetType().GetMethod(nameof(ProcessUsingModelPropertyAsync)).MakeGenericMethod(ViewContext.ViewData.ModelMetadata.ModelType)
                .Invoke(this, new object[] { context, output }) as Task;
        }

        /// <summary>
        /// Asynchronously executes the <see cref="TagHelper"/> with the given <paramref name="context"/> and
        /// <paramref name="output"/> against a property (of type <see cref="TProperty"/>) within
        /// the page model (of type <see cref="TModel"/>).
        /// </summary>
        /// <param name="context">Contains information associated with the current HTML tag.</param>
        /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
        /// <param name="modelProperty">A lambda expression representing the model property being targeted</param>
        /// <returns>A <see cref="Task"/> that on completion updates the <paramref name="output"/>.</returns>
        public abstract Task ProcessUsingModelPropertyAsync<TModel, TProperty>(TagHelperContext context,
            TagHelperOutput output, Expression<Func<TModel, TProperty>> modelProperty);
    }
}
