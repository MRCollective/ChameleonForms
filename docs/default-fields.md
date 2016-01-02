Default (Text) Fields
=====================

If you need to collect some sort of primitive value from the user such as a string, number, email, etc. then you can use any value type (nullable or not nullable), e.g.:

```c#
public string StringField { get; set; }
public int IntField { get; set; }
public int? NullableIntField { get; set; }
```

A non-nullable type will always be Required and a nullable type can be made to be Required by annotating with the `[Required]` attribute (but allows you to start off with a blank value when using `null` so you don't bias the user's input).

Default HTML
------------

When using the Default Field Generator then the default HTML of the [Field Element](field-element) will be:

```html
<input %validationAttrs% %htmlAttributes% id="%propertyName%" name="%propertyName%" type="text" value="%value%" />
```