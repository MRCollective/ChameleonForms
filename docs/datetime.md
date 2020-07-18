# Datetime Fields

If you need to collect DateTime data you can use a `DateTime` or `DateTime?` model property, e.g.:

```cs
public DateTime DateTimeField { get; set; }
public DateTime? NullableDateTimeField { get; set; }
[DisplayFormat(DataFormatString = "{0:d/M/yyyy}", ApplyFormatInEditMode = true)]
public DateTime DateTimeFieldWithFormat { get; set;
```

There only difference between a `DateTime` and `DateTime?`is that a `DateTime?` will not be required (unless it's annotated with `[Required]`).

## Default HTML

When using the Default Field Generator then the default HTML of the [Field Element](field-element.md) will be:

```html
<input %validationAttrs% %htmlAttributes% id="%propertyName%" name="%propertyName%" required="required" type="text" value="%value%" />
```

If you specify a `DateTime?` without a `[Required]` attribute then it will be:

<input %validationAttrs% %htmlAttributes% id="%propertyName%" name="%propertyName%" type="text" value="%value%" />

If you specify a `[DisplayFormat]` with `ApplyFormatInEditMode = true` then it will change the HTML to:

```html
<input %validationAttrs% data-val-format="%formatString%" %htmlAttributes% id="%propertyName%" name="%propertyName%" type="text" value="%value%" />
```

To find out why see the [Client-side validation](#client-side-validation) section below.

## Why we use type="text"

We deliberately don't make use of the various date-related HTML5 types, and instead opt for use of `type="text"`, because [browser support for controlling formatting](https://developer.mozilla.org/en-US/docs/Web/HTML/Date_and_time_formats) is limited. If you want to opt into a different type then you can by chaining the `.Attr("type", "adifferenttype")` on your field definition.

## Server-side validation and binding

By default MVC will attempt to parse a `DateTime` with whatever culture the thread is running as. This has a number of implications:

* If you want just a time, or just a date then you can't enforce that
* If you have a different culture on a particular server then you will get different results
* It's not very clear to a developer that doesn't know about that MVC behaviour what will happen
* You don't have much control over the format that you want your users to enter the date in

MVC provides the `[DisplayFormat]` attribute above, but that doesn't actually do anything apart from formatting the date nicely when the field is outputted (when using Editor Templates or Tag Helpers).

In order to provide a nice server-side validation experience, ChameleonForms provides first class support for `[DisplayFormat(ApplyFormatInEditMode = true)]`:

* It will output the date using the format string so a pre-populated model will automatically show the correct string to the user
* It provides a model binder that will perform server-side binding and validation using that format string as a guide when parsing the text the user entered
    * This [model](https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms/ModelBinders/DateTimeModelBinderProvider.cs) [binder](https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms/ModelBinders/DateTimeModelBinder.cs) is [registered by default](configuration.md#default-global-config), but you can turn it off

If you'd like to register the model binder yourself rather than using the global config you can do that like so:

```cs
    public void ConfigureServices(IServiceCollection services)
    {
        ...
        services.AddChameleonForms(b => b.WithoutDateTimeBinding());
        services.Configure<MvcOptions>(x =>
        {
            x.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());
            ...
        });
    }
```

## `g` format

There is support in ChameleonForms for a `g` format that uses the current thread's culture to determine the format string. That format string will then be passed to client validation so the end user has a consistent experience against what the server is expecting.

For instance, here are a couple of examples of what format will be used if you set your display format string to `{0:g}`:

* **en-GB**: `dd/MM/yyyy HH:mm`
* **uk-UA**: `dd.MM.yyyy H:mm`

Client-side validation
----------------------

Please see the [client-side validation of DateTime fields](datetime-client-side-validation.md) documentation.
