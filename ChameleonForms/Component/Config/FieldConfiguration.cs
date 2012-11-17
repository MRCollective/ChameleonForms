using System.Web;
using ChameleonForms.Templates;

namespace ChameleonForms.Component.Config
{
    /// <summary>
    /// Holds configuration data for a form field.
    /// </summary>
    public interface IFieldConfiguration : IHtmlString
    {
        /// <summary>
        /// Attributes to add to the form element's HTML.
        /// </summary>
        HtmlAttributes Attributes { get; set; }

        /// <summary>
        /// Sets the field that the field configuration is wrapping so that
        ///     a call to ToHtmlString() will output the given field.
        /// </summary>
        /// <param name="field">The field being configured</param>
        void SetField(IHtmlString field);
    }

    /// <summary>
    /// Holds configuration data for a form field.
    /// </summary>
    public class FieldConfiguration : IFieldConfiguration
    {
        private IHtmlString _field;

        /// <summary>
        /// Constructs a field configuration.
        /// </summary>
        public FieldConfiguration()
        {
            Attributes = new HtmlAttributes();
        }

        public HtmlAttributes Attributes { get; set; }

        public void SetField(IHtmlString field)
        {
            _field = field;
        }

        public string ToHtmlString()
        {
            return _field.ToHtmlString();
        }
    }
}
