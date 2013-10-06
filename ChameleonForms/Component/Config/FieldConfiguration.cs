using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web;
using ChameleonForms.Enums;
using ChameleonForms.Templates;

namespace ChameleonForms.Component.Config
{
    /// <summary>
    /// Holds configuration data for a form field.
    /// </summary>
    public interface IFieldConfiguration : IHtmlString
    {
        /// <summary>
        /// A dynamic bag to allow for custom extensions using the field configuration.
        /// </summary>
        dynamic Bag { get; }

        /// <summary>
        /// Attributes to add to the form element's HTML.
        /// </summary>
        HtmlAttributes Attributes { get; }

        /// <summary>
        /// Override the default id for the field.
        /// </summary>
        /// <param name="id">The text to use for the id</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Id(string id);

        /// <summary>
        /// Adds a CSS class (or a number of CSS classes) to the attributes.
        /// </summary>
        /// <param name="class">The CSS class(es) to add</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration AddClass(string @class);

        /// <summary>
        /// Adds or updates a HTML attribute with a given value.
        /// </summary>
        /// <param name="key">The name of the HTML attribute to add/update</param>
        /// <param name="value">The value of the HTML attribute to add/update</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Attr(string key, object value);

        /// <summary>
        /// Adds or updates a HTML attribute with using a lambda method to express the attribute.
        /// </summary>
        /// <example>
        /// h.Attr(style => "width: 100%;")
        /// </example>
        /// <param name="attribute">A lambda expression representing the attribute to set and its value</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Attr(Func<object, object> attribute);

        /// <summary>
        /// Adds or updates a set of HTML attributes using lambda methods to express the attributes.
        /// </summary>
        /// <param name="attributes">A list of lambas where the lambda variable name is the name of the attribute and the value is the value</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Attrs(params Func<object, object>[] attributes);

        /// <summary>
        /// Adds or updates a set of HTML attributes using a dictionary to express the attributes.
        /// </summary>
        /// <param name="attributes">A dictionary of attributes</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Attrs(IDictionary<string, object> attributes);

        /// <summary>
        /// Adds or updates a set of HTML attributes using anonymous objects to express the attributes.
        /// </summary>
        /// <param name="attributes">An anonymous object of attributes</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Attrs(object attributes);

        /// <summary>
        /// Sets the number of rows for a textarea to use.
        /// </summary>
        /// <param name="numRows">The number of rows for the textarea</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Rows(int numRows);

        /// <summary>
        /// Sets the number of cols for a textarea to use.
        /// </summary>
        /// <param name="numCols">The number of cols for the textarea</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Cols(int numCols);

        /// <summary>
        /// Sets the field to be disabled (value not submitted, can not click).
        /// </summary>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Disabled(bool disabled = true);

        /// <summary>
        /// Sets the field to be readonly (value can not be modified).
        /// </summary>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Readonly(bool @readonly = true);

        /// <summary>
        /// Sets a hint to the user of what can be entered in the field.
        /// </summary>
        /// <param name="placeholderText">The text to use for the placeholder</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Placeholder(string placeholderText);

        /// <summary>
        /// Gets any text that has been set for an inline label.
        /// </summary>
        IHtmlString InlineLabelText { get; }

        /// <summary>
        /// Sets an inline label for a checkbox.
        /// </summary>
        /// <param name="labelText">The text to use for the label</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration InlineLabel(string labelText);

        /// <summary>
        /// Sets an inline label for a checkbox.
        /// </summary>
        /// <param name="labelHtml">The html to use for the label</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration InlineLabel(IHtmlString labelHtml);

        /// <summary>
        /// Gets any text that has been set for the label.
        /// </summary>
        IHtmlString LabelText { get; }

        /// <summary>
        /// Override the default label for the field.
        /// </summary>
        /// <param name="labelText">The text to use for the label</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Label(string labelText);

        /// <summary>
        /// Override the default label for the field.
        /// </summary>
        /// <param name="labelHtml">The text to use for the label</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Label(IHtmlString labelHtml);

        /// <summary>
        /// Returns the display type for the field.
        /// </summary>
        FieldDisplayType DisplayType { get; }

        /// <summary>
        /// Renders the field as a list of radio or checkbox items.
        /// Use for a list or boolean value.
        /// </summary>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration AsList();

        /// <summary>
        /// Renders the field as a drop-down control.
        /// Use for a list or boolean value.
        /// </summary>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration AsDropDown();

        /// <summary>
        /// The label that represents true.
        /// </summary>
        string TrueString { get; }

        /// <summary>
        /// Change the label that represents true.
        /// </summary>
        /// <param name="trueString">The label to use as true</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration WithTrueAs(string trueString);

        /// <summary>
        /// The label that represents false.
        /// </summary>
        string FalseString { get; }

        /// <summary>
        /// Change the label that represents none.
        /// </summary>
        /// <param name="noneString">The label to use as none</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration WithNoneAs(string noneString);
        
