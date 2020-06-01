using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ChameleonForms.Component;
using ChameleonForms.Utils;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ChameleonForms.TagHelpers
{
    /// <summary>
    /// Creates a ChameleonForms form submit button, use within a ChameleonForm form navigation context.
    /// </summary>
    public class FormSubmitTagHelper : ModelAwareTagHelper
    {
        /// <summary>
        /// Name to submit value for.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Value to submit; requires Name to be specified.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Button label (if text, otherwise make it the content within the button)
        /// </summary>
        public string Label { get; set; }

        public Func<ButtonHtmlAttributes, ButtonHtmlAttributes> FluentAttrs { get; set; }

        /// <inheritdoc />
        public override async Task ProcessWhileAwareOfModelTypeAsync<TModel>(TagHelperContext context, TagHelperOutput output)
        {
            var helper = ViewContext.GetHtmlHelper<TModel>();
            var n = helper.GetChameleonFormsNavigation();
            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = null;
            var childContent = Label?.ToHtml() ?? await output.GetChildContentAsync(HtmlEncoder.Default);
            if (!string.IsNullOrEmpty(Value) && string.IsNullOrEmpty(Name))
                throw new NotSupportedException($"Form submit specified with Value ({Value}), but not Name.");

            ButtonHtmlAttributes buttonOutput;
            if (!string.IsNullOrEmpty(Value))
                buttonOutput = n.Submit(Name, Value, childContent);
            else
                buttonOutput = n.Submit(childContent);

            FluentAttrs?.Invoke(buttonOutput);

            var finalOutput = buttonOutput.Attrs(context.GetHtmlAttributes().Attributes);
            output.Content.SetHtmlContent(finalOutput);

        }
    }
}
