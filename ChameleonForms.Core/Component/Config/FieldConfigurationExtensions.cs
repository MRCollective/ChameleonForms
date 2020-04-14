using Microsoft.AspNetCore.Html;
using System;
using System.Web;

namespace ChameleonForms.Component.Config
{
    /// <summary>
    /// Provides additional configuration options to FieldConfiguration
    /// </summary>
    public static class FieldConfigurationExtensions
    {
        /// <summary>
        /// Sets the tab index of a given field
        /// </summary>
        /// <param name="config">Field configuration to update</param>
        /// <param name="index">Tab index to be set</param>
        /// <returns>The instance of IFieldConfiguration passed in to continue chaining things</returns>
        public static IFieldConfiguration TabIndex(this IFieldConfiguration config, int index)
        {
            if (config != null)
                return config.Attr("tabindex", index);
            return null;
        }

        /// <summary>
        /// Applys the autofocus attribute to a given field
        /// </summary>
        /// <param name="config">Field configuration to modify</param>
        /// <returns>The instance of IFieldConfiguration passed in to continue chaining things</returns>
        public static IFieldConfiguration AutoFocus(this IFieldConfiguration config)
        {
            if (config != null)
                return config.Attr("autofocus", "autofocus");
            return null;
        }

        /// <summary>
        /// Override the HTML of the form field.
        /// 
        /// This gives you ultimate flexibility with your field HTML when it's
        /// not quite what you want, but you still want the form template
        /// (e.g. label, surrounding html and validation message).
        /// </summary>
        /// <param name="config">Field configuration to modify</param>
        /// <param name="html">The HTML for the field</param>
        /// <returns>The instance of <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        public static IFieldConfiguration OverrideFieldHtml(this IFieldConfiguration config, Func<object, IHtmlContent> html)
        {
            if (config != null)
                return config.OverrideFieldHtml(html(null));
            return null;
        }

        /// <summary>
        /// Sets an inline label for a checkbox.
        /// </summary>
        /// <param name="labelHtml">The html to use for the label</param>
        /// <param name="config">Field configuration to update</param>
        /// <returns>The instance of <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        public static IFieldConfiguration InlineLabel(this IFieldConfiguration config, Func<object, IHtmlContent> labelHtml)
        {
            if (config != null)
                return config.InlineLabel(labelHtml(null));
            return null;
        }
        /// <summary>
        /// Override the default label for the field.
        /// </summary>
        /// <param name="labelHtml">The text to use for the label</param>
        /// <param name="config">Field configuration to update</param>
        /// <returns>The instance of <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        public static IFieldConfiguration Label(this IFieldConfiguration config, Func<object, IHtmlContent> labelHtml)
        {
            if (config != null)
                return config.Label(labelHtml(null));
            return null;
        }

        /// <summary>
        /// Supply a HTML hint to display along with the field.
        /// </summary>
        /// <param name="hint">The hint markup</param>
        /// <param name="config">Field configuration to update</param>
        /// <returns>The instance of <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        public static IFieldConfiguration WithHint(this IFieldConfiguration config, Func<object, IHtmlContent> hint)
        {
            if (config != null)
                return config.WithHint(hint(null));
            return null;
        }
        /// <summary>
        /// Prepends the given HTML to the form field.
        /// </summary>
        /// <param name="html">The HTML to prepend</param>
        /// <param name="config">Field configuration to update</param>
        /// <returns>The instance of <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        public static IFieldConfiguration Prepend(this IFieldConfiguration config, Func<object, IHtmlContent> html)
        {
            if (config != null)
                return config.Prepend(html(null));
            return null;
        }
        /// <summary>
        /// Appends the given HTML to the form field.
        /// </summary>
        /// <param name="html">The HTML to append</param>
        /// <param name="config">Field configuration to update</param>
        /// <returns>The instance of <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        public static IFieldConfiguration Append(this IFieldConfiguration config, Func<object, IHtmlContent> html)
        {
            if (config != null)
                return config.Append(html(null));
            return null;
        }
    }
}