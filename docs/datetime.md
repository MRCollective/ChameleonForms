Datetime Fields
===============

If you need to collect DateTime data you can use a `DateTime` or `DateTime?` model property, e.g.:

```c#
public DateTime DateTimeField { get; set; }
[DisplayFormat(DataFormatString = "{0:d/M/yyyy}", ApplyInEditMode = true)]
public DateTime DateTimeFieldWithFormat { get; set;
```

There is no difference between a `DateTime` and `DateTime?` except the latter will not be Required unless it's annotated with `[Required]`.

Default HTML
------------

When using the Default Field Generator then the default HTML of the [Field Element](field-element) will be:

```html
<input %validationAttrs% %htmlAttributes% id="%propertyName%" name="%propertyName%" type="text" value="%value%" />
```

If you specify a `[DisplayFormat]` then it will change the HTML to:

```html
<input %validationAttrs% data-val-format="%formatString%" %htmlAttributes% id="%propertyName%" name="%propertyName%" type="text" value="%value%" />
```

To find out why see the [Client-side validation](#Client-side_validation) section below.

Server-side validation and binding
----------------------------------

By default MVC will attempt to parse a `DateTime` with whatever culture the thread is running as. This has a number of implications:

* If you want just a time, or just a date then you can't enforce that
* If you have a different culture on a particular machine then you will get different results
* It's not very clear to a developer that doesn't know about that MVC behaviour what will happen
* You don't have much control over the format that you want your users to enter the date in

MVC provides the `[DisplayFormat]` attribute above, but that doesn't actually do anything apart from formatting the date nicely when the field is outputted (when using Editor Templates).

In order to provide a nice server-side validation experience, ChameleonForms provides first class support for `[DisplayFormat]`:

* It will output the date using the format string just like Editor Templates
* It provides a model binder that will perform server-side validation using that format string as a guide
    * By default, when you install the NuGet package this model binder will be registered for `DateTime` and `DateTime?` properties

You should be able to see if you have the model binder registered by searching for the `App_Start\RegisterChameleonFormsComponents.cs` file, which should look something like:

```c#
using ChameleonForms.ModelBinders;
using System;
using System.Web.Mvc;

[assembly: WebActivator.PreApplicationStartMethod(typeof(%yourNamespace%.App_Start.RegisterChameleonFormsComponents), "Start")]
 
namespace %yourNamespace%.App_Start
{
    public static class RegisterChameleonFormsComponents
    {
        public static void Start()
        {
            ModelBinders.Binders.Add(typeof(DateTime), new DateTimeModelBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), new DateTimeModelBinder());
            // ...
        }
    }
}
```

Alternatively, you can simply register the model binder yourself by adding the following to `Application_Start` (or a method it calls):

```c#
ModelBinders.Binders.Add(typeof(DateTime), new DateTimeModelBinder());
ModelBinders.Binders.Add(typeof(DateTime?), new DateTimeModelBinder());
```

Client-side validation
----------------------

Please see the [client-side validation of DateTime fields](datetime-client-side-validation) documentation.