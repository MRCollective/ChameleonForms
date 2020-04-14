using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Web;
using ChameleonForms.Enums;
using Microsoft.AspNetCore.Html;

namespace ChameleonForms.Component.Config
{
    /// <summary>
    /// Holds configuration data for a form field.
    /// </summary>
    public interface IFieldConfiguration : IHtmlContent, IReadonlyFieldConfiguration
    {
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
        IFieldConfiguration InlineLabel(IHtmlContent labelHtml);

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
        IFieldConfiguration Label(IHtmlContent labelHtml);

        /// <summary>
        /// Renders the field as a list of radio options for selecting single values or checkbox items for selecting multiple values.
        /// Use for a list or boolean value.
        /// </summary>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        /// <seealso cref="AsCheckboxList"/>
        IFieldConfiguration AsRadioList();

        /// <summary>
        /// Renders the field as a list of radio options for selecting single values or checkbox items for selecting multiple values.
        /// Use for a list or boolean value.
        /// </summary>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        /// <seealso cref="AsRadioList"/>
        IFieldConfiguration AsCheckboxList();

        /// <summary>
        /// Renders the field as a drop-down control.
        /// Use for a list or boolean value.
        /// </summary>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration AsDropDown();

        /// <summary>
        /// Change the label that represents true.
        /// </summary>
        /// <param name="trueString">The label to use as true</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration WithTrueAs(string trueString);
        
        /// <summary>
        /// Change the label that represents none.
        /// </summary>
        /// <param name="noneString">The label to use as none</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration WithNoneAs(string noneString);
        
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
        void SetField(Func<IHtmlContent> field);

        /// <summary>
        /// Sets the field that the field configuration is wrapping so that
        ///     a call to ToHtmlString() will output the given field.
        /// </summary>
        /// <param name="field">The field being configured</param>
        void SetField(IHtmlContent field);

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
        IFieldConfiguration WithHint(IHtmlContent hint);
        
        /// <summary>
        /// Prepends the given HTML to the form field.
        /// </summary>
        /// <param name="html">The HTML to prepend</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Prepend(IHtmlContent html);

        /// <summary>
        /// Prepends the given string to the form field.
        /// </summary>
        /// <param name="str">The string to prepend</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Prepend(string str);

        /// <summary>
        /// Appends the given HTML to the form field.
        /// </summary>
        /// <param name="html">The HTML to append</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Append(IHtmlContent html);

        /// <summary>
        /// Appends the given string to the form field.
        /// </summary>
        /// <param name="str">The string to prepend</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Append(string str);

        /// <summary>
        /// Override the HTML of the form field.
        /// 
        /// This gives you ultimate flexibility with your field HTML when it's
        /// not quite what you want, but you still want the form template
        /// (e.g. label, surrounding html and validation message).
        /// </summary>
        /// <param name="html">The HTML for the field</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration OverrideFieldHtml(IHtmlContent html);
                
        /// <summary>
        /// Uses the given format string when outputting the field value.
        /// </summary>
        /// <param name="formatString">The format string to use</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration WithFormatString(string formatString);
        
        /// <summary>
        /// Hide the empty item that would normally display for the field.
        /// </summary>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration HideEmptyItem();
        
        /// <summary>
        /// Don't use a &lt;label&gt;, but still include the label text for the field.
        /// </summary>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration WithoutLabelElement();


        /// <summary>
        /// Specify one or more CSS classes to use for the field label.
        /// </summary>
        /// <param name="class">Any CSS class(es) to use for the field label</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration AddLabelClass(string @class);

        /// <summary>
        /// Specify one or more CSS classes to use for the field container element.
        /// </summary>
        /// <param name="class">Any CSS class(es) to use for the field container element</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration AddFieldContainerClass(string @class);
        
        /// <summary>
        /// Specify one or more CSS classes to use for the field validation message.
        /// </summary>
        /// <param name="class">Any CSS class(es) to use for the field validation message</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration AddValidationClass(string @class);
        
        /// <summary>
        /// Excludes one or more Enum values from the generated field.
        /// </summary>
        /// <param name="enumValues">The value of Enum(s) to exclude from the generated field.</param>
        /// <returns></returns>
        IFieldConfiguration Exclude(params Enum[] enumValues);
        
        /// <summary>
        /// Specify that no inline label should be generated.
        /// </summary>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration WithoutInlineLabel();

        /// <summary>
        /// Specify that inline labels should wrap their input element. Important for bootstrap.
        /// </summary>
        /// <param name="wrapElement">True if the input element should be wrapped.</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration InlineLabelWrapsElement(bool wrapElement = true);
    }

