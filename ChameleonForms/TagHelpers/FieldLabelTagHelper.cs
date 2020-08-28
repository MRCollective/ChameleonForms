using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ChameleonForms.Component;
using ChameleonForms.Utils;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ChameleonForms.TagHelpers
{
    /// <summary>
    /// Creates a ChameleonForms form field label, use within a ChameleonForm form context.
    /// </summary>
    public class FieldLabelTagHelper : ModelPropertyTagHelper
    {
        /// <inheritdoc />
        public override Task ProcessUsingModelPropertyAsync<TModel, TProperty>(TagHelperContext context, TagHelperOutput output,
            Expression<Func<TModel, TProperty>> modelProperty)
        {
            var helper = ViewContext.GetHtmlHelper<TModel>();
            var f = helper.GetChameleonForm();
            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = null;
            output.Content.SetHtmlContent(f.LabelFor(modelProperty, context.GetFieldConfiguration()));
            return Task.CompletedTask;
        }
    }
}
