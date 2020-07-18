using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ChameleonForms.Component;
using ChameleonForms.Utils;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ChameleonForms.TagHelpers
{
    /// <summary>
    /// Creates a ChameleonForms form reset button, use within a ChameleonForm form navigation context.
    /// </summary>
    public class ResetButtonTagHelper : ModelAwareTagHelper
    {
        /// <summary>
        /// Button label (if text, otherwise make it the content within the button)
        /// </summary>
        public string Label { get; set; }

        /// <inheritdoc />
        public Func<ButtonHtmlAttributes, ButtonHtmlAttributes> FluentAttrs { get; set; }

        /// <inheritdoc />
        public override async Task ProcessWhileAwareOfModelTypeAsync<TModel>(TagHelperContext context, TagHelperOutput output)
        {
            var helper = ViewContext.GetHtmlHelper<TModel>();
            var n = helper.GetChameleonFormsNavigation();
            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = null;
            var childContent = Label?.ToHtml() ?? await output.GetChildContentAsync(HtmlEncoder.Default);
            var buttonOutput = n.Reset(childContent);

            FluentAttrs?.Invoke(buttonOutput);

            var finalOutput = buttonOutput.Attrs(context.GetHtmlAttributes().Attributes);
            output.Content.SetHtmlContent(finalOutput);
        }
    }
}
