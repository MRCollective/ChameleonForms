using ChameleonForms.Component;
using Humanizer;

namespace ChameleonForms.Templates.Bootstrap4
{
    /// <summary>
    /// Extension methods on <see cref="HtmlAttributes"/> for the Bootstrap 4 template.
    /// </summary>
    public static class ButtonHtmlAttributesExtensions
    {
        /// <summary>
        /// Adds the given emphasis to the button.
        /// </summary>
        /// <example>
        /// @n.Submit("Submit").WithStyle(ButtonStyle.Warning)
        /// </example>
        /// <param name="attrs">The Html Attributes from a navigation button</param>
        /// <param name="style">The style of button</param>
        /// <returns>The Html Attribute object so other methods can be chained off of it</returns>
        public static ButtonHtmlAttributes WithStyle(this ButtonHtmlAttributes attrs, ButtonStyle style)
        {
            // ReSharper disable once MustUseReturnValue
            if (style != ButtonStyle.Default)
                attrs.AddClass(style.Humanize());
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
            // ReSharper disable once MustUseReturnValue
            if (size != ButtonSize.Default && size != ButtonSize.NoneSpecified)
                attrs.AddClass($"btn-{size.Humanize()}");
            return attrs;
        }
    }
}
