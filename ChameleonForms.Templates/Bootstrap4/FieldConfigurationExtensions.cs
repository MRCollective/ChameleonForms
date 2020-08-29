using ChameleonForms.Component.Config;

namespace ChameleonForms.Templates.Bootstrap4
{
    /// <summary>
    /// Extension methods on <see cref="IFieldConfiguration"/> for the Bootstrap 4 template.
    /// </summary>
    public static class FieldConfigurationExtensions
    {
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
