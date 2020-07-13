# Password Fields

If you need to collect a password then you can use the `[DataType]` attribute in `System.ComponentModel.DataAnnotations` to annotate that a string model property is in fact a password, e.g.:

```csharp
[DataType(DataType.Password)]
public string PasswordField { get; set; }
```

## Default HTML

When using the Default Field Generator then the default HTML of the [Field Element](field-element.md) will be:

```html
<input %validationAttrs% %htmlAttributes% id="%propertyName%" name="%propertyName%" type="password" value="%value%" />
```