    /// <summary>
    /// Holds configuration data for a form field.
    /// </summary>
    public class FieldConfiguration : IFieldConfiguration
    {
        private Func<IHtmlContent> _field;
        private readonly List<IHtmlContent> _prependedHtml = new List<IHtmlContent>();
        private readonly List<IHtmlContent> _appendedHtml = new List<IHtmlContent>();
        private IHtmlContent _fieldHtml;

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
            HasLabelElement = true;
            Bag = new ExpandoObject();
            ExcludedEnums = new Enum[]{};
            HasInlineLabel = true;
        }

        /// <inheritdoc />
        public dynamic Bag { get; private set; }
        /// <inheritdoc />
        public TData GetBagData<TData>(string propertyName)
        {
            var bagAsDictionary = (IDictionary<string, object>) Bag;

            return bagAsDictionary.ContainsKey(propertyName) && bagAsDictionary[propertyName] is TData
                ? (TData) bagAsDictionary[propertyName]
                : default(TData);
        }

        /// <inheritdoc />
        public HtmlAttributes Attributes { get; private set; }

        /// <inheritdoc />
        public IDictionary<string, object> HtmlAttributes
        {
            get
            {
                return Attributes.Attributes.ToDictionary(x => x.Key, x => (object)x.Value);
            }
        }
            

        /// <inheritdoc />
        public IFieldConfiguration Id(string id)
        {
            Attributes.Id(id);
            return this;
        }

        /// <inheritdoc />
        public IFieldConfiguration AddClass(string @class)
        {
            Attributes.AddClass(@class);
            return this;
        }

        /// <inheritdoc />
        public IFieldConfiguration Attr(string key, object value)
        {
            Attributes.Attr(key, value);
            return this;
        }

        /// <inheritdoc />
        public IFieldConfiguration Attr(Func<object, object> attribute)
        {
            Attributes.Attr(attribute);
            return this;
        }

        /// <inheritdoc />
        public IFieldConfiguration Attrs(params Func<object, object>[] attributes)
        {
            Attributes.Attrs(attributes);
            return this;
        }

        /// <inheritdoc />
        public IFieldConfiguration Attrs(IDictionary<string, object> attributes)
        {
            Attributes.Attrs(attributes);
            return this;
        }

        /// <inheritdoc />
        public IFieldConfiguration Attrs(object attributes)
        {
            Attributes.Attrs(attributes);
            return this;
        }

        /// <inheritdoc />
        public IFieldConfiguration Rows(int numRows)
        {
            Attr("rows", numRows);
            return this;
        }

        /// <inheritdoc />
        public IFieldConfiguration Cols(int numCols)
        {
            Attr("cols", numCols);
            return this;
        }

        /// <inheritdoc />
        public IFieldConfiguration Disabled(bool disabled = true)
        {
            Attributes.Disabled(disabled);
            return this;
        }

        /// <inheritdoc />
        public IFieldConfiguration Readonly(bool @readonly = true)
        {
            Attributes.Readonly(@readonly);
            return this;
        }

        /// <inheritdoc />
        public IFieldConfiguration Placeholder(string placeholderText)
        {
            Attr("placeholder", placeholderText);
            return this;
        }

        /// <inheritdoc />
        public IHtmlContent InlineLabelText { get; private set; }

        /// <inheritdoc />
        public IFieldConfiguration InlineLabel(string labelText)
        {
            InlineLabelText = labelText.ToHtml();
            return this;
        }

        /// <inheritdoc />
        public IFieldConfiguration InlineLabel(IHtmlContent labelHtml)
        {
            InlineLabelText = labelHtml;
            return this;
        }

        /// <inheritdoc />
        public IHtmlContent LabelText { get; private set; }

        /// <inheritdoc />
        public IFieldConfiguration Label(string labelText)
        {
            LabelText = labelText.ToHtml();
            return this;
        }

        /// <inheritdoc />
        public IFieldConfiguration Label(IHtmlContent labelHtml)
        {
            LabelText = labelHtml;
            return this;
        }

        /// <inheritdoc />
        public FieldDisplayType DisplayType { get; private set; }

        /// <inheritdoc />
        public IFieldConfiguration AsRadioList()
        {
            DisplayType = FieldDisplayType.List;
            return this;
        }

        /// <inheritdoc />
        public IFieldConfiguration AsCheckboxList()
        {
            return AsRadioList();
        }

        /// <inheritdoc />
        public IFieldConfiguration AsDropDown()
        {
            DisplayType = FieldDisplayType.DropDown;
            return this;
        }

        /// <inheritdoc />
        public string TrueString { get; private set; }
        /// <inheritdoc />
        public IFieldConfiguration WithTrueAs(string trueString)
        {
            TrueString = trueString;
            return this;
        }

        /// <inheritdoc />
        public string FalseString { get; private set; }
        /// <inheritdoc />
        public IFieldConfiguration WithFalseAs(string falseString)
        {
            FalseString = falseString;
            return this;
        }

