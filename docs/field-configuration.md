# Field Configuration

Field Configuration provides the ability to configure a [Field](field.md) (and its sub-components) on both an ad-hoc basis within a particular form and a convention basis across all forms. Specifying a Field Configuration is done by chaining calls to the methods on the `IFieldConfiguration` interface and/or calling the mapped field configuration attributes when using tag helpers.

The `IFieldConfiguration` interface is translated to an `IReadonlyFieldConfiguration` just before it's passed to the template to make sure that modifications can't be made to it after it's processed by the template.

The `IFieldConfiguration` interface looks like this and is in the `ChameleonForms.Component.Config` namespace:

```cs
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
        /// Sets the minimum value to accept for numeric text controls.
        /// </summary>
        /// <param name="min">The minimum value to accept</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Min(long min);

        /// <summary>
        /// Sets the maximum value to accept for numeric text controls.
        /// </summary>
        /// <param name="max">The maximum value to accept</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Max(long max);

        /// <summary>
        /// Sets the minimum value to accept for numeric text controls.
        /// </summary>
        /// <param name="min">The minimum value to accept</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Min(decimal min);

        /// <summary>
        /// Sets the maximum value to accept for numeric text controls.
        /// </summary>
        /// <param name="max">The maximum value to accept</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Max(decimal max);

        /// <summary>
        /// Sets the minimum value to accept for numeric text controls.
        /// </summary>
        /// <param name="min">The minimum value to accept</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Min(string min);

        /// <summary>
        /// Sets the maximum value to accept for numeric text controls.
        /// </summary>
        /// <param name="max">The maximum value to accept</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Max(string max);

        /// <summary>
        /// Sets the stepping interval to use for numeric text controls.
        /// </summary>
        /// <param name="step">The stepping interval</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Step(decimal step);

        /// <summary>
        /// Sets the stepping interval to use for numeric text controls.
        /// </summary>
        /// <param name="step">The stepping interval</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Step(long step);

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
        /// Sets the field to be required.
        /// </summary>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Required(bool required = true);

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
        /// Sets an inline label for a checkbox.
        /// </summary>
        /// <param name="labelHtml">The html to use for the label as a templated razor delegate</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration InlineLabel(Func<dynamic, IHtmlContent> labelHtml);

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
        /// Override the default label for the field.
        /// </summary>
        /// <param name="labelHtml">The text to use for the label as a templated razor delegate</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Label(Func<dynamic, IHtmlContent> labelHtml);

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
        /// Sets the field that the field configuration is wrapping so that
        ///     a call to ToHtmlString() will output the given field.
        /// </summary>
        /// <param name="field">The field being configured as a templated razor delegate</param>
        void SetField(Func<dynamic, IHtmlContent> field);

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
        /// Supply a HTML hint to display along with the field.
        /// </summary>
        /// <param name="hint">The hint markup as a templated razor delegate</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration WithHint(Func<dynamic, IHtmlContent> hint);
        
        /// <summary>
        /// Prepends the given HTML to the form field.
        /// </summary>
        /// <param name="html">The HTML to prepend</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Prepend(IHtmlContent html);
        
        /// <summary>
        /// Prepends the given HTML to the form field.
        /// </summary>
        /// <param name="html">The HTML to prepend as a templated razor delegate</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Prepend(Func<dynamic, IHtmlContent> html);

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
        /// Appends the given HTML to the form field.
        /// </summary>
        /// <param name="html">The HTML to append as a templated razor delegate</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration Append(Func<dynamic, IHtmlContent> html);

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
        /// Override the HTML of the form field.
        /// 
        /// This gives you ultimate flexibility with your field HTML when it's
        /// not quite what you want, but you still want the form template
        /// (e.g. label, surrounding html and validation message).
        /// </summary>
        /// <param name="html">The HTML for the field as a templated razor delegate</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration OverrideFieldHtml(Func<dynamic, IHtmlContent> html);
                
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

        /// <summary>
        /// Specify an ID to use for a field hint.
        /// </summary>
        /// <param name="hintId">The ID to use</param>
        /// <returns>The <see cref="IFieldConfiguration"/> to allow for method chaining</returns>
        IFieldConfiguration WithHintId(string hintId);
    }
```

The `IReadonlyFieldConfiguration` interface can be created by calling the `ToReadonly()` method on the `IFieldConfiguration`; it is in the `ChameleonForms.Component.Config` namespace and looks like this:

```cs
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
        /// Returns data from the Bag stored in the given property or default(TData) if there is none present.
        /// </summary>
        /// <typeparam name="TData">The type of the expected data to return</typeparam>
        /// <param name="propertyName">The name of the property to retrieve the data for</param>
        /// <returns>The data from the Bag or default(TData) if there was no data against that property in the bag</returns>
        TData GetBagData<TData>(string propertyName);

        /// <summary>
        /// Attributes to add to the form element's HTML.
        /// </summary>
        IDictionary<string, object> HtmlAttributes { get; }

        /// <summary>
        /// Gets any text that has been set for an inline label.
        /// </summary>
        IHtmlContent InlineLabelText { get; }

        /// <summary>
        /// Gets any text that has been set for the label.
        /// </summary>
        IHtmlContent LabelText { get; }

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
        IHtmlContent Hint { get; }

        /// <summary>
        /// A list of HTML to be prepended to the form field in ltr order.
        /// </summary>
        IEnumerable<IHtmlContent> PrependedHtml { get; }

        /// <summary>
        /// A list of HTML to be appended to the form field in ltr order.
        /// </summary>
        IEnumerable<IHtmlContent> AppendedHtml { get; }

        /// <summary>
        /// The HTML to be used as the field html.
        /// </summary>
        IHtmlContent FieldHtml { get; }

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
        bool HasLabelElement { get; }

        /// <summary>
        /// Any CSS class(es) to use for the field label.
        /// </summary>
        string LabelClasses { get; }

        /// <summary>
        /// Any CSS class(es) to use for the field container element.
        /// </summary>
        string FieldContainerClasses { get; }

        /// <summary>
        /// Any CSS class(es) to use for the field validation message.
        /// </summary>
        string ValidationClasses { get; }

        /// <summary>
        /// Enum value(s) to exclude from the generated field.
        /// </summary>
        Enum[] ExcludedEnums { get; }

        /// <summary>
        /// Whether or not to use an inline &lt;label&gt;.
        /// </summary>
        bool HasInlineLabel { get; }

        /// <summary>
        /// Whether or not inline &lt;label&gt; should wrap their &lt;input&gt; element.
        /// </summary>
        bool ShouldInlineLabelWrapElement { get; }

        /// <summary>
        /// The ID to use for a field hint.
        /// </summary>
        string HintId { get; }
    }
```

The xmldoc comments above should give a pretty good indication of how each of those methods are meant to be used. There is further documentation about the effect of the different methods in the following documentation:

