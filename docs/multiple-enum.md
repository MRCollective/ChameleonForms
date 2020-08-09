# Multiple-Select Enum Fields

If you want the user to specify multiple values from an enum you can either use a [flags enum](flags-enum.md) (separate page) or use a non-flags enum against any property with a type convertible to `IEnumerable<%enumType%>` (this page), e.g.:

```cs
public enum MyEnum  { ... }
...
public IEnumerable<MyEnum> EnumEnumerableField { get; set; }
public List<MyEnum> EnumListField { get; set; }
public MyEnum[] EnumArrayField { get; set; }
// Or, alternatively:
public IEnumerable<MyEnum?> NullableEnumEnumerableField { get; set; }
public List<MyEnum?> NullableEnumListField { get; set; }
public MyEnum?[] NullableEnumArrayField { get; set; }
```

**Note:** as you will see below - there isn't much point in specifying a nullable enum for the enum type in the enumerable/list - we recommend you always use the enum type directly.

If you want the user to specify a single value only from an enum then you can [use the enum type directly](enum.md) without the collection modifier.

## Default HTML

### Required nullable or non-nullable enum (multi-select drop-down with no empty option)

To indicate that a value is required, regardless of whether you use the enum type or nullable variant of the enum type, you need to mark the field with `[Required]`.

```html
<select %validationAttrs% %htmlAttributes% multiple="multiple" id="%propertyName%" name="%propertyName%" required="required">
%foreach enum value x%
    <option value="%x.ToString()%">%x.Humanize()%</option>
%endforeach%
</select>
```

### Non-Required nullable or non-nullable enum (multi-select drop-down with empty option)

```html
<select %validationAttrs% %htmlAttributes% multiple="multiple" id="%propertyName%" name="%propertyName%">
    <option selected="selected" value="">%noneDescription%</option>
%foreach enum value x%
    <option value="%x.ToString()%">%x.Humanize()%</option>
%endforeach%
</select>
```

### Explanation and example

Please see the explanation an example on the [Enum Field](enum.md#explanation-and-example) page to understand what `%x.ToString()` and `%x.Humanize()%` do and how to control the description values.

## Configurability

### Display as list of checkboxes

You can force a list of enums field to display as a list of checkboxes rather than a multi-select drop-down using the `AsCheckboxList` method on the Field Configuration, e.g.:

# [Tag Helpers variant](#tab/checkbox-list-th)

The `AsCheckboxList` method is [mapped](./field-configuration.md#mapped-attributes) to `as="CheckboxList"`.

```cshtml
<field for="EnumListField" as="CheckboxList" />
```

# [HTML Helpers variant](#tab/checkbox-list-hh)

```cshtml
@s.FieldFor(m => m.EnumListField).AsCheckboxList()
```

***


This will change the default HTML for a both Required and non-Required list of enums (both nullable and non-nullable) fields as shown above to:

```html
<ul>
%foreach enum value x with increment i %
    <li><input %validationAttrs% %htmlAttributes% id="%propertyName%_%i%" name="%propertyName%" type="checkbox" value="%x.ToString()%" /> <label for="%propertyName%_%i%">%x.Humanize()%</label></li>
%endforeach%
</ul>
```

### Change the text description of none

When you display a non-Required list of enums field (nullable or non-nulable) as a drop-down you can change the text that is used to display the `none` value to the user. By default the text used is `None`. To change the text simply use the `WithNoneAs` method, e.g.:

# [Tag Helpers variant](#tab/none-label-th)

The `WithNoneAs` method is [mapped](./field-configuration.md#mapped-attributes) to `none-label="{label}"`.

```cshtml
<field for="NullableEnumListField" none-label="No value" />
```

# [HTML Helpers variant](#tab/none-label-hh)

```cshtml
@s.FieldFor(m => m.NullableEnumListField).WithNoneAs("No value")
```

***

This will change the default HTML for the non-Required drop-down list of enum field as shown above to:

```html
<select %validationAttrs% %htmlAttributes% multiple="multiple" id="%propertyName%" name="%propertyName%">
    <option selected="selected" value="">No value</option>
    @* Enum values as <options>... *@
</select>
```

### Hide empty item
If you have a non-Required list of enums field then it will show the empty item and this item will be selected by default if no values are selected. If for some reason you want a non-Required list of enums field, but you would also like to hide the empty item you can do so with the `HideEmptyItem` method in the Field Configuration, e.g.:

# [Tag Helpers variant](#tab/xxx-th)

The `HideEmptyItem` method is [mapped](./field-configuration.md#mapped-attributes) to `hide-empty-item="true"`.

```cshtml
<field for="NullableEnumListField" hide-empty-item="true" />
```

# [HTML Helpers variant](#tab/xxx-hh)

```cshtml
@s.FieldFor(m => m.NullableEnumListField).HideEmptyItem()
```

***


This will change the default HTML for the non-Required drop-down list of enum field as shown above to:

```html
<select %validationAttrs% %htmlAttributes% multiple="multiple" id="%propertyName%" name="%propertyName%">
    @* Enum values as <options>... *@
</select>
```