        /// <inheritdoc />
        public string NoneString { get; private set; }
        /// <inheritdoc />
        public IFieldConfiguration WithNoneAs(string noneString)
        {
            NoneString = noneString;
            return this;
        }

        /// <inheritdoc />
        public void SetField(IHtmlContent field)
        {
            _field = () => field;
        }

        /// <inheritdoc />
        public IFieldConfiguration WithHint(string hint)
        {
            Hint = hint.ToHtml();
            return this;
        }

        /// <inheritdoc />
        public IFieldConfiguration WithHint(IHtmlContent hint)
        {
            Hint = hint;
            return this;
        }

        /// <inheritdoc />
        public IHtmlContent Hint { get; private set; }

        /// <inheritdoc />
        public IFieldConfiguration Prepend(IHtmlContent html)
        {
            _prependedHtml.Add(html);
            return this;
        }

        /// <inheritdoc />
        public IFieldConfiguration Prepend(string str)
        {
            _prependedHtml.Add(str.ToHtml());
            return this;
        }

        /// <inheritdoc />
        public IEnumerable<IHtmlContent> PrependedHtml { get { var html = _prependedHtml.ToArray(); Array.Reverse(html); return html; } }

        /// <inheritdoc />
        public IFieldConfiguration Append(IHtmlContent html)
        {
            _appendedHtml.Add(html);
            return this;
        }

        /// <inheritdoc />
        public IFieldConfiguration Append(string str)
        {
            _appendedHtml.Add(str.ToHtml());
            return this;
        }

        /// <inheritdoc />
        public IEnumerable<IHtmlContent> AppendedHtml { get { return _appendedHtml.ToArray(); } }

        /// <inheritdoc />
        public IFieldConfiguration WithFormatString(string formatString)
        {
            FormatString = formatString;
            return this;
        }

        /// <inheritdoc />
        public string FormatString { get; private set; }

        /// <inheritdoc />
        public IFieldConfiguration HideEmptyItem()
        {
            EmptyItemHidden = true;
            return this;
        }

        /// <inheritdoc />
        public bool EmptyItemHidden { get; private set; }

        /// <inheritdoc />
        public IFieldConfiguration WithoutLabelElement()
        {
            HasLabelElement = false;
            return this;
        }

        /// <inheritdoc />
        public bool HasLabelElement { get; private set; }

        /// <inheritdoc />
        public IFieldConfiguration AddLabelClass(string @class)
        {
            if (!string.IsNullOrEmpty(LabelClasses))
                LabelClasses += " ";
            LabelClasses += @class;
            return this;
        }

        /// <inheritdoc />
        public string LabelClasses { get; private set; }

        /// <inheritdoc />
        public IFieldConfiguration AddFieldContainerClass(string @class)
        {
            if (!string.IsNullOrEmpty(FieldContainerClasses))
                FieldContainerClasses += " ";
            FieldContainerClasses += @class;
            return this;
        }

        /// <inheritdoc />
        public string FieldContainerClasses { get; private set; }

        /// <inheritdoc />
        public IFieldConfiguration AddValidationClass(string @class)
        {
            if (!string.IsNullOrEmpty(ValidationClasses))
                ValidationClasses += " ";
            ValidationClasses += @class;
            return this;
        }

        /// <inheritdoc />
        public string ValidationClasses { get; private set; }

        /// <inheritdoc />
        public IFieldConfiguration OverrideFieldHtml(IHtmlContent html)
        {
            _fieldHtml = html;
            return this;
        }

        /// <inheritdoc />
        public IHtmlContent FieldHtml { get { return _fieldHtml; } }

        /// <inheritdoc />
        public void SetField(Func<IHtmlContent> field)
        {
            _field = field;
        }

        /// <inheritdoc />
        public void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            var field = _field();

            if (field != null)
            {
                field.WriteTo(writer, encoder);
            }
        }

        /// <inheritdoc />
        public Enum[] ExcludedEnums { get; private set; }

        /// <inheritdoc />
        public IFieldConfiguration Exclude(params Enum[] enumValues)
        {
            ExcludedEnums = enumValues;
            return this;
        }

        /// <inheritdoc />
        public IFieldConfiguration WithoutInlineLabel()
        {
            HasInlineLabel = false;
            return this;
        }

        /// <inheritdoc />
        public IFieldConfiguration InlineLabelWrapsElement(bool wrapElement = true)
        {
            ShouldInlineLabelWrapElement = wrapElement;
            return this;
        }

        /// <inheritdoc />
        public bool HasInlineLabel { get; private set; }

        /// <inheritdoc />
        public bool ShouldInlineLabelWrapElement { get; private set; }
    }
}
