# Textarea Fields

If you need to collect multi-line text data using a textarea then you can use the `[DataType]` attribute in `System.ComponentModel.DataAnnotations` to annotate that a string model property is in fact multi-line text, e.g.:

```cs
[DataType(DataType.MultilineText)]
public string TextareaField { get; set; }
```

## Default HTML

When using the Default Field Generator then the default HTML of the [Field Element](field-element.md) will be:

```html
<textarea %validationAttrs% %htmlAttributes% id="%propertyName%" name="%propertyName%">%value%</textarea>
```

## Configurability

### Specify rows and columns

You can easily specify the required `rows` and `cols` HTML attributes by using the `Rows` and `Cols` methods on the [Field Configuration](field-configuration.md), e.g.:

# [Tag Helpers variant](#tab/rows-cols-th)

```cshtml
<field for="TextareaField" rows="5" cols="60" />
@* or *@
<field for="TextareaField" fluent-config='c => c.Rows(5).Cols(60)' />
```

# [HTML Helpers variant](#tab/rows-cols-hh)

```cshtml
@s.FieldFor(m => m.TextareaField).Rows(5).Cols(60)
```

***
