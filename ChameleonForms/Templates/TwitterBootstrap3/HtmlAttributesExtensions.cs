using ChameleonForms.Component;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;
using Humanizer;

namespace ChameleonForms.Templates.TwitterBootstrap3
{
    /// <summary>
    /// Extension methods on <see cref="HtmlAttributes"/> for Twitter Bootstrap template.
    /// </summary>
    public static class HtmlAttributesExtensions
    {
        /// <summary>
        /// Adds the given icon to the start of a navigation button.
        /// </summary>
        /// <example>
        /// @n.Submit("Submit").WithIcon("arrow-right")
        /// // Output:
        /// &lt;button type="submit">&lt;span class="glyphicon glyphicon-arrow-right">&lt;/span> Submit&lt;/button>
        /// </example>
        /// <param name="attrs">The Html Attributes from a navigation button</param>
        /// <param name="icon">The icon to use; see http://getbootstrap.com/components/#glyphicons-glyphs</param>
        /// <returns>The Html Attribute object so other methods can be chained off of it</returns>
        public static ButtonHtmlAttributes WithIcon(this ButtonHtmlAttributes attrs, string icon)
        {
            attrs.Attr(TwitterBootstrapFormTemplate.IconAttrKey, icon);
            return attrs;
        }

        /// <summary>
        /// Adds the given emphasis to the button.
        /// </summary>
        /// <example>
        /// @n.Submit("Submit").WithStyle(EmphasisStyle.Warning)
        /// </example>
        /// <param name="attrs">The Html Attributes from a navigation button</param>
        /// <param name="style">The style of button</param>
        /// <returns>The Html Attribute object so other methods can be chained off of it</returns>
        public static ButtonHtmlAttributes WithStyle(this ButtonHtmlAttributes attrs, EmphasisStyle style)
        {
            attrs.AddClass(string.Format("btn-{0}", style.ToString().ToLower()));
            return attrs;
        }

        /// <summary>
        /// Changes the button to use the given size.
        /// </summary>
        /// <example>
        /// @n.Submit("Submit").WithSize(ButtonSize.Large)
        /// </example>
        /// <param name="attrs">The Html Attributes from a navigation button</param>
        /// <param name="size">The size of button</param>
        /// <returns>The Html Attribute object so other methods can be chained off of it</returns>
        public static ButtonHtmlAttributes WithSize(this ButtonHtmlAttributes attrs, ButtonSize size)
        {
            if (size != ButtonSize.Default)
                attrs.AddClass(string.Format("btn-{0}", size.Humanize()));
            return attrs;
        }

        /// <summary>
        /// Outputs the field in an input group using prepended and appended HTML.
        /// </summary>
        /// <example>
        /// @n.Field(labelHtml, elementHtml, validationHtml, metadata, new FieldConfiguration().Prepend(beforeHtml).Append(afterHtml).AsInputGroup(), false)
        /// </example>
        /// <param name="fc">The configuration for a field</param>
        /// <returns>The field configuration object to allow for method chaining</returns>
        public static IFieldConfiguration AsInputGroup(this IFieldConfiguration fc)
        {
            fc.Bag.DisplayAsInputGroup = true;
            return fc;
        }
    }
}
