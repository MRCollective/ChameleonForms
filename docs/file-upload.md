File Upload Fields
==================

If you need to collect uploaded file data then you can use ASP.NET MVC's `HttpPostedFileBase` type for your model property, e.g.:

```csharp
public HttpPostedFileBase FileUpload { get; set; }
```

In order for file uploads to work you will need to set the encoding type on the form to `multipart/form-data` ([as opposed to the default](http://stackoverflow.com/questions/4526273/what-does-enctype-multipart-form-data-mean)), e.g.:

```csharp
@using (var f = Html.BeginChameleonForm(encType: EncType.Multipart)) {
    @* ... *@
}
```

To then use the file upload in your controller action you use the [standard approaches](http://askjonskeet.com/answer/7852256/Convert-HttpPostedFileBase-to-byte). If you want the field to be Required you can annotate with `[Required]` as normal and check for the property value being null to see if something was submitted by the user.

Default HTML
------------

When using the Default Field Generator then the default HTML of the [Field Element](field-element) will be:

```html
<input %validationAttrs% %htmlAttributes% id="%propertyName%" name="%propertyName%" type="file" value="%value%" />
```