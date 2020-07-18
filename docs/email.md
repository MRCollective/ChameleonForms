# Email Fields

If you need to collect emails then that will automatically be handled for you with a HTML5 `<input type="email">` field if you annotate your model field correctly:

```cs
[DataType(DataType.EmailAddress)]
public string Email { get; set; }
[EmailAddress]
public string Email { get; set; }
```

## Validation

If you just apply the `[DataType]` attribute it won't perform any server-side validation, but it will apply client-side validation attributes. If you apply the `[EmailAddress]` attribute it will also perform basic [server-side validation](https://github.com/dotnet/runtime/blob/master/src/libraries/System.ComponentModel.Annotations/src/System/ComponentModel/DataAnnotations/EmailAddressAttribute.cs). Fun-fact: the server-side validation used to be [more comprehensive](https://github.com/dotnet/corefx/commit/070e282397b21450f80a20028c5e5eff10ec46a4), but was simplified [due to a security vulnerability](https://blog.malerisch.net/2015/09/net-mvc-redos-denial-of-service-vulnerability-cve-2015-2526.html). At the end of the day, the best way to verify an email address (if you really need to) is to [send an email to it](https://medium.com/hackernoon/the-100-correct-way-to-validate-email-addresses-7c4818f24643).

## Default HTML

When using the Default Field Generator then the default HTML of the [Field Element](field-element.md) will be:

```html
<input %validationAttrs% %htmlAttributes% id="%propertyName%" name="%propertyName%" type="email" value="%value%" />
```

If the field is required then the default HTML will be:

```html
<input %validationAttrs% %htmlAttributes% id="%propertyName%" name="%propertyName%" required="required" type="email" value="%value%" />
```