        /// <summary>
        /// The label that represents none.
        /// </summary>
        string NoneString { get; }

        /// <summary>
        /// Change the label that represents false.
        /// </summary>
        /// <param name="falseString">The label to use as false</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration WithFalseAs(string falseString);
        
        /// <summary>
        /// Sets a lambda expression to get the field that the field configuration is wrapping so that
        ///     a call to ToHtmlString() will output the given field.
        /// </summary>
        /// <param name="field">A lambda returning the HTML to output</param>
        void SetField(Func<IHtmlString> field);

        /// <summary>
        /// Sets the field that the field configuration is wrapping so that
        ///     a call to ToHtmlString() will output the given field.
        /// </summary>
        /// <param name="field">The field being configured</param>
        void SetField(IHtmlString field);

        /// <summary>
        /// Supply a string hint to display along with the field.
        /// </summary>
        /// <param name="hint">The hint string</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration WithHint(string hint);

        /// <summary>
        /// Supply a HTML hint to display along with the field.
        /// </summary>
        /// <param name="hint">The hint markup</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration WithHint(IHtmlString hint);

        /// <summary>
        /// Get the hint to display with the field.
        /// </summary>
        IHtmlString Hint { get; }

        /// <summary>
        /// Prepends the given HTML to the form field.
        /// </summary>
        /// <param name="html">The HTML to prepend</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Prepend(IHtmlString html);

        /// <summary>
        /// Prepends the given string to the form field.
        /// </summary>
        /// <param name="str">The string to prepend</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Prepend(string str);

        /// <summary>
        /// A list of HTML to be prepended to the form field in ltr order.
        /// </summary>
        IEnumerable<IHtmlString> PrependedHtml { get; }

        /// <summary>
        /// Appends the given HTML to the form field.
        /// </summary>
        /// <param name="html">The HTML to append</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Append(IHtmlString html);

        /// <summary>
        /// Appends the given string to the form field.
        /// </summary>
        /// <param name="str">The string to prepend</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Append(string str);

        /// <summary>
        /// A list of HTML to be appended to the form field in ltr order.
        /// </summary>
        IEnumerable<IHtmlString> AppendedHtml { get; }

        /// <summary>
        /// Override the HTML of the form field.
        /// 
        /// This gives you ultimate flexibility with your field HTML when it's
        /// not quite what you want, but you still want the form template
        /// (e.g. label, surrounding html and validation message).
        /// </summary>
        /// <param name="html">The HTML for the field</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration OverrideFieldHtml(IHtmlString html);

        /// <summary>
        /// The HTML to be used as the field html.
        /// </summary>
        IHtmlString FieldHtml { get; }
        
        /// <summary>
        /// Uses the given format string when outputting the field value.
        /// </summary>
        /// <param name="formatString">The format string to use</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration WithFormatString(string formatString);

        /// <summary>
        /// The format string to use for the field.
        /// </summary>
        string FormatString { get; }

        /// <summary>
        /// Hide the empty item that would normally display for the field.
        /// </summary>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration HideEmptyItem();

        /// <summary>
        /// Whether or not the empty item is hidden.
        /// </summary>
        bool EmptyItemHidden { get; }

        /// <summary>
        /// Don't use a &lt;label&gt;, but still include the label text for the field.
        /// </summary>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration WithoutLabel();

        /// <summary>
        /// Whether or not to use a &lt;label&gt;.
        /// </summary>
        bool HasLabel { get; }

        /// <summary>
        /// Specify one or more CSS classes to use for the field label.
        /// </summary>
        /// <param name="classes">Any CSS classes to use for the field label</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration WithLabelClasses(string classes);

        /// <summary>
        /// Any CSS class(es) to use for the field label.
        /// </summary>
        string LabelClasses { get; }

        /// <summary>
        /// Returns readonly field configuration from the current field configuration.
        /// </summary>
        /// <returns>A readonly field configuration</returns>
        IReadonlyFieldConfiguration ToReadonly();
    }

    /// <summary>
    /// Holds configuration data for a form field.
    /// </summary>
    public class FieldConfiguration : IFieldConfiguration
    {
        private Func<IHtmlString> _field;
        private readonly List<IHtmlString> _prependedHtml = new List<IHtmlString>();
        private readonly List<IHtmlString> _appendedHtml = new List<IHtmlString>();
        private IHtmlString _fieldHtml;

        /// <summary>
        /// Constructs a field configuration.
        /// </summary>
        public FieldConfiguration()
        {
            Attributes = new HtmlAttributes();
            DisplayType = FieldDisplayType.Default;
            TrueString = "Yes";
            FalseString = "No";
            NoneString = "";
            HasLabel = true;
            Bag = new ExpandoObject();
        }

        public dynamic Bag { get; private set; }
        public HtmlAttributes Attributes { get; private set; }

        public IFieldConfiguration Id(string id)
        {
            Attributes.Id(id);
            return this;
        }

        public IFieldConfiguration AddClass(string @class)
        {
            Attributes.AddClass(@class);
            return this;
        }

