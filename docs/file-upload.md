# File Upload Fields

If you need to collect uploaded file data then you can use ASP.NET Core MVC's `IFormFile` type for your model property, e.g.:

```cs
public IFormFile FileUpload { get; set; }
```

In order for file uploads to work you will need to set the encoding type on the form to `multipart/form-data` ([as opposed to the default](http://stackoverflow.com/questions/4526273/what-does-enctype-multipart-form-data-mean)), e.g.:

```cshtml
@using (var f = Html.BeginChameleonForm(encType: EncType.Multipart)) {
    @* ... *@
}
```

To then use the file upload in your controller action you use the [documented approaches](https://docs.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-3.1) to handle the uploaded file(s). If you want the field to be Required you can annotate with `[Required]`.

ChameleonForms doesn't currently support generating the correct HTML to handle multiple file uploads, but feel free to submit an [issue](https://github.com/MRCollective/ChameleonForms/issues) or [pull request](https://github.com/MRCollective/ChameleonForms/pulls) to discuss adding that feature.

## Default HTML

When using the Default Field Generator then the default HTML of the [Field Element](field-element.md) will be:

```html
<input %validationAttrs% %htmlAttributes% id="%propertyName%" name="%propertyName%" type="file" value="%value%" />
```

If the field is marked as `[Required]` then `required="required"` will be added.
