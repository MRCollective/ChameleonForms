using System;
using System.Threading.Tasks;
using ChameleonForms.Component.Config;
using ChameleonForms.Utils;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ChameleonForms.TagHelpers
{
    /// <summary>
    /// Specify HTML for the manually specified field's element.
    /// </summary>
    [HtmlTargetElement("manual-element", ParentTag = "field")]
    public class ManualFieldElementTagHelper : TagHelper
    {
        /// <summary>
        /// Called when the tag helper is being processed.
        /// </summary>
        /// <param name="context">The context within which the tag helper is processed</param>
        /// <param name="output">The output from the tag helper</param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var fieldContext = (ManualFieldContext)(context.Items[typeof(ManualFieldTagHelper)]);
            fieldContext.Element = await output.GetChildContentAsync();
            output.SuppressOutput();
        }
    }

    /// <summary>
    /// Specify HTML for the manually specified field's label.
    /// </summary>
    [HtmlTargetElement("manual-label", ParentTag = "field")]
    public class ManualFieldLabelTagHelper : TagHelper
    {
        /// <summary>
        /// Called when the tag helper is being processed.
        /// </summary>
        /// <param name="context">The context within which the tag helper is processed</param>
        /// <param name="output">The output from the tag helper</param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var fieldContext = (ManualFieldContext)(context.Items[typeof(ManualFieldTagHelper)]);
            fieldContext.Label = await output.GetChildContentAsync();
            output.SuppressOutput();
        }
    }

    /// <summary>
    /// Specify HTML for the manually specified field's validation.
    /// </summary>
    [HtmlTargetElement("manual-validation", ParentTag = "field")]
    public class ManualFieldValidationTagHelper : TagHelper
    {
        /// <summary>
        /// Called when the tag helper is being processed.
        /// </summary>
        /// <param name="context">The context within which the tag helper is processed</param>
        /// <param name="output">The output from the tag helper</param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var fieldContext = (ManualFieldContext) (context.Items[typeof(ManualFieldTagHelper)]);
            fieldContext.Validation = await output.GetChildContentAsync();
            output.SuppressOutput();
        }
    }

    /// <summary>
    /// Context object for a manually specified form field.
    /// </summary>
    public class ManualFieldContext
    {
        /// <summary>
        /// Optional model metadata for the field.
        /// </summary>
        public ModelMetadata ModelMetadata { get; set; }

        /// <summary>
        /// Optionally specify whether the field is in a valid state.
        /// </summary>
        public bool? IsValid { get; set; }

        /// <summary>
        /// HTML for the field element.
        /// </summary>
        public IHtmlContent Element { get; set; }

        /// <summary>
        /// HTML for the field label.
        /// </summary>
        public IHtmlContent Label { get; set; }

        /// <summary>
        /// HTML for the field validation.
        /// </summary>
        public IHtmlContent Validation { get; set; }

        /// <summary>
        /// Field Configuration.
        /// </summary>
        public IFieldConfiguration FieldConfiguration { get; set; }
    }

    /// <summary>
    /// Creates a ChameleonForms form field context, use within a ChameleonForm form section or form field context.
    /// </summary>
    [HtmlTargetElement("field", Attributes = "manual")]
    public class ManualFieldTagHelper : ModelAwareTagHelper
    {
        /// <summary>
        /// Optionally specify the model metadata for the field.
        /// </summary>
        public ModelMetadata ModelMetadata { get; set; }

        /// <summary>
        /// Optionally specify whether the field is in a valid state.
        /// </summary>
        public bool? IsValid { get; set; }

        /// <summary>
        /// Order in which this tag helper gets executed. Set higher than default so field configuration extensions can apply, but lower than the &lt;field&gt; tag helper so it can take control.
        /// </summary>
        public override int Order => 9;

        /// <inheritdoc />
        public override async Task ProcessWhileAwareOfModelTypeAsync<TModel>(TagHelperContext context, TagHelperOutput output)
        {
            var helper = ViewContext.GetHtmlHelper<TModel>();

            var fieldContext = new ManualFieldContext
            {
                ModelMetadata = ModelMetadata,
                IsValid = IsValid,
                FieldConfiguration = context.GetFieldConfiguration()
            };
            context.Items[typeof(ManualFieldTagHelper)] = fieldContext;

            // ReSharper disable once MustUseReturnValue
            await output.GetChildContentAsync();

            if (helper.IsInChameleonFormsSection())
            {
                var s = helper.GetChameleonFormsSection();
                output.TagName = null;
                output.Content.SetHtmlContent(s.Field(fieldContext.Label, fieldContext.Element, fieldContext.Validation, fieldContext.ModelMetadata, fieldContext.IsValid ?? true, fieldContext.FieldConfiguration));
            }
            else
            {
                throw new NotSupportedException("Attempt to specify a <field> outside of a <form-section>.");
            }
        }
    }
}
