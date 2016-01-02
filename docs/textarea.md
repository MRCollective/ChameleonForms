Textarea Fields
===============

If you need to collect multi-line text data using a textarea then you can use the `[DataType]` attribute in `System.ComponentModel.DataAnnotations` to annotate that a string model property is in fact multi-line text, e.g.:

```c#
[DataType(DataType.MultilineText)]
public string TextareaField { get; set; }
```

Default HTML
------------

When using the Default Field Generator then the default HTML of the [Field Element](field-element) will be:

```html
<textarea %validationAttrs% %htmlAttributes% id="%propertyName%" name="%propertyName%">%value%</textarea>
```

Configurability
---------------

### Specify rows and columns

You can easily specify the required `rows` and `cols` HTML attributes by using the `Rows` and `Cols` methods on the [Field Configuration](field-configuration), e.g.:

```c#
@s.FieldFor(m => m.TextareaField).Rows(5).Cols(60)
```