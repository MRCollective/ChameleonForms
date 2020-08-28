# URI Fields

If you need to collect URI/URL data then that will automatically be handled for you with a HTML5 `<input type="url">` field.

Any one of the following model types will trigger one of these fields:

```cs
public Uri Uri { get; set; }

[DataType(DataType.Url)]
public string Url { get; set; }

[Url]
public string Url { get; set; }
```

With the [default global configuration](configuration.md), using a `Uri` will also result in server-side validation - the value will get parsed by the `Uri` class to check validity. The `[Url]` against a `string` will also result in (rudimentary) validation (starts with `http://`, `https://` or `ftp://`).

## Default HTML

```html
<input %validationAttrs% %htmlAttributes% id="%propertyName%" name="%propertyName%" type="url" value="%value%" />
```

## Configurability

### Specify URL

You can specify your property as a URL value, which will automatically validate server-side for a valid HTTP or HTTPS URL on the server-side (if using `Uri` with a `[DataType(DataType.Url)]` attribute) or starting with ftp://, http:// or https:// (if using `string` with `[Url]`):

```cs
[DataType(DataType.Url)]
public Uri Url { get; set; }

[Url]
public string Url { get; set; }
```

Note: if you specify the `[Url]` attribute with a `Uri` type it will always fail server-side validation because the built-in `[Url]` attribute expects a string.
