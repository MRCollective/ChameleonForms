using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ChameleonForms.TagHelpers
{
    /// <summary>
    /// Tag helper that accepts field configuration attributes for a <field /> or <field-validation />.
    /// </summary>
    [HtmlTargetElement("field")]
    [HtmlTargetElement("field-validation")]
    public class FieldValidationConfigurationTagHelper : TagHelper
    {
        /// <summary>
        /// Specify one or more CSS classes to use for the field validation message.
        /// </summary>
        public string AddValidationClass { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var fc = context.GetFieldConfiguration();

            if (!string.IsNullOrWhiteSpace(AddValidationClass))
                fc.AddValidationClass(AddValidationClass);

            return Task.CompletedTask;
        }
    }
}
