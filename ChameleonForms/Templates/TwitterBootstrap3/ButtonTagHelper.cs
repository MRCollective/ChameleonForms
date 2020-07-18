using System.Threading.Tasks;
using ChameleonForms.TagHelpers;
using Humanizer;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ChameleonForms.Templates.TwitterBootstrap3
{
    /// <summary>
    /// Adds Boostrap 3 properties to ChameleonForms form buttons.
    /// </summary>
    [HtmlTargetElement("submit-button")]
    [HtmlTargetElement("form-button")]
    [HtmlTargetElement("reset-button")]
    public class TwitterBootstrap3SubmitButtonTagHelper : TagHelper
    {
        /// <summary>
        /// Button size for Bootstrap 3 form button.
        /// </summary>
        public ButtonSize Size { get; set; }

        /// <summary>
        /// Glyphicon icon for Bootstrap 3 form button.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Emphasis style for Bootstrap 3 form button.
        /// </summary>
        public EmphasisStyle EmphasisStyle { get; set; }

        /// <summary>
        /// Called when the tag helper is being processed.
        /// </summary>
        /// <param name="context">The context within which the tag helper is processed</param>
        /// <param name="output">The output from the tag helper</param>
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var attrs = context.GetHtmlAttributes();

            if (Size != ButtonSize.Default && Size != ButtonSize.NoneSpecified)
                attrs.AddClass($"btn-{Size.Humanize()}");

            if (EmphasisStyle != EmphasisStyle.Default)
                attrs.AddClass($"btn-{EmphasisStyle.ToString().ToLower()}");

            if (!string.IsNullOrEmpty(Icon))
                attrs.Attr(TwitterBootstrap3FormTemplate.IconAttrKey, Icon);

            return Task.CompletedTask;
        }


    }
}
