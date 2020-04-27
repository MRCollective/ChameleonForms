# Using different form templates

## Default form template

The `DefaultFormTemplate` outputs a sensible default that looks good without any / with minimal CSS and is a nice way to semantically describe a form. It makes use of definition lists to do this.

See [Configuring the Default Form Template](the-form#configuring-the-default-form-template) / [Default global config](configuration.md#default-global-config).

To see examples of the HTML this template outputs check out:

* [Form HTML](the-form.md#default-html)
* [Message HTML](the-message.md#default-html) - including the documentation about how the ChameleonForms `MessageType` maps to the Twitter Bootstrap Emphasis Styles
* [Section HTML](the-section.md#default-html) - including top-level and nested sections
* [Navigation HTML](the-navigation.md#default-html) - including how to add submit and reset buttons
* [Field HTML](field.md#default-html) - including:
    * The HTML it uses to layout the field sub-components
    * The HTML it uses for hints
    * The default required designator HTML ([which can be overriden](custom-template.md#field))
    * The HTML for nested fields

## Twitter Bootstrap 3 form template

See [Twitter Bootstrap 3 template](bootstrap-template.md).

## Custom templates

See [Creating custom form templates](custom-template.md).

## Custom extension method

If you would like to change the template being used across one or more forms, or would like a method name that is more meaningful to your application than `BeginChameleonForm`, you can simply define you own extension method instead, e.g.:

```csharp
    public static class FormHelpers
    {
        public static IForm<TModel> BeginMyApplicationNameForm<TModel>(this HtmlHelper<TModel> htmlhelper, string action = "", FormMethod method = FormMethod.Post, HtmlAttributes htmlAttributes = null, EncType? enctype = null)
        {
            return new Form<TModel>(htmlhelper, new MyCustomFormTemplate(), action, method, htmlAttributes, enctype);
        }
    }
```

If you'd like to use the [globally configured](configuration.md#default-global-config) default form template rather than newing up the template as shown above then you can use the same code in the `BeginChameleonForm` extension method to resolve the template:

```cs
htmlHelper.GetDefaultFormTemplate()
```

Which is an extension method on `IHtmlHelper` in the `ChameleonForms` namespace, which resolves `IFormTemplate` from the request services collection:

```cs
        /// <summary>
        /// Gets the registered default form template from RequestServices.
        /// </summary>
        /// <param name="htmlHelper">The HTML Helper</param>
        /// <returns>An instance of the default <see cref="IFormTemplate"/></returns>
        public static IFormTemplate GetDefaultFormTemplate(this IHtmlHelper htmlHelper)
        {
            return htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<IFormTemplate>();
        }
```

## Using multiple templates in a single application

If you want to use different form templates across multiple forms in a single application then you can't make use of `FormTemplate.Default` and you will need to create multiple extension methods (as shown above) for each form template you want to use. Then you opt in to a particular type of template by using the corresponding extension method for a given form.
