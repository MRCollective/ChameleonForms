Default (Text) Fields
=====================

The default field will output a field when no other field type worked. It allows the user to enter a simple string input for the value of that field.

```csharp
public string StringField { get; set; }
```

A non-nullable type will always be Required and a nullable type can be made to be Required by annotating with the `[Required]` attribute (but allows you to start off with a blank value when using `null` so you don't bias the user's input).

Default HTML
------------

When using the Default Field Generator then the default HTML of the [Field Element](field-element.md) will be:

```html
<input %validationAttrs% %htmlAttributes% id="%propertyName%" name="%propertyName%" type="text" value="%value%" />
```

If the field is required then the default HTML will be:

```html
<input %validationAttrs% %htmlAttributes% id="%propertyName%" name="%propertyName%" required="required" type="text" value="%value%" />
```