        public IFieldConfiguration Attr(string key, object value)
        {
            Attributes.Attr(key, value);
            return this;
        }

        public IFieldConfiguration Attr(Func<object, object> attribute)
        {
            Attributes.Attr(attribute);
            return this;
        }

        public IFieldConfiguration Attrs(params Func<object, object>[] attributes)
        {
            Attributes.Attrs(attributes);
            return this;
        }

        public IFieldConfiguration Attrs(IDictionary<string, object> attributes)
        {
            Attributes.Attrs(attributes);
            return this;
        }

        public IFieldConfiguration Attrs(object attributes)
        {
            Attributes.Attrs(attributes);
            return this;
        }

        public IFieldConfiguration Rows(int numRows)
        {
            Attr("rows", numRows);
            return this;
        }

        public IFieldConfiguration Cols(int numCols)
        {
            Attr("cols", numCols);
            return this;
        }

        public IFieldConfiguration Disabled(bool disabled = true)
        {
            Attributes.Disabled(disabled);
            return this;
        }

        public IFieldConfiguration Readonly(bool @readonly = true)
        {
            Attributes.Readonly(@readonly);
            return this;
        }

        public IFieldConfiguration Placeholder(string placeholderText)
        {
            Attr("placeholder", placeholderText);
            return this;
        }

        public IHtmlString InlineLabelText { get; private set; }

        public IFieldConfiguration InlineLabel(string labelText)
        {
            InlineLabelText = labelText.ToHtml();
            return this;
        }

        public IFieldConfiguration InlineLabel(IHtmlString labelHtml)
        {
            InlineLabelText = labelHtml;
            return this;
        }

        public IHtmlString LabelText { get; private set; }

        public IFieldConfiguration Label(string labelText)
        {
            LabelText = labelText.ToHtml();
            return this;
        }

        public IFieldConfiguration Label(IHtmlString labelHtml)
        {
            LabelText = labelHtml;
            return this;
        }

        public FieldDisplayType DisplayType { get; private set; }

        public IFieldConfiguration AsList()
        {
            DisplayType = FieldDisplayType.List;
            return this;
        }

        public IFieldConfiguration AsDropDown()
        {
            DisplayType = FieldDisplayType.DropDown;
            return this;
        }

        public string TrueString { get; private set; }
        public IFieldConfiguration WithTrueAs(string trueString)
        {
            TrueString = trueString;
            return this;
        }

        public string FalseString { get; private set; }
        public IFieldConfiguration WithFalseAs(string falseString)
        {
            FalseString = falseString;
            return this;
        }

        public string NoneString { get; private set; }
        public IFieldConfiguration WithNoneAs(string noneString)
        {
            NoneString = noneString;
            return this;
        }

        public void SetField(IHtmlString field)
        {
            _field = () => field;
        }

        public IFieldConfiguration WithHint(string hint)
        {
            Hint = hint.ToHtml();
            return this;
        }

        public IFieldConfiguration WithHint(IHtmlString hint)
        {
            Hint = hint;
            return this;
        }

        public IHtmlString Hint { get; private set; }
        
        public IFieldConfiguration Prepend(IHtmlString html)
        {
            _prependedHtml.Add(html);
            return this;
        }

        public IFieldConfiguration Prepend(string str)
        {
            _prependedHtml.Add(str.ToHtml());
            return this;
        }

        public IEnumerable<IHtmlString> PrependedHtml { get { var html = _prependedHtml.ToArray(); Array.Reverse(html); return html; } }
        
        public IFieldConfiguration Append(IHtmlString html)
        {
            _appendedHtml.Add(html);
            return this;
        }

        public IFieldConfiguration Append(string str)
        {
            _appendedHtml.Add(str.ToHtml());
            return this;
        }

        public IEnumerable<IHtmlString> AppendedHtml { get { return _appendedHtml.ToArray(); } }

        public IFieldConfiguration WithFormatString(string formatString)
        {
            FormatString = formatString;
            return this;
        }

        public string FormatString { get; private set; }

        public IFieldConfiguration HideEmptyItem()
        {
            EmptyItemHidden = true;
            return this;
        }

        public bool EmptyItemHidden { get; private set; }

        public IFieldConfiguration WithoutLabel()
        {
            HasLabel = false;
            return this;
        }

        public bool HasLabel { get; private set; }
        public IFieldConfiguration WithLabelClasses(string classes)
        {
            LabelClasses = classes;
            return this;
        }

        public string LabelClasses { get; private set; }

        public IFieldConfiguration OverrideFieldHtml(IHtmlString html)
        {
            _fieldHtml = html;
            return this;
        }

        public IHtmlString FieldHtml { get { return _fieldHtml; } }

        public void SetField(Func<IHtmlString> field)
        {
            _field = field;
        }
        
        public string ToHtmlString()
        {
            return _field().ToHtmlString();
        }

        public IReadonlyFieldConfiguration ToReadonly()
        {
            return new ReadonlyFieldConfiguration(this);
        }
    }
}