* [Field](field.md)
* [Field Label](field-label.md)
* [Field Types](index.md#field-types)

## Tag Helper mappings

When using tag helpers there are two ways of specifying field configuration:

1. Use the `fluent-config` attribute and chain the field configuration method calls
2. Use individual attributes that are mapped to individual field configuration methods

### Fluent configuration

Note: We recommend that you make use of single quotes (`'`) rather than double quotes (`"`) so that you can use the double quotes in any field configuration methods that need a string.

```cshtml
<field for="..." fluent-config='c => c.AddClass("a-class").Append("after")...' />
<field-element for="..." fluent-config='c => c.AddClass("a-class").Min(2)...' />
<field-label for="..." fluent-config='c => c.Label("a-class").WithoutLabelElement()...' />
<field-validation for="..." fluent-config='c => c.AddValidationClass("a-class")...' />
```

### Mapped attributes

Any attributes that take string values can have a variable or other C# expression added by prepending with a `@` per usual Razor syntax. Most field configuration methods map to a tag helper attribute by convention - `UpperCamelCase` to `upper-camel-case` (i.e. kebab case), but a few are slightly different for clarity or terseness.

| Field Configuration Method                                                 | Equivalent Tag Helper attribute                       | Available On                       |
|----------------------------------------------------------------------------|-------------------------------------------------------|------------------------------------|
| `Id(string id)`                                       | `id="{id}"`                                           | `<field>` and `<field-element>`    |
| `AddClass(string @class)`                             | `add-class="{class}"`                                 | `<field>` and `<field-element>`    |
| `AddFieldContainerClass(string @class)`               | `add-container-class="{class}"`                       | `<field>`                          |
| `AddLabelClass(string @class)`                        | `add-label-class="{class}"`                           | `<field>` and `<field-label>`      |
| `AddValidationClass(string @class)`                   | `add-validation-class="{class}"`                      | `<field>` and `<field-validation>` |
| `AsRadioList()`                                       | `as="RadioList"`                                      | `<field>` and `<field-element>`    |
| `AsCheckboxList()`                                    | `as="CheckboxList"`                                   | `<field>` and `<field-element>`    |
| `AsDropDown()`                                        | `as="DropDown"`                                       | `<field>` and `<field-element>`    |
| `Append(IHtmlContent html)`                           | `append-html-content="{html}"`                        | `<field>`                          |
| `Append(Func<dynamic, IHtmlContent> html)`            | `append-html="{html}"`                                | `<field>`                          |
| `Append(string str)`                                  | `append="{str}"`                                      | `<field>`                          |
| `Attr(string key, object value)`                      | `attr-{key}="{value}"`                                | `<field>` and `<field-element>`    |
| `Attr(Func<object, object> attribute)`                | *No equivalent*                                       | N/A                                |
| `Attrs(params Func<object, object>[] attributes)`     | *No equivalent*                                       | N/A                                |
| `Attrs(IDictionary<string, object> attributes)`       | `attrs="{attributes}"`                                | `<field>` and `<field-element>`    |
| `Attrs(object attributes)`                            | *No equivalent*                                       | N/A                                |
| `Cols(int numCols)`                                   | `cols="{numCols}"`                                    | `<field>` and `<field-element>`    |
| `Disabled(bool disabled = true)`                      | `disabled="{disabled}"`                               | `<field>` and `<field-element>`    |
| `Exclude(params Enum[] enumValues)`                   | `exclude="new Enum[] {enumValues...}"`                | `<field>` and `<field-element>`    |
| `InlineLabel(string labelText)`                       | `inline-label="{labelText}"`                          | `<field>` and `<field-element>`    |
| `InlineLabel(IHtmlContent labelHtml)`                 | `inline-label-html-content="{labelHtml}"`             | `<field>` and `<field-element>`    |
| `InlineLabel(Func<dynamic, IHtmlContent> labelHtml)`  | `inline-label-html="{labelHtml}"`                     | `<field>` and `<field-element>`    |
| `InlineLabelWrapsElement(bool wrapElement = true)`    | `inline-label-wraps-element="{wrapElement}"`          | `<field>` and `<field-element>`    |
| `Label(string labelText)`                             | `label="{labelText}"`                                 | `<field>` and `<field-label>`      |
| `Label(IHtmlContent labelHtml)`                       | `label-html-content="{labelHtml}"`                    | `<field>` and `<field-label>`      |
| `Label(Func<dynamic, IHtmlContent> labelHtml)`        | `label-html="{labelHtml}"`                            | `<field>` and `<field-label>`      |
| `Min(decimal min)`                                    | `min="@min.ToString()"`                               | `<field>` and `<field-element>`    |
| `Min(long min)`                                       | `min="@min.ToString()"`                               | `<field>` and `<field-element>`    |
| `Min(string min)`                                     | `min="{min}"`                                         | `<field>` and `<field-element>`    |
| `Max(decimal max)`                                    | `max="@max.ToString()"`                               | `<field>` and `<field-element>`    |
| `Max(long max)`                                       | `max="@max.ToString()"`                               | `<field>` and `<field-element>`    |
| `Max(string max)`                                     | `max="{max}"`                                         | `<field>` and `<field-element>`    |
| `OverrideFieldHtml(IHtmlContent html)`                | `override-field-html-content="{html}"`                | `<field>`                          |
| `OverrideFieldHtml(Func<dynamic, IHtmlContent> html)` | `override-field-html="{html}"`                        | `<field>`                          |
| `Placeholder(string placeholderText)`                 | `placeholder="{placeholderText}"`                     | `<field>` and `<field-element>`    |
| `Prepend(IHtmlContent html)`                          | `prepend-html-content="{html}"`                       | `<field>`                          |
| `Prepend(Func<dynamic, IHtmlContent> html)`           | `prepend-html="{html}"`                               | `<field>`                          |
| `Prepend(string str)`                                 | `prepend="{str}"`                                     | `<field>`                          |
| `Readonly(bool @readonly = true)`                     | `readonly="{readonly}"`                               | `<field>` and `<field-element>`    |
| `Required(bool required = true)`                      | `required="{required}"`                               | `<field>` and `<field-element>`    |
| `Rows(int numRows)`                                   | `rows="{numRows}"`                                    | `<field>` and `<field-element>`    |
| `Step(decimal step)`                                  | `step="{step}"` (inline) or `step="@step.ToString()"` | `<field>` and `<field-element>`    |
| `Step(long step)`                                     | `step="{step}"` (inline) or `step="@step.ToString()"` | `<field>` and `<field-element>`    |
| `HideEmptyItem()`                                     | `hide-empty-item="true"`                              | `<field>` and `<field-element>`    |
| `WithFalseAs(string falseString)`                     | `false-label="{falseString}"`                         | `<field>` and `<field-element>`    |
| `WithFormatString(string formatString)`               | `format-string="{formatString}"`                      | `<field>` and `<field-element>`    |
| `WithHint(string hint)`                               | `hint="{hint}"`                                       | `<field>`                          |
| `WithHint(IHtmlContent hint)`                         | `hint-html-content="{hint}"`                          | `<field>`                          |
| `WithHint(Func<dynamic, IHtmlContent> hint)`          | `hint-html="{hint}"`                                  | `<field>`                          |
| `WithHintId(string hintId)`                           | `hint-id="{hintId}"`                                  | `<field>`                          |
| `WithNoneAs(string noneString)`                       | `none-label="{noneString}"`                           | `<field>` and `<field-element>`    |
| `WithoutInlineLabel()`                                | `without-inline-label="true"`                         | `<field>` and `<field-element>`    |
| `WithoutLabelElement()`                               | `without-label-element="true"`                        | `<field>` and `<field-label>`      |
| `WithTrueAs(string trueString)`                       | `true-label="{trueString}"`                           | `<field>` and `<field-element>`    |

## How does the IFieldConfiguration output the Field HTML for HTML Helper extension methods?

The astute viewer will notice that the various `FieldFor`, `FieldElementFor`, `LabelFor` and `ValidationMessageFor` HTML Helper extension methods all return an `IFieldConfiguration` as opposed to a `string` or `IHtmlContent`, yet when prefixed  with `@` in a razor view (with or without chaining any Field Configuration methods) will always output the correct HTML.

This works because:

* The `IFieldConfiguration` interface extends `IHtmlContent`, which forces it to implement the `.WriteTo(TextWriter writer, HtmlEncoder encoder)` method (which will be called by razor via the `@` operator)
* All the methods on `IFieldConfiguration` return the same instance of the `IFieldConfiguration` object so the `@` operator will apply to that Field Configuration regardless of what methods the user calls
* The `SetField(IHtmlContent)` method, `SetField(Func<dynamic, IHtmlContent>)` method (for [templated razor delegates](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/razor?view=aspnetcore-3.1#directive-attributes)) or the `SetField(Func<IHtmlContent>)` method will be called before returning the `IFieldConfiguration` to indicate what HTML should be output by the Field Configuration if the `.ToHtmlString()` method is called
* The `SetField` method approach allows for lazy evaluation of the HTML to output, meaning the HTML generation can occur after all of the `IFieldConfiguration` methods have been called (allowing the Field Configuration to be mutated before eventually being used)

## Passing HTML to field configuration methods

For all the field configuration methods that take an `IHtmlContent` you have a few options available to you:

* Pass the HTML as a string e.g. `.Label(new HtmlString("<strong>My label</strong>"))`
* Pass the HTML by calling any method that returns an `IHtmlContent`
* Use the override that takes a [templated razor delegate](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/razor?view=aspnetcore-3.1#directive-attributes), e.g.:


    # [Tag Helpers variant](#tab/templated-razor-delegate-example-th)

    Unfortunately, tag helpers don't support inline templated razor delegates so you have to pass it in via a variable.

    ```cshtml
        @{
            Func<dynamic, IHtmlContent> myLabel = @<strong>My label</strong>;
        }
        
        ...
        <field for="MyField" label="myLabel" />
        ...
    ```

    # [HTML Helpers variant](#tab/templated-razor-delegate-example-hh)
    
    ```cshtml
        @{
            Func<dynamic, IHtmlContent> myLabel = @<strong>My label</strong>;
        }
        
        ...
        @s.FieldFor(m => m.MyField).Label(myLabel)
        @s.FieldFor(m => m.MyOtherField).Label(@<strong>Inline templated razor delegate with single parent element</strong>)
        @s.FieldFor(m => m.MyOtherField).Label(@<text><strong>Inline</strong> templated razor delegate with no single parent element</text>)
        ...
    ```

    ***

The Tag Helper attributes have a convention where the version that takes a `IHtmlContent` will be appended with `-html-content` and the version that takes a templated razor delegate will be appended with `-html`.
