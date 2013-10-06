using System.Collections.Generic;
using System.Web;
using ChameleonForms.Enums;

namespace ChameleonForms.Component.Config
{
    /// <summary>
    /// Immutable field configuration for use when generating a field's HTML.
    /// </summary>
    public interface IReadonlyFieldConfiguration
    {
        /// <summary>
        /// A dynamic bag to allow for custom extensions using the field configuration.
        /// </summary>
        dynamic Bag { get; }

        /// <summary>
        /// Attributes to add to the form element's HTML.
        /// </summary>
        // todo: consider making this a readonly dictionary
        IDictionary<string, object> HtmlAttributes { get; }

        /// <summary>
        /// Gets any text that has been set for an inline label.
        /// </summary>
        IHtmlString InlineLabelText { get; }

        /// <summary>
        /// Gets any text that has been set for the label.
        /// </summary>
        IHtmlString LabelText { get; }

        /// <summary>
        /// Returns the display type for the field.
        /// </summary>
        FieldDisplayType DisplayType { get; }

        /// <summary>
        /// The label that represents true.
        /// </summary>
        string TrueString { get; }

        /// <summary>
        /// The label that represents false.
        /// </summary>
        string FalseString { get; }

        /// <summary>
        /// The label that represents none.
        /// </summary>
        string NoneString { get; }

        /// <summary>
        /// Get the hint to display with the field.
        /// </summary>
        IHtmlString Hint { get; }

        /// <summary>
        /// A list of HTML to be prepended to the form field in ltr order.
        /// </summary>
        IEnumerable<IHtmlString> PrependedHtml { get; }

        /// <summary>
        /// A list of HTML to be appended to the form field in ltr order.
        /// </summary>
        IEnumerable<IHtmlString> AppendedHtml { get; }

        /// <summary>
        /// The HTML to be used as the field html.
        /// </summary>
        IHtmlString FieldHtml { get; }

        /// <summary>
        /// The format string to use for the field.
        /// </summary>
        string FormatString { get; }

        /// <summary>
        /// Whether or not the empty item is hidden.
        /// </summary>
        bool EmptyItemHidden { get; }

        /// <summary>
        /// Whether or not to use a &lt;label&gt;.
        /// </summary>
        bool HasLabel { get; }

        /// <summary>
        /// Any CSS class(es) to use for the field label.
        /// </summary>
        string LabelClass { get; }
    }

    /// <summary>
    /// Wraps an <see cref="IFieldConfiguration"/> to provide a readonly wrapper over the properties.
    /// </summary>
    public class ReadonlyFieldConfiguration: IReadonlyFieldConfiguration
    {
        private readonly IFieldConfiguration _fieldConfiguration;

        /// <summary>
        /// Creates a <see cref="ReadonlyFieldConfiguration"/> that wraps the provided <see cref="IFieldConfiguration"/>.
        /// </summary>
        /// <param name="fieldConfiguration">The field configuration to wrap</param>
        public ReadonlyFieldConfiguration(IFieldConfiguration fieldConfiguration)
        {
            _fieldConfiguration = fieldConfiguration ?? new FieldConfiguration();
        }

        public dynamic Bag
        {
            get { return _fieldConfiguration.Bag; }
        }

        public IDictionary<string, object> HtmlAttributes
        {
            get { return _fieldConfiguration.Attributes.ToDictionary(); }
        }

        public IHtmlString InlineLabelText
        {
            get { return _fieldConfiguration.InlineLabelText; }
        }

        public IHtmlString LabelText
        {
            get { return _fieldConfiguration.LabelText; }
        }

        public FieldDisplayType DisplayType
        {
            get { return _fieldConfiguration.DisplayType; }
        }

        public string TrueString
        {
            get { return _fieldConfiguration.TrueString; }
        }

        public string FalseString
        {
            get { return _fieldConfiguration.FalseString; }
        }

        public string NoneString
        {
            get { return _fieldConfiguration.NoneString; }
        }

        public IHtmlString Hint
        {
            get { return _fieldConfiguration.Hint; }
        }

        public IEnumerable<IHtmlString> PrependedHtml
        {
            get { return _fieldConfiguration.PrependedHtml; }
        }

        public IEnumerable<IHtmlString> AppendedHtml
        {
            get { return _fieldConfiguration.AppendedHtml; }
        }

        public IHtmlString FieldHtml
        {
            get { return _fieldConfiguration.FieldHtml; }
        }

        public string FormatString
        {
            get { return _fieldConfiguration.FormatString; }
        }

        public bool EmptyItemHidden
        {
            get { return _fieldConfiguration.EmptyItemHidden; }
        }

        public bool HasLabel
        {
            get { return _fieldConfiguration.HasLabel; }
        }

        public string LabelClass
        {
            get { return _fieldConfiguration.LabelClasses; }
        }
    }
}
