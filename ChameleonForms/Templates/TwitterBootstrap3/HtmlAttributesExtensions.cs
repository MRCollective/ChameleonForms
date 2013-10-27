using ChameleonForms.Component;

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
    }
}
