using ChameleonForms.Templates;

namespace ChameleonForms.Component.Config
{
    /// <summary>
    /// Holds configuration data for a form field.
    /// </summary>
    public interface IFieldConfiguration
    {
        /// <summary>
        /// Attributes to add to the form element's HTML.
        /// </summary>
        HtmlAttributes Attributes { get; set; }
    }

    /// <summary>
    /// Holds configuration data for a form field.
    /// </summary>
    public class FieldConfiguration : IFieldConfiguration
    {
        /// <summary>
        /// Constructs a field configuration.
        /// </summary>
        public FieldConfiguration()
        {
            Attributes = new HtmlAttributes();
        }

        public HtmlAttributes Attributes { get; set; }
    }
}
