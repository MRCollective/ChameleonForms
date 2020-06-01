using System;
using System.Threading.Tasks;
using ChameleonForms.Component.Config;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ChameleonForms.TagHelpers
{
    /// <summary>
    /// Tag helper that accepts field configuration attributes for a <field />, <field-element />, <field-label /> or <field-validation />.
    /// </summary>
    [HtmlTargetElement("field")]
    [HtmlTargetElement("field-element")]
    [HtmlTargetElement("field-label")]
    [HtmlTargetElement("field-validation")]
    public class FieldConfigurationBaseTagHelper : TagHelper
    {
        /// <summary>
        /// Fluent configuration of the <see cref="IFieldConfiguration"/>.
        /// </summary>
        public Func<IFieldConfiguration, IFieldConfiguration> FluentConfig { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var fc = context.GetFieldConfiguration();

            FluentConfig?.Invoke(fc);

            return Task.CompletedTask;
        }
    }
}
