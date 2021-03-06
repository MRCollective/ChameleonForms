# Enum Fields

If you want the user to specify a value from an enum you can use that enum type (or a nullable instance of it) against a model property, e.g.:

```cs
public enum MyEnum  { ... }
...
public MyEnum EnumField { get; set; } // automatically required since it's non-nullable
[Required]
public MyEnum? RequiredNullableEnumField { get; set; } // Required, but can start off as an empty value
public MyEnum? NullableEnumField { get; set; } // Not required
```

If you want the user to select multiple enum values you can either use a [flags enum](flags-enum.md) or a [list of enums](multiple-enum.md).

## Default HTML

### Non-nullable enum (drop-down with no empty option)

When using the Default Field Generator then the default HTML of the [Field Element](field-element.md) for a non-nullable enum will be:

```html
<select %validationAttrs% %htmlAttributes% id="%propertyName%" name="%propertyName%" required="required">
%foreach enum value x%
    <option value="%x.ToString()%">%x.Humanize()%</option>
%endforeach%
</select>
```

Note: See below to understand what the effect of `.ToString()` and `.Humanize()` are.

### Nullable enum (drop-down with empty option)

```html
<select %validationAttrs% %htmlAttributes% id="%propertyName%" name="%propertyName%">
    <option selected="selected" value="">%noneDescription%</option>
%foreach enum value x%
    <option value="%x.ToString()%">%x.Humanize()%</option>
%endforeach%
</select>
```

If the field is marked `[Required]` there will also be a `required="required"` added.

### Explanation and example

`x.ToString` means the string representation of the enum value and `x.Humanize()` means converting the enum value to a human-readable string using the awesome [Humanizer](https://github.com/MehdiK/Humanizer#humanize-enums) library. This will automatically convert camel-cased enum values to sentence case and pick up any usage of `[Description]`, `[Display]`, etc.. Read the [Humanizer documentation](https://github.com/MehdiK/Humanizer#humanize-enums) for more information including how to perform localisation.

As an example, if you had the following enum:

```cs
public enum AnEnum
{
    Singleword,
    MultipleWords,
    [Description("Custom-description!")]
    CustomDescription
}
```

And you had a property on your model like:

```cs
public AnEnum EnumValue { get; set; }
```

Then by default the Field Element HTML would be (if labels are [automatically sentence cased](labels.md) and you don't specify any extra [HTML attributes](html-attributes.md)):

```html
<select data-val="true" data-val-required="The Enum value field is required." id="EnumValue" name="EnumValue" required="required">
    <option value="Singleword">Singleword</option>
    <option value="MultipleWords">Multiple words</option>
    <option value="CustomDescription">Custom-description!</option>
</select>
```

## Configurability

### Display as list of radio buttons

You can force an enum field to display as a list of radio buttons rather than a drop-down using the `AsRadioList` method on the Field Configuration, e.g.:

# [Tag Helpers variant](#tab/radiolist-th)

The `AsRadioList` method is [mapped](./field-configuration.md#mapped-attributes) to `as="RadioList"`.

```cshtml
<field for="EnumField" as="RadioList" />
<field for="NullableEnumField" as="RadioList" />
```

# [HTML Helpers variant](#tab/radiolist-hh)

```cshtml
@s.FieldFor(m => m.EnumField).AsRadioList()
@s.FieldFor(m => m.NullableEnumField).AsRadioList()
```

***

This will change the default HTML for the non-nullable enum field and the Required nullable enum field as shown above to:

```html
<ul>
%foreach enum value x with increment i %
    <li><input %validationAttrs% %htmlAttributes% id="%propertyName%_%i%" name="%propertyName%" required="required" type="radio" value="%x.ToString()%" /> <label for="%propertyName%_%i%">%x.Humanize()%</label></li>
%endforeach%
</ul>
```

And it will change the default HTML for the non-Required nullable enum field as shown above to:

```html
<ul>
    <li><input checked="checked" %validationAttrs% %htmlAttributes% id="%propertyName%_1" name="%propertyName%" type="radio" value="" /> <label for="%propertyName%_1">%noneDescription%</label></li>
%foreach enum value x with increment i%
    <li><input %htmlAttributes% id="%propertyName%_%i+1%" name="%propertyName%" type="radio" value="%x.ToString()%" /> <label for="%propertyName%_%i+1%">%x.Humanize()%</label></li>
%endforeach%
</ul>
```

### Change the text description of none

When you display a nullable enum field as a drop-down or a non-Required nullable enum field as a list of radio buttons you can change the text that is used to display the `none` value to the user. By default the text used is an empty string for the drop-down and `None` for the radio button. To change the text simply use the `WithNoneAs` method, e.g.:

# [Tag Helpers variant](#tab/none-th)

The `WithNoneAs` method is [mapped](./field-configuration.md#mapped-attributes) to `none-label="{label}"`.

```cshtml
<field for="NullableEnumField" none-label="No value" />
```

# [HTML Helpers variant](#tab/none-hh)

```cshtml
@s.FieldFor(m => m.NullableEnumField).WithNoneAs("No value")
```

***

This will change the default HTML for the nullable enum field as shown above to:

```html
<select %validationAttrs% %htmlAttributes% id="%propertyName%" name="%propertyName%">
    <option selected="selected" value="">No value</option>
    @* Enum values as <options>... *@
</select>
```

### Hide empty item
If you have a nullable enum field then it will show the empty item and this item will be selected by default if the field value is null. If for some reason you want a nullable enum, but you would also like to hide the empty item you can do so with the `HideEmptyItem` method in the Field Configuration, e.g.:

# [Tag Helpers variant](#tab/hide-empty-th)

```cshtml
<field for="NullableEnumField" hide-empty-item="true" />
```

# [HTML Helpers variant](#tab/hide-empty-hh)

```cshtml
@s.FieldFor(m => m.NullableEnumField).HideEmptyItem()
```

***

This will change the default HTML for the nullable enum field as shown above to:

```html
<select %validationAttrs% %htmlAttributes% id="%propertyName%" name="%propertyName%">
    @* Enum values as <options>... *@
</select>
```

### Exclude specific enum values
If there are some enum values you want to exclude from showing up as options then you can do so with the `Exclude` method in the Field Configuration, e.g.:

# [Tag Helpers variant](#tab/exclude-th)

Unfortunately, you can't have generic typing in the tag helper, so you need to cast the array of enums to `Enum[]`.

```cshtml
<field for="EnumField"  exclude="new Enum[]{MyEnum.Value1, MyEnum.Value3}" />
```

# [HTML Helpers variant](#tab/exclude-hh)

```cshtml
@s.FieldFor(m => m.EnumField).Exclude(MyEnum.Value1, MyEnum.Value3)
```

***
