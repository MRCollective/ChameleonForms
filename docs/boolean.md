Boolean Fields
==============

If you need to collect Boolean data you can use a `bool` or `bool?` model property, e.g.:

```csharp
    public bool BooleanField { get; set; }
    public bool? NullableBooleanField { get; set; }
```

Default HTML
------------

When using the Default Field Generator then the default HTML of the [Field Element](field-element) will be:

### Non-nullable Boolean (checkbox)

This field will always be Required since it's not nullable.

```html
<input %validationAttrs% %htmlAttributes% id="%propertyName%" name="%propertyName%" type="checkbox" value="true" /> <label for="%propertyName%">%inlineLabel%</label>
```

### Nullable Boolean (drop-down with empty option)

If the field is Required then the empty option will still show and it will be selected if the value of the property in the model is null, but it will trigger a validation error if the user selects it.

```html
<select %validationAttrs% %htmlAttributes% id="%propertyName%" name="%propertyName%">
    <option selected="selected" value=""></option>
    <option value="true">Yes</option>
    <option value="false">No</option>
</select>
```

Configurability
---------------

### Inline label

If you are outputting a non-nullable Boolean then a label will show next to the field as part of the Field Element. If you want to override just the label text for this label (and not the Field Label as well) then you can do so using the `InlineLabel` method in the Field Configuration, e.g.:

```csharp
@s.FieldFor(m => m.BooleanField).InlineLabel("override")
```

### Display as drop-down

You can force a Boolean field to display as a drop-down box rather than a checkbox using the `AsDropDown` method on the Field Configuration, e.g.:

```csharp
@s.FieldFor(m => m.BooleanField).AsDropDown()
```

This will change the default HTML for the non-nullable Boolean field as shown above to:

```html
<select %validationAttrs% %htmlAttributes% id="%propertyName%" name="%propertyName%">
    <option value="true">Yes</option>
    <option value="false">No</option>
</select>
```

### Display as list of radio buttons

You can force a Boolean field to display as a list of radio buttons rather than a checkbox using the `AsRadioList` method on the Field Configuration, e.g.:

```csharp
@s.FieldFor(m => m.BooleanField).AsRadioList()
@s.FieldFor(m => m.NullableBooleanField).AsRadioList()
```

And it will change the default HTML for the non-nullable Boolean field and the Required nullable Boolean field as shown above to:

```html
<ul>
    <li><input %validationAttrs% %htmlAttributes% id="%propertyName%_1" name="%propertyName" type="radio" value="true" /> <label for="%propertyName%_1">%trueDescription%</label></li>
    <li><input %htmlAttributes% checked="checked" id="%propertyName%_2" name="%propertyName%" type="radio" value="false" /> <label for="%propertyName%_2">%falseDescription%</label></li>
</ul>
```

And it will change the default HTML for the non-Required nullable Boolean field as shown above to:

```html
<ul>
    <li><input %validationAttrs% %htmlAttributes% checked="checked" id="%propertyName%_1" name="%propertyName" type="radio" value="" /> <label for="%propertyName%_1">%noneDescription%</label></li>
    <li><input %htmlAttributes% id="%propertyName%_2" name="%propertyName%" type="radio" value="true" /> <label for="%propertyName%_2">%trueDescription%</label></li>
    <li><input %htmlAttributes% id="%propertyName%_3" name="%propertyName%" type="radio" value="false" /> <label for="%propertyName%_3">%falseDescription%</label></li>
</ul>
```

### Change the text descriptions of true, false and none

When you display a Boolean field as a drop-down or a list of radio buttons you can change the text that is used to display the `true`, `false` and `none` values to the user. By default the text used is `Yes`, `No` and `None` (except for drop-downs, which have an empty string) respectively. To change the text simply use the `WithTrueAs`, `WithFalseAs` and `WithNoneAs` methods respectively, e.g.:

```csharp
@s.FieldFor(m => m.NullableBooleanField).WithTrueAs("OK").WithFalseAs("Not OK").WithNoneAs("No comment")
```

This will change the default HTML for the nullable Boolean field as shown above to:

```html
<select %validationAttrs% %htmlAttributes% id="%propertyName%" name="%propertyName%">
    <option selected="selected" value="">No comment</option>
    <option value="true">OK</option>
    <option value="false">Not OK</option>
</select>
```

### Hide empty item
If you have a nullable Boolean field then it will show the empty item and this item will be selected by default if the field value is null. If for some reason you want a nullable boolean, but you would also like to hide the empty item you can do so with the `HideEmptyItem` method in the Field Configuration, e.g.:

```csharp
@s.FieldFor(m => m.NullableBooleanField).HideEmptyItem()
```

This will change the default HTML for the nullable Boolean field as shown above to:

```html
<select %validationAttrs% %htmlAttributes% id="%propertyName%" name="%propertyName%">
    <option value="true">Yes</option>
    <option value="false">No</option>
</select>
```

### Hide inline label
If you would like to output just a checkbox for a non-nullable boolean field without an inline label next to it you can do so with the `WithoutInlineLabel` method in the Field Configuration, e.g.:

```csharp
@s.FieldFor(m => m.BooleanField).WithoutInlineLabel()
```

This will change the default HTML for the non-nullable Boolean field as shown above to:

```html
<input %validationAttrs% %htmlAttributes% id="%propertyName%" name="%propertyName%" type="checkbox" value="true" />
```

### Wrap input with label
If you would like to wrap the checkbox with the label you can do so with the `InlineLabelWrapsElement` method in the Field Configuration, e.g.:

```csharp
@s.FieldFor(m => m.BooleanField).InlineLabelWrapsElement()
```

This will change the default HTML for the field as shown above to:

```html
<label><input %validationAttrs% %htmlAttributes% id="%propertyName%" name="%propertyName%" type="checkbox" value="true" /> %inlineLabel%</label>
```